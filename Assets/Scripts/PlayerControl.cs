using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class PlayerControl: MonoBehaviour
{
	public Camera cam;
	public float lookSpeed;
	public float speed;
	public float vAngle;
	public float jumpForce;

	private int hitcount;
	
	// Use this for initialization
	void Start ()
	{

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
			//Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, yRotation, 0), Time.deltaTime);
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
		Vector3 v2 = new Vector3();
		// forward
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			v2 += speed * this.transform.forward;
		}
		// backwards
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			v2 -= speed * this.transform.forward;
		}
		// left
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			v2 -= speed * this.transform.right;
		}
		// right
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			v2 += speed * this.transform.right;
		}
		// jump
		if (Input.GetKeyDown(KeyCode.Space)) {
			v2 += jumpForce * Vector3.up;
		}
		this.GetComponent<Rigidbody>().AddForce(v2,ForceMode.VelocityChange);
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
