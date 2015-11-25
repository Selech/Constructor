using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	public GameObject Introduction;
	public GameObject Controls;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowInstructions(){
		Introduction.SetActive (!Introduction.activeSelf);
		Controls.SetActive (!Controls.activeSelf);
	}

	public void GoToOverview(){
		Camera.main.GetComponent<MenuMouse> ().enabled = true;
		Camera.main.GetComponent<Animator> ().SetTrigger ("Overview");
	}
}
