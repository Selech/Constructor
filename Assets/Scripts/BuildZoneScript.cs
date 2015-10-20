using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent (typeof (BoxCollider))]
public class BuildZoneScript : MonoBehaviour {

	private BoxCollider box;
	// current blueprint of the build zone
	private BlueprintScript blueprint;
	// Lists containing all blocks in the build zone
	private Dictionary<BlockType, ArrayList> blockLists;
	// List of all excessive blocks in the build zone
	private ArrayList excessiveBlocks;
	private string[] blueprintFiles;

	// Use this for initialization
	void Awake () {
		blueprintFiles = new string[] {
			"OvenPrint",
			"SmallStoneHouse"
		};
		box = GetComponent<BoxCollider>();
		blueprint = this.transform.Find ("Blueprint").gameObject.GetComponent<BlueprintScript>();
		blockLists = new Dictionary<BlockType, ArrayList> ();
		foreach(BlockType bt in Enum.GetValues(typeof(BlockType))) {
			blockLists[bt] = new ArrayList();
		}
		excessiveBlocks = new ArrayList();
		DataLoader.LoadBlueprint (blueprintFiles[0], blueprint);
	}

	public bool Contains(Vector3 position) {
		return box.bounds.Contains(position);
	}

	public void BlockAdded(GameObject block){
		BlockType type = block.GetComponent<BlockScript> ().type;

		if (blueprint.Contains (type, block.transform.position)) {
			blockLists[type].Add (block);
			if(IsDone()){
				BlueprintFinished();
			}
		} else {
			excessiveBlocks.Add (block);
		}
	}

	public void BlockRemoved(GameObject block){
		BlockType type = block.GetComponent<BlockScript> ().type;
		
		if (blueprint.Contains (type, block.transform.position)) {
			blockLists[type].Remove(block);
		} else {
			excessiveBlocks.Remove(block);
			if(IsDone()){
				BlueprintFinished();
			}
		}
	}

	private void BlueprintFinished() {
		Clear ();
		DataLoader.LoadBlueprint (blueprintFiles [1], blueprint);
	}

	private void Clear() {
		blueprint.Clear ();
		foreach(BlockType bt in Enum.GetValues(typeof(BlockType))) {
			foreach(GameObject block in blockLists[bt]) {
				Destroy(block);
			}
			blockLists[bt] = new ArrayList();
		}
		excessiveBlocks = new ArrayList();
	}

	private bool IsDone(){
		// Check if excessive blocks exists
		if(excessiveBlocks.Count > 0){
			return false;
		}
		// Check if all blocks are placed
		foreach(BlockType bt in Enum.GetValues(typeof(BlockType))) {
			if(blockLists[bt].Count != blueprint.GetBlockCount(bt)){
				return false;
			}
		}
		return true;
	}
}
