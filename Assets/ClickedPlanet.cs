using UnityEngine;
using System.Collections;

public class ClickedPlanet : MonoBehaviour {

	public string trigger;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Clicked(){
		print ("Clicked");
		Camera.main.GetComponent<MenuMouse> ().enabled = false;
		Camera.main.GetComponent<Animator> ().SetTrigger (trigger);
	}
}
