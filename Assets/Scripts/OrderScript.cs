using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OrderScript : MonoBehaviour {
	
	private string ordername;
	public Button TakeOrder;
	public Text SelectedOrder;

	public BlueprintScript bs;
	public PreviewScript ps;
	
	public void LoadBlueprint(){
		TakeOrder.interactable = false;

		GameObject player = GameObject.Find ("Player");
		player.GetComponent<PlayerScript> ().blueprintSelected = true;

		switch(this.ordername){
		case "SpaceshipPart":
			GameObject.Find ("SpaceshipPart").SetActive (false);
			break;
		case "SmallOven":
			GameObject.Find ("SmallOven").SetActive (false);
			break;
		case "Bonfire":
			GameObject.Find ("Bonfire").SetActive (false);
			break;
		}

		GameObject.Find ("Game Manager").GetComponent<GameManager>().Unpause();
	}
	
	public void SelectOrder(string ordername){
		this.ordername = ordername;
		
		SelectedOrder.text = "Selected order: " + this.ordername;
		
		TakeOrder.interactable = true;

		switch(ordername){
		case "SpaceshipPart":
			SpaceShipPart ();
			break;
		case "SmallOven":
			SmallOven ();
			break;
		case "Bonfire":
			Bonfire ();
			break;
		}
	}

	public void SpaceShipPart(){
		Vector3 size = new Vector3(3, 1, 2);

		var data = new byte[(int)size.x, (int)size.y, (int)size.z];
		data [0, 0, 0] = 2;
		data [1, 0, 0] = 0;
		data [2, 0, 0] = 2;
		data [0, 0, 1] = 2;
		data [1, 0, 1] = 3;
		data [2, 0, 1] = 2;

		bs.SelectOrder (data, size); 
		ps.SelectModel (0);
	}

	public void SmallOven(){
		Vector3 size = new Vector3(4, 2, 2);

		var data = new byte[(int)size.x, (int)size.y, (int)size.z];
		data [0, 0, 0] = 2;
		data [1, 0, 0] = 0;
		data [2, 0, 0] = 0;
		data [3, 0, 0] = 2;
		data [0, 0, 1] = 2;
		data [1, 0, 1] = 2;
		data [2, 0, 1] = 2;
		data [3, 0, 1] = 2;

		data [0, 1, 0] = 1;
		data [1, 1, 0] = 1;
		data [2, 1, 0] = 1;
		data [3, 1, 0] = 1;
		data [0, 1, 1] = 1;
		data [1, 1, 1] = 1;
		data [2, 1, 1] = 1;
		data [3, 1, 1] = 1;

		bs.SelectOrder (data, size); 
		ps.SelectModel (1);
	}

	public void Bonfire(){
		Vector3 size = new Vector3(4, 1, 4);

		var data = new byte[(int)size.x, (int)size.y, (int)size.z];
		data [0, 0, 0] = 2;
		data [1, 0, 0] = 2;
		data [2, 0, 0] = 2;
		data [3, 0, 0] = 2;
		data [0, 0, 1] = 2;
		data [1, 0, 1] = 0;
		data [2, 0, 1] = 0;
		data [3, 0, 1] = 2;

		data [0, 0, 2] = 2;
		data [1, 0, 2] = 0;
		data [2, 0, 2] = 0;
		data [3, 0, 2] = 2;
		data [0, 0, 3] = 2;
		data [1, 0, 3] = 2;
		data [2, 0, 3] = 2;
		data [3, 0, 3] = 2;

		bs.SelectOrder (data, size); 
		ps.SelectModel (2);
	}
}
