using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlockScript : MonoBehaviour {

	private static Dictionary<BlockType, GameObject> prefabs = LoadPrefabs();

	public float HP;
	public ParticleSystem ps;
	public BlockType type;

	// Creates a block 
	public static GameObject Create(BlockType type, Vector3 localPos, GameObject parent) {
		GameObject block = Instantiate(prefabs[type]);
		block.transform.parent = parent.transform;
		block.transform.localPosition = localPos;
		return block;
	}

	private static Dictionary<BlockType, GameObject> LoadPrefabs() {
		Dictionary<BlockType, GameObject> prefabs = new Dictionary<BlockType, GameObject>();
		foreach(BlockType bt in Enum.GetValues(typeof(BlockType))) {
			prefabs[bt] = (GameObject)Resources.Load("Blocks/" + bt, typeof(GameObject));
		}
		return prefabs;
	}

	// returns true if block breaks
	public bool Hit(Vector3 hitPos, float dmg){
		HP -= dmg;
		// hit particle animation
		ps.transform.position = hitPos;
		ps.Play ();

		if(HP <= 0){
			Destroy(this.gameObject);
			return true;
		}
		else {
			return false;
		}
	}
}

public enum BlockType {
	Dirt, Stone, Wood
}
