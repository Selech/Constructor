using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradesScript : MonoBehaviour {

	public Text range;
	public Text energy;
	public Text jump;
	public Text speed;

	public static bool reset;

	public Button rangeButton;
	public Button energyButton;
	public Button jumpButton;
	public Button speedButton;

	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetFloat ("MaxEnergy") == 0){
			PlayerPrefs.SetFloat("MaxEnergy", 100);
			PlayerPrefs.SetFloat("Range", 3.0f);
			PlayerPrefs.SetFloat("JumpJets", 6f);
			PlayerPrefs.SetFloat("MiningSpeed", 2f);

			PlayerPrefs.SetInt("Money", 0);
		}
	}

	private static void checker(){
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		range.text = "Upgrade range: " + (PlayerPrefs.GetFloat ("Range")) + ">" + (PlayerPrefs.GetFloat ("Range") + 2f);
		energy.text = "Upgrade energy: " + (PlayerPrefs.GetFloat ("MaxEnergy")) + ">" + (PlayerPrefs.GetFloat ("MaxEnergy") + 30f);
		jump.text = "Upgrade jump force: " + (PlayerPrefs.GetFloat ("JumpJets")) + ">" + (PlayerPrefs.GetFloat ("JumpJets") + 5f);
		speed.text = "Upgrade mine speed: " + (PlayerPrefs.GetFloat ("MiningSpeed")) + ">" + (PlayerPrefs.GetFloat ("MiningSpeed") + 4f);

		if (PlayerPrefs.GetInt ("Money") > 20) {
			rangeButton.interactable = true;
			energyButton.interactable = true;
			jumpButton.interactable = true;
			speedButton.interactable = true;
		} else {
			rangeButton.interactable = false;
			energyButton.interactable = false;
			jumpButton.interactable = false;
			speedButton.interactable = false;
		}
	}

	public void AddRange(){
		PlayerPrefs.SetFloat("Range", PlayerPrefs.GetFloat ("Range") + 2f);
		GameObject.Find ("Player").GetComponent<Inventory> ().GiveMoney (-20);
		OnEnable ();
	}

	public void AddBattery(){
		PlayerPrefs.SetFloat("MaxEnergy", PlayerPrefs.GetFloat ("MaxEnergy") + 30f);
		GameObject.Find ("Player").GetComponent<Inventory> ().GiveMoney (-20);		
		OnEnable ();
	}

	public void AddJumpJets(){
		PlayerPrefs.SetFloat("JumpJets", PlayerPrefs.GetFloat ("JumpJets") + 5f);
		GameObject.Find ("Player").GetComponent<Inventory> ().GiveMoney (-20);		
		OnEnable ();
	}

	public void AddMiningSpeed(){
		PlayerPrefs.SetFloat("MiningSpeed", PlayerPrefs.GetFloat ("MiningSpeed") + 4f);
		GameObject.Find ("Player").GetComponent<Inventory> ().GiveMoney (-20);		
		OnEnable ();
	}

	public static float GetBattery(){
		return PlayerPrefs.GetFloat ("MaxEnergy");
	}

	public static float GetRange(){
		return PlayerPrefs.GetFloat ("Range");
	}

	public static float GetJumpJets(){
		return PlayerPrefs.GetFloat ("JumpJets");
	}

	public static float GetMiningSpeed(){
		return PlayerPrefs.GetFloat ("MiningSpeed");
	}
}
