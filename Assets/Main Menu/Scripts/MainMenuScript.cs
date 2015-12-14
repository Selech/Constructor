using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	public GameObject Introduction;
	public GameObject Controls;

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetFloat("MaxEnergy", 100);
		PlayerPrefs.SetFloat("Range", 3.0f);
		PlayerPrefs.SetFloat("JumpJets", 6f);
		PlayerPrefs.SetFloat("MiningSpeed", 2f);

		PlayerPrefs.SetInt("Money", 0);
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
