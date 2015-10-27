using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlockScript : MonoBehaviour {

	private static Dictionary<string, GameObject> prefabs;

	public float HP;
	public ParticleSystem ps;
	public BlockType type;

	void Awake(){
		prefabs = LoadPrefabs();
	}

	// Creates a block 
	public static GameObject Create(BlockType type, Vector3 localPos, GameObject parent) {
		if(prefabs == null){
			prefabs = LoadPrefabs();
		}

		GameObject block = Instantiate(prefabs[type+""]);
		block.transform.parent = parent.transform;
		block.transform.localPosition = localPos;
		return block;
	}

	public static GameObject CreateFaded(BlockType type, Vector3 localPos, GameObject parent) {
		if(prefabs == null){
			prefabs = LoadPrefabs();
		}

		GameObject block = Instantiate(prefabs[type+"-Trans"]);
		block.transform.parent = parent.transform;
		block.transform.localPosition = localPos;
		return block;
	}

	private static Dictionary<string, GameObject> LoadPrefabs() {
		Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
		foreach(BlockType bt in Enum.GetValues(typeof(BlockType))) {
			prefabs[bt+""] = (GameObject)Resources.Load("Blocks/" + bt, typeof(GameObject));
			prefabs[bt+"-Trans"] = (GameObject)Resources.Load("Blocks/BlueprintBlocks/" + bt + "-Trans", typeof(GameObject));
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
