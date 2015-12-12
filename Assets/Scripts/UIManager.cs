using UnityEngine;
using UnityEngine.SceneManagement;
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
	public Text charging;
	public Text discharging;
	private float lastEnergy;


	// Use this for initialization
	void Start ()
	{
		gm = this.gameObject.GetComponent<GameManager> ();
	}

	// Update is called once per frame
	void Update ()
	{
		// Activate pause menu
		if (Input.GetKey(KeyCode.Escape) && !gm.IsPaused()) {
			HUD.SetActive(false);
			pauseMenu.SetActive(true);
			gm.Pause();

		}


	}

	public void SetEnergy (float val)
	{
		if(lastEnergy > val){
			charging.gameObject.SetActive(false);
			discharging.gameObject.SetActive(true);
		}
		else{
			charging.gameObject.SetActive(true);
			discharging.gameObject.SetActive(false);
		}
		lastEnergy = val;
		energyBar.value = val;
	}

	public void ShowDeathMenu() {
		deathMenu.SetActive(true);
		HUD.SetActive(false);
		gm.Pause ();
	}

	public void Restart(){
		pauseMenu.SetActive (false);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
	
	public void ReturnToShip(){
		pauseMenu.SetActive (false);
		SceneManager.LoadScene ("Menu");
	}
}
