using UnityEngine;
using System.Collections;

public class DirtuCloud : MonoBehaviour {

	private int moveX;
	private int moveZ;
	private Vector3 target;

	// Use this for initialization
	void Start () {
		moveX = Random.Range (1, 95);
		moveZ = Random.Range (1, 95);
		
		target = new Vector3(moveX, this.transform.position.y, moveZ);
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos = new Vector2(this.transform.position.x, this.transform.position.z);
		Vector2 target2D = new Vector2(target.x, target.z);

		if(Vector2.Distance(pos, target2D) < 5){
			moveX = Random.Range (1, 95);
			moveZ = Random.Range (1, 95);

			target = new Vector3(moveX, this.transform.position.y, moveZ);
		}

		target = new Vector3(target.x, this.transform.position.y, target.z);
		this.transform.position = Vector3.MoveTowards(this.transform.position, target, 0.05f);
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Player") {
			other.GetComponent<PlayerScript>().Damage(1f);
		}
	}
}
