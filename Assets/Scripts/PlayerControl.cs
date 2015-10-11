using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
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
	private int hitcount;
	private float distToGround;
	
	// Use this for initialization
	void Start ()
	{
		CapsuleCollider collider = GetComponent<CapsuleCollider>();
		distToGround = collider.bounds.extents.y;
	}

	void FixedUpdate ()
	{
		UpdateLook();
		UpdateMovement();
		UpdateAction();
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
			rb.AddForce(this.transform.up * jumpForce);
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

	void UpdateAction ()
	{
		// left click
		if (Input.GetMouseButton (0)) {
			hitcount--;
			if (hitcount < 0) {
				Ray ray = new Ray (cam.transform.position, cam.transform.forward);
				RaycastHit hit;
					
				if (Physics.Raycast (ray, out hit)) {
					if (hit.collider.gameObject.tag == "Block") {
						hit.collider.gameObject.GetComponent<BlockScript> ().Hit (this.gameObject, hit.point);
					}
					hitcount = 10;
				}
			}
		}
		// right click
		if (Input.GetMouseButtonDown (1)) {
			Ray ray = new Ray (cam.transform.position, cam.transform.forward);
			RaycastHit hit;
		
			if (Physics.Raycast (ray, out hit)) {
				Vector3 result = new Vector3 (0, 0, 0);
			
				Vector3 C = hit.collider.gameObject.transform.position;
				Vector3 I = hit.point;
			
				if (I.x == C.x + 0.5f) {
					result = C + new Vector3 (1f, 0f, 0f);
				
				}
			
				if (I.x == C.x - 0.5f) {
					result = C + new Vector3 (-1f, 0f, 0f);
				
				}
			
				if (I.y == C.y + 0.5f) {
					result = C + new Vector3 (0f, 1f, 0f);
				
				}
			
				if (I.y == C.y - 0.5f) {
					result = C + new Vector3 (0f, -1f, 0f);
				
				}
			
				if (I.z == C.z + 0.5f) {
					result = C + new Vector3 (0f, 0f, 1f);
				
				}
			
				if (I.z == C.z - 0.5f) {
					result = C + new Vector3 (0f, 0f, -1f);
				
				}
			
				this.gameObject.GetComponent<Inventory> ().PlaceBlock (result);
			}
		}
	}
}
