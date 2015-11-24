using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
[RequireComponent (typeof (Inventory))]
public class PlayerControl: MonoBehaviour
{
	public Camera cam;
	public float lookSpeed;
	public float vAngle;
	public float walkSpeed;
	public float maxSpeed;
	public float breakingSpeed;
	public float airSpeedAmp;
	public float jumpForce;
	public float miningSpeed;
	public float miningPower;
	private int hitcount;
	private float distToGround;
	private Inventory inventory;
	private GameObject container;
	private DateTime lastAction;

	private float energy;
	private float maxEnergy = 100;
	public Slider energyBar;

	public ParticleSystem ps;
	
	// Use this for initialization
	void Start ()
	{
		Cursor.visible = false;

		CapsuleCollider collider = GetComponent<CapsuleCollider>();
		inventory = GetComponent<Inventory>();
		container = GameObject.Find ("Blocks");
		distToGround = collider.bounds.extents.y;

		energy = maxEnergy;
	}

	void Update(){
		UpdateLook();
		UpdateAction();

		Ray ray = new Ray (this.transform.position, this.transform.up);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.tag == "Sun") {
				if(energy < maxEnergy){
					energy += 1f;
				}
			} else {
				energy -= 0.05f;
			}
		}
		float newEnergy = (energy / maxEnergy);
		energyBar.value = newEnergy;

		if (Input.GetKey(KeyCode.Escape)) {
			Application.LoadLevel("Menu");
		}
	}

	void FixedUpdate ()
	{
		UpdateMovement();
	}

	void UpdateLook() {
		// horizontal turning
		float mx = Input.GetAxis ("Mouse X");
		if (mx != 0) {
			float yRotation = mx * lookSpeed;
			yRotation += transform.rotation.eulerAngles.y;
			transform.rotation = Quaternion.Euler(0, yRotation, 0);
		}
		
		// vertical head rotation
		float my = Input.GetAxis ("Mouse Y");
		if (my != 0) {
			float xRotation = (cam.transform.rotation.eulerAngles.x + vAngle) % 360;
			xRotation += -my * lookSpeed;
			xRotation = Mathf.Clamp (xRotation, 0, 2 * vAngle);
			cam.transform.localRotation = Quaternion.Euler (xRotation - vAngle, 0, 0);
		}
	}

	void UpdateMovement() {
		bool moving = false;
		Rigidbody rb = GetComponent<Rigidbody>();
		// if in air the movement speed is reduced
		float amp = IsGrounded() ? 1f : airSpeedAmp;
		// forward
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			moving = true;
			rb.AddForce(this.transform.forward * walkSpeed * amp);
		}
		// backwards
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			moving = true;
			rb.AddForce(-this.transform.forward * walkSpeed * amp);
		}
		// left
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			moving = true;
			rb.AddForce(-this.transform.right * walkSpeed * amp);
		}
		// right
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			moving = true;
			rb.AddForce(this.transform.right * walkSpeed * amp);
		}
		// jump
		if (Input.GetKey(KeyCode.Space) && IsGrounded()) {
			rb.AddForce(ps.transform.position = this.transform.up * jumpForce);
		}

		// control horizontal max speed
		Vector2 horizontalVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
		if (horizontalVelocity.magnitude > maxSpeed) {
			horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
			rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.y);
		}
		// deaccelerates player when not moving and standing on the ground
		if (!moving && IsGrounded()) {
			horizontalVelocity *= 1 - breakingSpeed;
			rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.y);
		}
	}

	bool IsGrounded() {
		return Physics.Raycast(this.transform.position, -Vector3.up, distToGround + 0.05f);
	}

	private bool canPerformAction() {
		// Time difference in milli seconds
		long deltaTime = (DateTime.Now.Ticks - lastAction.Ticks) / TimeSpan.TicksPerMillisecond;
		return deltaTime > (1000 / miningSpeed);
	}

	public void GameStart(){
		Cursor.visible = false;
	}

	public void GameStop(){
		Cursor.visible = true;
		Application.Quit ();
	}

	void UpdateAction ()
	{
		// left click
		if (Input.GetMouseButton (0) && canPerformAction()) {
			Ray ray = new Ray (cam.transform.position, cam.transform.forward);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				string tag = hit.collider.gameObject.tag;
				if (tag == "Collectable") {
					BlockScript bs = hit.collider.gameObject.GetComponent<BlockScript>();
					// Check if block breaks
					ps.transform.position = hit.point;
					ps.Play ();

					energy -= 0.05f;
					inventory.AddBlock(bs.type);

					BuildZoneScript bz = GameObject.Find("Build Zone").GetComponent<BuildZoneScript>();
					if(GameObject.Find("Build Zone").GetComponent<BuildZoneScript>().Contains(bs.gameObject.transform.position)){
						bz.BlockRemoved(bs.gameObject);
					}

					GameObject.Find("Map").GetComponent<MapGenerator>().UpdateQuad(hit.collider.gameObject);

					// CoOlDoWn
					lastAction = DateTime.Now;
				}
			}
		}

		if(Input.GetMouseButtonDown(0)){
			Ray ray = new Ray (cam.transform.position, cam.transform.forward);
			Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.green);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				string tag = hit.collider.gameObject.tag;
				if (tag == "Button") {
					Button b = hit.collider.gameObject.GetComponent<Button>();
					b.onClick.Invoke();
				}
			}
		}

		// right click
		if (Input.GetMouseButtonDown (1)) {
			Ray ray = new Ray (cam.transform.position, cam.transform.forward);
			RaycastHit hit;
		
			if (Physics.Raycast (ray, out hit)) {
				// Checks if the clicked object can be build upon
				if (hit.collider.gameObject.tag == "Buildable" || hit.collider.gameObject.tag == "Collectable") {
					// Checks if inventory have any of the chosen blocks left
					if (inventory.RemoveBlock()) {
						Vector3 blockCoord = hit.collider.gameObject.GetComponent<Buildable>().GetBlockPosition(hit);
						GameObject block = BlockScript.Create(inventory.GetChosen(), blockCoord, container);

						BuildZoneScript bz = GameObject.Find("Build Zone").GetComponent<BuildZoneScript>();
						if(GameObject.Find("Build Zone").GetComponent<BuildZoneScript>().Contains(block.transform.position)){
							bz.BlockAdded(block);
						}

					}
				}
			}
		}
	}
}
