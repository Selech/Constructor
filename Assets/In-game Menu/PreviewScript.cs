using UnityEngine;
using System.Collections;

public class PreviewScript : MonoBehaviour {

	public GameObject SpaceshipPart;
	public GameObject SmallOvenModel;
	public GameObject Bonfire;

	private GameObject model;

	// Use this for initialization
	void OnEnable () {
		model.transform.rotation = new Quaternion ();
	}
	
	// Update is called once per frame
	void Update () {
		// up
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			model.transform.Rotate(new Vector3(2f,0,0));
		}

		// down
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			model.transform.Rotate(new Vector3(-2f,0,0));
		}

		// left
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			model.transform.Rotate(new Vector3(0f,2f,0));
		}

		// right
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
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

	public void SelectModel(int i){
		SpaceshipPart.SetActive (false);
		SmallOvenModel.SetActive (false);
		Bonfire.SetActive (false);

		switch(i){
		case 0:
			model = SpaceshipPart;
			SpaceshipPart.SetActive (true);
			break;
		case 1: 
			model = SmallOvenModel;
			SmallOvenModel.SetActive (true);
			break;
		case 2:
			model = Bonfire;
			Bonfire.SetActive (true);
			break;
		}

		model.transform.rotation = new Quaternion ();
	}
}
