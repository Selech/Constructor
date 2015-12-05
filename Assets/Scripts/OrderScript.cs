using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OrderScript : MonoBehaviour {
	
	private string ordername;
	public Button TakeOrder;
	public Button DeliverOrder;
	public Button DiscardOrder;
	public Text SelectedOrder;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void LoadBlueprint(){
		TakeOrder.interactable = false;
		DiscardOrder.interactable = false;

		GameObject player = GameObject.Find ("Player");
		player.GetComponent<PlayerScript> ().blueprintSelected = true;

		GameObject.Find ("Game Manager").GetComponent<GameManager>().Unpause();
	}
	
	public void SendBlueprint(){
		
	}
	
	public void DiscardBlueprint(){
		
	}
	
	public void SelectOrder(string ordername){
		this.ordername = ordername;
		
		SelectedOrder.text = "Selected order: " + this.ordername;
		
		TakeOrder.interactable = true;
	}
}
