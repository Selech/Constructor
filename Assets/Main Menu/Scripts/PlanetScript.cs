using UnityEngine;
using System.Collections;

public class PlanetScript : MonoBehaviour {

	public float rotationspeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3(0f, rotationspeed, rotationspeed));
	}
}
