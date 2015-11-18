using UnityEngine;
using System.Collections;

public class StarRotation : MonoBehaviour {

	public float rotation;
	private bool planetView;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!planetView)
			transform.Rotate (new Vector3(0f, 0f, rotation));


	}
}
