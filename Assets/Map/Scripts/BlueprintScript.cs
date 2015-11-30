using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlueprintScript : MonoBehaviour {
	// Lists local positions of all blocks in blueprint
//	private Dictionary<BlockType, ArrayList> blockLists;
//	// Containers for all the blocks in blueprint
//	private Dictionary<BlockType, GameObject> containers;
//
//	void Awake(){
//		containers = new Dictionary<BlockType, GameObject> ();
//		blockLists = new Dictionary<BlockType, ArrayList> ();
//		foreach(BlockType bt in Enum.GetValues(typeof(BlockType))) {
//			containers[bt] = this.transform.Find(bt + " Blocks").gameObject;
//			blockLists[bt] = new ArrayList();
//		}
//
//	}
//
//	public void AddBlock(BlockType type, Vector3 localPos){
//		BlockScript.CreateFaded (type, localPos, containers[type]);
//		blockLists [type].Add (localPos);
//	}
//
//	public bool Contains(BlockType type, Vector3 globalPos){
//		Vector3 localPos = globalPos - this.transform.position;
//		return blockLists [type].Contains (localPos);
//	}
//
//	public int GetBlockCount(BlockType type) {
//		return blockLists [type].Count;
//	}
//
//	public void Clear(){
//		foreach(BlockType bt in Enum.GetValues(typeof(BlockType))) {
//			blockLists[bt] = new ArrayList();
//			// destroy all blocks in container
//			foreach (Transform child in containers[bt].transform) {
//				GameObject.Destroy(child.gameObject);
//			}
//		}
//	}
}
