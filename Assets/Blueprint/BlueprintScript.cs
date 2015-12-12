using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent (typeof(BoxCollider))]
public class BlueprintScript : MonoBehaviour
{
	public Material freeMat;
	public Material blockedMat;
	public Material floorMat;
	public Material dirt, stone, wood;
	public Button SelectOrderBtn;

	// Data of all supposed block positions in the blueprint
	public byte[,,] data;
	// Lists of actual block positions
	private byte[,,] blocks;
	// local position at which the blueprint will be placed
	private BoxCollider box;
	private Vector3 anchorPoint;
	private Vector3 dimensions;
	private bool IsPlacing;
	private Vector3 oldPos;
	public bool isPlaced;

	private GameObject floor;

	public Button preview;

	void Awake ()
	{
		box = this.GetComponent<BoxCollider> ();
		oldPos = this.transform.position;
		isPlaced = false;
		preview.interactable = false;
	}

	// should delete this
	void Start ()
	{
		
	}

	public void Update ()
	{
		// if the grid has changed position
		if (!isPlaced && PositionChanged ()) {
			oldPos = this.transform.position;
			UpdatePlacementGrid ();
		}
	}

	public void SelectOrder(byte[,,] data, Vector3 size){
		isPlaced = false;
		IsPlacing = true;

		SetDimensions (size, new Vector2 (1, 0));
		this.data = data;
	}

	private bool PositionChanged ()
	{
		Vector3 pos = this.transform.position;
		return (int)oldPos.x != (int)pos.x ||
			(int)oldPos.y != (int)pos.y ||
			(int)oldPos.z != (int)pos.z;
	}

	public void SetDimensions (Vector3 dim, Vector2 anchor)
	{
		blocks = new byte[(int)dim.x, (int)dim.y, (int)dim.z];
		this.anchorPoint = anchor;
		this.dimensions = dim;
		box.size = dim;
		box.center = (dim / 2.0f) - new Vector3 (0.5f, 0.5f, 0.5f);
		// test
		GameObject obj = new GameObject ("Filler");
		obj.transform.parent = this.transform;
		obj.transform.localScale = dim;
	}

	public void AddBlock (byte type, Vector3 globalPos)
	{
		int x = (int)(globalPos.x - transform.position.x);
		int y = (int)(globalPos.y - transform.position.y);
		int z = (int)(globalPos.z - transform.position.z);
		blocks [x, y, z] = type;
		if (IsFinished()) {
			Destroy(floor);
			Clear();
		}
	}

	private void Clear() {
		MapGenerator generator = GameObject.Find ("Map").GetComponent<MapGenerator> ();
		for (int x = 0; x < dimensions.x; x++) {
			for (int y = 0; y < dimensions.y; y++) {
				for (int z = 0; z < dimensions.z; z++) {
					Vector3 pos = (this.transform.position + new Vector3 (x, y, z));
					if (y == 0) {
						generator.DestroyBlueprintBlock (pos, true);
					} else {
						generator.DestroyBlueprintBlock (pos, false);
				
					}
				}
			}
		}
	}

	public bool IsFinished() {
		for (int x = 0; x < dimensions.x; x++) {
			for (int y = 0; y < dimensions.y; y++) {
				for (int z = 0; z < dimensions.z; z++) {
					if (data[x, y, z] != blocks[x, y, z]) {
						return false;
					}
				}
			}
		}
		preview.interactable = false;
		GameObject.Find ("Player").GetComponent<PlayerScript> ().GiveMoney (25);
		SoundSystem.PlaySound ("Cash-in");
		SelectOrderBtn.interactable = true;
		return true;
	}

	public void RemoveBlock (Vector3 globalPos)
	{
		int x = (int)(globalPos.x - transform.position.x);
		int y = (int)(globalPos.y - transform.position.y);
		int z = (int)(globalPos.z - transform.position.z);
		blocks [x, y, z] = BlockType.EMPTY;
		if (IsFinished()) {
			Destroy(floor);
			Clear();
		}
	}

	public bool Contains (Vector3 globalPos)
	{
		return box.bounds.Contains (globalPos);
	}

	public void MoveTo (Vector3 pos)
	{
		this.gameObject.transform.position = pos - anchorPoint;
	}

	public void UpdatePlacementGrid ()
	{
		// destroy the old grid
		foreach (Transform quad in this.transform) {
			GameObject.Destroy (quad.gameObject);
		}

		// calculate free tiles
		bool[,] frees;
		CalculatePlacementGrid (out frees);

		// create new grid
		for (int x = 0; x < (int)dimensions.x; x++) {
			for (int z = 0; z < (int)dimensions.z; z++) {
				CreateQuad (x, z, frees [x, z]);
			}
		}
	}

	public bool TryPlace (Vector3 pos)
	{
		bool[,] arr;
		// if can be placed
		if (CalculatePlacementGrid (out arr)) {
			isPlaced = true;
			foreach (Transform child in this.transform) {
				Destroy (child.gameObject);
			}
			GenerateFloor ();

			preview.interactable = true;
			GameObject.Find("Game Manager").GetComponent<UIManager>().HUD.SetActive(true);

			return true;
		}
		return false;
	}

	private bool CalculatePlacementGrid (out bool[,] arr)
	{
		int w = (int)dimensions.x;
		int d = (int)dimensions.z;
		arr = new bool[w, d];
		bool allFree = true;

		for (int x = 0; x < w; x++) {
			for (int z = 0; z < d; z++) {
				Vector3 rayPos = this.transform.position + new Vector3 (x, 0f, z);
				// check if there is a block underneath
				Ray ray = new Ray (rayPos, Vector3.down);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, 0.6f)) {
					if (hit.collider.tag == "Collectable") {
						arr [x, z] = true;
						continue;
					}
				}
				allFree = false;
			}
		}

		return allFree;
	}

	private void GenerateFloor ()
	{
		floor = new GameObject ("Floor");
		floor.transform.parent = this.transform;
		floor.transform.localPosition = new Vector3 (0, -0.5f, 0);

		GameObject bottom = GameObject.CreatePrimitive (PrimitiveType.Cube);
		bottom.AddComponent<Buildable> ();
		bottom.tag = "Buildable";
		bottom.transform.parent = floor.transform;
		float border = 0.05f;
		bottom.transform.localScale = new Vector3 (dimensions.x + 2 * border, 0.02f, dimensions.z + 2 * border);
		bottom.transform.localPosition = new Vector3 ((dimensions.x - 1) / 2.0f, 0.01f, (dimensions.z - 1) / 2.0f);
		bottom.GetComponent<MeshRenderer> ().material = floorMat;

		for (int x = 0; x < (int)dimensions.x; x++) {
			for (int z = 0; z < (int)dimensions.z; z++) {
				if (data [x, 0, z] == BlockType.EMPTY)
					continue;

				GameObject quad = GameObject.CreatePrimitive (PrimitiveType.Quad);
				quad.transform.parent = floor.transform;
				quad.transform.rotation = Quaternion.Euler (90, 0, 0);
				quad.transform.localPosition = new Vector3 (x, 0.022f, z);
				Destroy (quad.GetComponent<MeshCollider> ());
				switch (data [x, 0, z]) {
				case BlockType.DIRT:
					quad.GetComponent<MeshRenderer> ().material = dirt;
					break;
				case BlockType.STONE:
					quad.GetComponent<MeshRenderer> ().material = stone;
					break;
				case BlockType.WOOD:
					quad.GetComponent<MeshRenderer> ().material = wood;
					break;
				}
			}
		}
	}

	// place a quad indicating if the block underneath is blocked or free
	private void CreateQuad (int x, int z, bool free)
	{
		GameObject quad = GameObject.CreatePrimitive (PrimitiveType.Quad);
		Destroy (quad.GetComponent<MeshCollider> ());
		quad.transform.parent = this.transform;
		quad.transform.localPosition = new Vector3 (x, -0.49f, z);
		quad.transform.rotation = Quaternion.Euler (90f, 0, 0);
		quad.GetComponent<MeshRenderer> ().material = free ? freeMat : blockedMat;
	}
}
