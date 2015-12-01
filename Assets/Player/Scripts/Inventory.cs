﻿using UnityEngine;
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
	public GameObject blueprint;
	private byte chosen;
	
	// Use this for initialization
	void Start () {
		chosen = 1;
		UIMarker.rectTransform.localPosition = new Vector3(-190,50,0);
		collected = new Dictionary<byte, int> ();
		collected[1] = 0;
		collected[2] = 0;
		collected[3] = 0;

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			chosen = 0;
			UIMarker.rectTransform.localPosition = new Vector3(-300,50,0);
		}
		else if (Input.GetKeyDown (KeyCode.Alpha1)) {
			chosen = BlockType.DIRT;
			UIMarker.rectTransform.localPosition = new Vector3(-190,50,0);
		}
		else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			chosen = BlockType.STONE;
			UIMarker.rectTransform.localPosition = new Vector3(-95,50,0);
		}
		else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			chosen = BlockType.WOOD;
			UIMarker.rectTransform.localPosition = new Vector3(0,50,0);
		}
	}
	
	public void AddBlock(byte type){
		collected[type]++;
		UpdateBlockText ();
	}
	
	// returns false if inventory have no selected blocks left
	public bool RemoveBlock() {
		if (collected[chosen] <= 0) {
			return false;
		}
		else {
			collected[chosen]--;
			UpdateBlockText ();
			return true;
		}
	}

	public BlueprintScript GetBlueprint() {
		return blueprint.GetComponent<BlueprintScript>();
	}

	public bool IsBlueprintChosen() {
		return chosen == 0 && blueprint != null;
	}

	public byte GetChosenBlock() {
		return chosen;
	}

	private void UpdateBlockText(){
		AmountOfDirt.text = collected[1] + "";
		AmountOfStone.text = collected[2] + "";
		AmountOfWood.text = collected[3] + "";
	}
}