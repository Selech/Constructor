using UnityEngine;
using System.Collections;

[RequireComponent (typeof(UIManager))]
public class GameManager : MonoBehaviour {

	private UIManager uim;
	private PlayerScript player;
	private PlayerControl control;
	private MapGenerator generator;


	// Use this for initialization
	void Start () {
		uim = GetComponent<UIManager>();
		player = GameObject.Find ("Player").GetComponent<PlayerScript> ();
		control = player.gameObject.GetComponent<PlayerControl> ();
		generator = GameObject.Find ("Map").GetComponent<MapGenerator> ();
		generator.GenerateMap ();
		//player.transform.position = generator.GetRandomSpawnPoint ();
		Unpause ();
	}
	
	// Update is called once per frame
	void Update () {
		// check if player is dead
		if (player.isDead ()) {
			uim.ShowDeathMenu ();
		} else {
			uim.SetEnergy(player.GetEnergy());
		}
	}

	public bool IsPaused() {
		return !player.enabled;
	}

	public void Pause() {
		Cursor.visible = true;
		control.enabled = false;
		player.enabled = false;
	}

	public void Unpause() {
		Cursor.visible = false;
		control.enabled = true;
		player.enabled = true;
	}
}
