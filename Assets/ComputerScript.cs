using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComputerScript : MonoBehaviour {

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
		print ("Should load blueprint");
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
