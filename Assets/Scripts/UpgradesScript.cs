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
			PlayerPrefs.SetFloat("MiningSpeed", 2);

			PlayerPrefs.SetInt("Money", 0);
		}
	}

	private static void checker(){
		if(true){
			PlayerPrefs.SetFloat("MaxEnergy", 100);
			PlayerPrefs.SetFloat("Range", 3.0f);
			PlayerPrefs.SetFloat("JumpJets", 6f);
			PlayerPrefs.SetFloat("MiningSpeed", 2);

			PlayerPrefs.SetInt("Money", 0);
		}

		if(PlayerPrefs.GetFloat ("MaxEnergy") == 0){
			PlayerPrefs.SetFloat("MaxEnergy", 100);
			PlayerPrefs.SetFloat("Range", 3.0f);
			PlayerPrefs.SetFloat("JumpJets", 6f);
			PlayerPrefs.SetFloat("MiningSpeed", 2);
			
			PlayerPrefs.SetInt("Money", 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		range.text = "Upgrade range: " + (PlayerPrefs.GetFloat ("Range")) + ">" + (PlayerPrefs.GetFloat ("Range") + 1f);
		energy.text = "Upgrade energy: " + (PlayerPrefs.GetFloat ("MaxEnergy")) + ">" + (PlayerPrefs.GetFloat ("MaxEnergy") + 20f);
		jump.text = "Upgrade energy: " + (PlayerPrefs.GetFloat ("JumpJets")) + ">" + (PlayerPrefs.GetFloat ("JumpJets") + 3f);
		speed.text = "Upgrade mine speed: " + (PlayerPrefs.GetFloat ("MiningSpeed")) + ">" + (PlayerPrefs.GetFloat ("MiningSpeed") + 1f);

		if(PlayerPrefs.GetInt("Money") > 20){
			rangeButton.interactable = true;
			energyButton.interactable = true;
			jumpButton.interactable = true;
			speedButton.interactable = true;
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
		PlayerPrefs.SetFloat("JumpJets", PlayerPrefs.GetFloat ("JumpJets") + 3f);
		OnEnable ();
	}

	public void AddMiningSpeed(){
		PlayerPrefs.SetFloat("MiningSpeed", PlayerPrefs.GetFloat ("MiningSpeed") + 3f);
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

	public static float GetMiningSpeed(){
		return PlayerPrefs.GetFloat ("MiningSpeed");
	}
}
