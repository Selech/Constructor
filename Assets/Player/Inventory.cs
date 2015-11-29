using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour {
	
	public Dictionary<BlockType, int> collected;
	public Text AmountOfDirt;
	public Text AmountOfStone;
	public Text AmountOfWood;
	public Image UIMarker;
	
	private GameObject stone;
	private GameObject dirt;
	private GameObject wood;
	
	private BlockType chosen;
	
	// Use this for initialization
	void Start () {
		chosen = BlockType.Dirt;
		collected = new Dictionary<BlockType, int> ();
		foreach (BlockType bt in Enum.GetValues(typeof(BlockType))) {
			collected[bt] = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			chosen = BlockType.Dirt;
			UIMarker.rectTransform.localPosition = new Vector3(-190,50,0);
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			chosen = BlockType.Stone;
			UIMarker.rectTransform.localPosition = new Vector3(-95,50,0);
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			chosen = BlockType.Wood;
			UIMarker.rectTransform.localPosition = new Vector3(0,50,0);
		}
	}
	
	public void AddBlock(BlockType type){
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

	public BlockType GetChosen() {
		return chosen;
	}

	private void UpdateText(){
		AmountOfDirt.text = collected[BlockType.Dirt] + "";
		AmountOfStone.text = collected[BlockType.Stone] + "";
		AmountOfWood.text = collected[BlockType.Wood] + "";
	}
}
