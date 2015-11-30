using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour {
	
	public Dictionary<byte, int> collected;
	public Text AmountOfDirt;
	public Text AmountOfStone;
	public Text AmountOfWood;
	public Image UIMarker;
	
	private GameObject stone;
	private GameObject dirt;
	private GameObject wood;
	
	private byte chosen;
	
	// Use this for initialization
	void Start () {
		chosen = 1;
		collected = new Dictionary<byte, int> ();

		collected[0] = 0;
		collected[1] = 0;
		collected[2] = 0;
		collected[3] = 0;

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			chosen = 1;
			UIMarker.rectTransform.localPosition = new Vector3(-190,50,0);
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			chosen = 2;
			UIMarker.rectTransform.localPosition = new Vector3(-95,50,0);
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			chosen = 3;
			UIMarker.rectTransform.localPosition = new Vector3(0,50,0);
		}
	}
	
	public void AddBlock(byte type){
		collected[type]++;
		UpdateText ();
	}

	// returns false if inventory have no selected blocks left
	public bool RemoveBlock() {
		if (collected[chosen] <= 0) {
			return false;
		}
		else {
			collected[chosen]--;
			UpdateText ();
			return true;
		}
	}

	public byte GetChosen() {
		return chosen;
	}

	private void UpdateText(){
		AmountOfDirt.text = collected[1] + "";
		AmountOfStone.text = collected[2] + "";
		AmountOfWood.text = collected[3] + "";
	}
}
