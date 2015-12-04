﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[RequireComponent (typeof (PlayerControl))]
public class PlayerScript : MonoBehaviour {
	public float miningSpeed;
	public float miningPower;
	private Inventory inventory;
	private DateTime lastAction;
	private float energy;
	private float maxEnergy;
	private float range;
	public Light flashlight;
	public Camera cam;
	public ParticleSystem ps;
	public ParticleSystem electricPs;
	private PlayerControl control;

	public GameObject water;
	public GameObject cube;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		control = GetComponent<PlayerControl>();
		inventory = GetComponent<Inventory>();

		UpdateStats();
		energy = maxEnergy;
	}

	public bool isDead() {
		return energy <= 0;
	}
	
	// Update is called once per frame
	void Update () {
		// turn on/off flashlight
		if (Input.GetKeyDown (KeyCode.L)) {
			flashlight.enabled = !flashlight.enabled;
		}

		//If flash light is on, then drain more energy
		if(flashlight.enabled){
			energy -= 0.08f;
		}

		// check if player is under the sun
		Ray ray = new Ray (this.transform.position, this.transform.up);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.tag == "Sun" && !(cam.transform.position.y < water.transform.position.y)) {
				if(energy < maxEnergy){
					float gain = 30 / hit.distance + 0.25f;
					energy += gain;
				}
			}
			else if (hit.collider.tag == "DirtuCloud") {
				if(hit.distance < 10f){
					energy -= 1f;
				}
			}
			else {
				energy -= 0.07f;
			}
		}

		if (cam.transform.position.y < water.transform.position.y) {
			energy -= 0.4f;
			RenderSettings.fog = true;
			RenderSettings.fogColor = new Color (0, 0.15f, 0.2f, 0.5f);
			RenderSettings.fogDensity = 0.35f;
			RenderSettings.fogStartDistance = 0.0f;
		} else {
			RenderSettings.fog = true;
			RenderSettings.fogColor = new Color (0.2f, 0.0f, 0.0f, 0.5f);
			RenderSettings.fogDensity = 0.05f;
			RenderSettings.fogStartDistance = 10.0f;
		}

		UpdateAction();
	}

	public float GetEnergy() {
		return energy / maxEnergy;
	}

	public void Damage(float damage){
		energy -= damage;
	}
	
	public void UpdateStats(){
		maxEnergy = UpgradesScript.GetBattery ();
		control.SetJumpForce (UpgradesScript.GetJumpJets());
		range = UpgradesScript.GetRange ();
	}

	private bool canPerformAction() {
		// Time difference in milli seconds
		long deltaTime = (DateTime.Now.Ticks - lastAction.Ticks) / TimeSpan.TicksPerMillisecond;
		return deltaTime > (1000 / miningSpeed);
	}

	void UpdateAction ()
	{
		// raycast where crosshair is pointing
		Ray ray = new Ray (cam.transform.position, cam.transform.forward);
		RaycastHit hit;
		bool hasHit = Physics.Raycast (ray, out hit, maxDistance: 5.0f);

		BlueprintScript blueprint = inventory.GetBlueprint();


		// if nothing is hit, do nothing
		if (!hasHit) {
			if (inventory.IsBlueprintChosen ()) {
				blueprint.gameObject.SetActive(false);
			}
			cube.SetActive(false);

			return;
		}

		if(hasHit){
			if(hit.distance < 2.5f && hit.collider.gameObject.tag == "Collectable"){
				cube.SetActive(true);
				cube.GetComponent<BuildingBlock>().SetMaterial(0);
				cube.GetComponent<BuildingBlock>().SetHighlightPosition(hit.collider.gameObject);
			}
			else if(hit.collider.gameObject.tag == "Collectable" || hit.collider.gameObject.tag == "Buildable"){
				cube.SetActive(true);
				cube.GetComponent<BuildingBlock>().SetMaterial(inventory.GetChosenBlock());
				cube.GetComponent<BuildingBlock>().SetBlockPosition(hit.collider.gameObject);
			}
		}

		// if blueprint is equiped, highlight placement grid
		if (inventory.IsBlueprintChosen ()) {
			Vector3 pos = GetBlockPosition(hit);
			blueprint.MoveTo(pos);
			blueprint.gameObject.SetActive(true);
		} else {
			inventory.GetBlueprint().gameObject.SetActive(false);
		}


		// left click (mine)
		if (Input.GetMouseButton (0) && canPerformAction()) {
			if (hasHit) {
				string tag = hit.collider.gameObject.tag;
				if (tag == "Collectable") {
					BlockScript bs = hit.collider.gameObject.GetComponent<BlockScript>();
					
					switch(bs.type){
					case BlockType.STONE:
						ps.startColor = ToColor("DFDFDFFF");
						CollectableBlock (bs, hit.point);
						break;
						
					case BlockType.DIRT:
						ps.startColor = ToColor("CBBAA7FF");
						CollectableBlock (bs, hit.point);
						break;
						
					case BlockType.WOOD:
						ps.startColor = ToColor("A69568FF");
						CollectableBlock (bs, hit.point);
						break;
					case BlockType.ELECTRIC:
						electricPs.startColor = ToColor("A69568FF");
						RechargableBlock (hit.point);
						break;
						
					default:
						break;
					}
					
					// Cooldown
					lastAction = DateTime.Now;
				}
			}
		}
		
		// right click
//		if (Input.GetMouseButtonDown (1) && hasHit) {
//			// Checks if the clicked object can be build upon
//			if (hit.collider.gameObject.tag == "Buildable" || hit.collider.gameObject.tag == "Collectable") {
//				// Checks if inventory have any of the chosen blocks left
//				if (inventory.RemoveBlock()) {
//					GameObject.Find("Map").GetComponent<MapGenerator>().PlaceBlock(GetBlockPosition(hit), inventory.GetChosen());
//					
//				}
//			}
//		}
	}

	public void CollectableBlock(BlockScript bs, Vector3 hitpoint){
		ps.transform.position = hitpoint;
		ps.Play ();

		energy -= 0.05f;
		inventory.AddBlock(bs.type);
		
		GameObject.Find("Map").GetComponent<MapGenerator>().DestroyBlock(bs.gameObject);
	}

	public void RechargableBlock(Vector3 hitpoint){
		electricPs.Play ();
		
		energy += 1.6f;
	}

	public Vector3 GetBlockPosition(RaycastHit hit) {
		// Finds closest block coordinate
		float x = Mathf.Round(hit.point.x);
		float y = Mathf.Round(hit.point.y);
		float z = Mathf.Round(hit.point.z);
		
		if (hit.normal == Vector3.up)
			y = Mathf.Ceil(hit.point.y);
		else if (hit.normal == Vector3.down)
			y = Mathf.Floor(hit.point.y);
		else if (hit.normal == Vector3.right)
			x = Mathf.Ceil(hit.point.x);
		else if (hit.normal == Vector3.left)
			x = Mathf.Floor(hit.point.x);
		else if (hit.normal == Vector3.forward)
			z = Mathf.Ceil(hit.point.z);
		else if (hit.normal == Vector3.back)
			z = Mathf.Floor(hit.point.z);
		return new Vector3(x, y, z);
	}

	
	
	private Color ToColor(string hex){
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}
}
