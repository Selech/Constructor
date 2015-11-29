using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradesScript : MonoBehaviour {

	public Text range;
	public Text energy;
	public Text jump;

	public Button rangeButton;
	public Button energyButton;
	public Button jumpButton;

	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetFloat ("MaxEnergy") == 0){
			PlayerPrefs.SetFloat("MaxEnergy", 100);
			PlayerPrefs.SetFloat("Range", 3.0f);
			PlayerPrefs.SetFloat("JumpJets", 25f);

			PlayerPrefs.SetInt("Money", 0);
		}

	}

	private static void checker(){
		if(PlayerPrefs.GetFloat ("MaxEnergy") == 0){
			PlayerPrefs.SetFloat("MaxEnergy", 100);
			PlayerPrefs.SetFloat("Range", 3.0f);
			PlayerPrefs.SetFloat("JumpJets", 25f);
			
			PlayerPrefs.SetInt("Money", 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		range.text = "Upgrade range: " + (PlayerPrefs.GetFloat ("Range")) + ">" + (PlayerPrefs.GetFloat ("Range") + 1f);
		energy.text = "Upgrade energy: " + (PlayerPrefs.GetFloat ("MaxEnergy")) + ">" + (PlayerPrefs.GetFloat ("MaxEnergy") + 20f);
		jump.text = "Upgrade energy: " + (PlayerPrefs.GetFloat ("JumpJets")) + ">" + (PlayerPrefs.GetFloat ("JumpJets") + 2f);

		if(PlayerPrefs.GetInt("Money") > 20){
			rangeButton.interactable = true;
			energyButton.interactable = true;
			jumpButton.interactable = true;
		}
	}

	public void AddRange(){
		PlayerPrefs.SetFloat("Range", PlayerPrefs.GetFloat ("Range") + 1f);
		OnEnable ();
	}

	public void AddBattery(){
		PlayerPrefs.SetFloat("MaxEnergy", PlayerPrefs.GetFloat ("MaxEnergy") + 20f);
		OnEnable ();
	}

	public void AddJumpJets(){
		PlayerPrefs.SetFloat("JumpJets", PlayerPrefs.GetFloat ("JumpJets") + 2f);
		OnEnable ();
	}

	public void AddShields(){
		
	}

	public static float GetBattery(){
		checker ();
		return PlayerPrefs.GetFloat ("MaxEnergy");
	}

	public static float GetRange(){
		checker ();
		return PlayerPrefs.GetFloat ("Range");
	}

	public static float GetJumpJets(){
		checker ();
		return PlayerPrefs.GetFloat ("JumpJets");
	}

	public static float GetShields(){
		return PlayerPrefs.GetFloat ("MaxEnergy");
	}
}
