using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowInstructions(){

	}

	public void GoToOverview(){
		Camera.main.GetComponent<MenuMouse> ().enabled = true;
		Camera.main.GetComponent<Animator> ().SetTrigger ("Overview");
	}
}
