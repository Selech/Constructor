using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = player.transform.position + new Vector3 (0, 3,-4);

		this.transform.LookAt (player.transform.position);
	}
}
