using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class BuildZoneScript : MonoBehaviour {

	public BoxCollider box;

	// Use this for initialization
	void Start () {
		box = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool Contains(Vector3 position) {
		return box.bounds.Contains(position);
	}
}
