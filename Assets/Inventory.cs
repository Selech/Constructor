using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	
	public Dictionary<string,int> collected;
	
	public Text AmountOfDirt;
	public Text AmountOfStone;
	public Text AmountOfWood;
	public Image UIMarker;
	
	public GameObject stone;
	public GameObject dirt;
	public GameObject wood;
	
	public string chosen;
	
	// Use this for initialization
	void Start () {
		collected = new Dictionary<string, int> ();
		collected.Add ("dirt",0);
		collected.Add ("wood",0);
		collected.Add ("stone",0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			chosen = "dirt";
			UIMarker.rectTransform.localPosition = new Vector3(-190,50,0);
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			chosen = "stone";
			UIMarker.rectTransform.localPosition = new Vector3(-95,50,0);
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			chosen = "wood";
			UIMarker.rectTransform.localPosition = new Vector3(0,50,0);
		}
	}
	
	public void AddBlock(string block){
		collected[block] += 1;
		UpdateText ();
	}
	
	private void UpdateText(){
		AmountOfDirt.text = collected["dirt"]+"";
		AmountOfStone.text = collected["stone"]+"";
		AmountOfWood.text = collected["wood"]+"";
	}
	
	public void PlaceBlock(Vector3 position){
		if (collected [chosen] > 0) {
			GameObject g = GetBlock (chosen);
			g.transform.position = position;
			g.transform.parent = GameObject.Find ("Map").transform;
			
			collected [chosen] -= 1;
			UpdateText ();
		}
	}
	
	private GameObject GetBlock(string s){
		if(s == "stone"){
			return Instantiate(stone);
		}
		
		if(s == "wood"){
			return Instantiate(wood);
		}
		
		if(s == "dirt"){
			return Instantiate(dirt);
		}
		
		return Instantiate(dirt);
	}
	
}
