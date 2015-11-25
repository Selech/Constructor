using UnityEngine;
using System.Collections;

public class DirtuCloud : MonoBehaviour {

	private int moveX;
	private int moveZ;
	private Vector3 target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		print (Vector3.Distance(this.transform.position, target));

		if(Vector3.Distance(this.transform.position, target) < 5){
			moveX = Random.Range (1, 74);
			moveZ = Random.Range (1, 74);

			target = new Vector3(this.transform.position.x + moveX, this.transform.position.y, this.transform.position.z + moveZ);
		}

		Ray ray = new Ray (this.transform.position, Vector3.down);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			target = new Vector3(target.x, hit.collider.gameObject.transform.position.y + 5, target.z);

			this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(target.x, hit.collider.gameObject.transform.position.y + 5, target.z), 0.025f);
		}
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Player") {
			other.GetComponent<PlayerControl>().Damage(1f);
		}
	}
}
