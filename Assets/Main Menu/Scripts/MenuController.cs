using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour {

	public GameObject DirtusCanvas;
	public GameObject PlantusCanvas;
	public GameObject MystosCanvas;
	public GameObject IsosCanvas;

	public void OpenPlanetCanvas(string state){
		switch (state) {
		case "Dirtus":
			DirtusCanvas.SetActive (true);
			break;
		case "Plantus":
			PlantusCanvas.SetActive (true);
			break;
		case "Mystos":
			MystosCanvas.SetActive (true);
			break;
		case "Isos":
			IsosCanvas.SetActive (true);
			break;
		default:
			break;
		}
	}

	public void Deploy(string planet) {
		SceneManager.LoadScene("Dirtus");
	}

	public void BackToOverview(){
		Camera.main.GetComponent<Animator> ().SetTrigger ("Overview");
	}
}
