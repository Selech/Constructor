using UnityEngine;
using System.Collections;

public class PreviewScript : MonoBehaviour {

	public GameObject model;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// up
		if (Input.GetKey(KeyCode.UpArrow)) {
			model.transform.Rotate(new Vector3(2f,0,0));
		}

		// down
		if (Input.GetKey(KeyCode.DownArrow)) {
			model.transform.Rotate(new Vector3(-2f,0,0));
		}

		// left
		if (Input.GetKey(KeyCode.LeftArrow)) {
			model.transform.Rotate(new Vector3(0f,2f,0));
		}

		// right
		if (Input.GetKey(KeyCode.RightArrow)) {
			model.transform.Rotate(new Vector3(0f,-2f,0));
		}

		// zoom in
		if (Input.GetKey(KeyCode.Plus)) {
			model.transform.localScale += new Vector3(1f,1f,1f);
		}

		// zoom out
		if (Input.GetKey(KeyCode.Minus)) {
			model.transform.localScale -= new Vector3(1f,1f,1f);;
		}
	}
}
