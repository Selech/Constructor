using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent (typeof(BoxCollider))]
public class BlueprintScript : MonoBehaviour
{
	public GameObject quadPrefab;
	public Material freeMat;
	public Material blockedMat;

	// Data of all supposed block positions in the blueprint
	private byte[,,] data;
	// Lists of actual block positions
	private byte[,,] blocks;
	// local position at which the blueprint will be placed
	private BoxCollider box;
	private Vector3 anchorPoint;
	private Vector3 dimensions;
	private bool IsPlacing;
	private Vector3 oldPos;

	void Awake ()
	{
		box = this.GetComponent<BoxCollider> ();
		oldPos = this.transform.position;
	}

	// should delete this
	void Start ()
	{
		int x = 3;
		int y = 1;
		int z = 2;
		SetDimensions (new Vector3 (x, y, z), new Vector2 (1, 0));
		data = new byte[x, y, z];
		data [0, 0, 0] = 1;
		data [1, 0, 0] = 1;
		data [2, 0, 0] = 1;
		data [0, 0, 1] = 1;
		data [1, 0, 1] = 1;
		data [2, 0, 1] = 1;
	}

	public void Update ()
	{
		// if the grid has changed position
		if (PositionChanged ()) {
			oldPos = this.transform.position;
			UpdatePlacementGrid ();
		}
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

	public void SetData (byte[,,] data)
	{
		this.data = data;
	}

	public void AddBlock (byte BlockType, Vector3 localPos)
	{

	}

	public void RemoveBlock (byte BlockType, Vector3 localPos)
	{
		
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
		return CalculatePlacementGrid (out arr);
	}

	private bool CalculatePlacementGrid (out bool[,] arr)
	{
		int w = (int)dimensions.x;
		int d = (int)dimensions.z;
		arr = new bool[w, d];
		bool allFree = true;

		// 63 22 64
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

	// place a quad indicating if the block underneath is blocked or free
	private void CreateQuad (int x, int z, bool free)
	{
		GameObject quad = (GameObject)Instantiate (quadPrefab);
		quad.transform.parent = this.transform;
		quad.transform.localPosition = new Vector3 (x, -0.49f, z);
		quad.GetComponent<MeshRenderer> ().material = free ? freeMat : blockedMat;
	}
}
