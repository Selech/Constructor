using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class BuildZoneScript : MonoBehaviour {

	private BoxCollider box;

	// Use this for initialization
	void Awake () {
		box = GetComponent<BoxCollider>();
	}

	public bool Contains(Vector3 position) {
		return box.bounds.Contains(position);
	}
}
