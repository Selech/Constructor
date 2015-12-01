using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof(GameManager))]
public class UIManager : MonoBehaviour
{

	private GameManager gm;
	public GameObject HUD;
	public GameObject pauseMenu;
	public GameObject deathMenu;
	public Slider energyBar;

	// Use this for initialization
	void Start ()
	{
		gm = this.gameObject.GetComponent<GameManager> ();
	}

	// Update is called once per frame
	void Update ()
	{
		// Activate pause menu
		if (Input.GetKey(KeyCode.Escape)) {
			HUD.SetActive(false);
			pauseMenu.SetActive(true);
			gm.Pause();
		}
	}

	public void SetEnergy (float val)
	{
		energyBar.value = val;
	}

	public void ShowDeathMenu() {
		deathMenu.SetActive(true);
		HUD.SetActive(false);
		gm.Pause ();
	}

	public void Restart(){
		pauseMenu.SetActive (false);
		Application.LoadLevel (Application.loadedLevel);
	}
	
	public void ReturnToShip(){
		pauseMenu.SetActive (false);
		Application.LoadLevel ("Menu");
	}
}
