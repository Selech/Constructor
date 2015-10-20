using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class BuildZoneScript : MonoBehaviour {

	private BoxCollider box;
	public BlueprintScript blueprint;

	private ArrayList woodBlocks = new ArrayList();
	private ArrayList dirtBlocks = new ArrayList();
	private ArrayList stoneBlocks = new ArrayList();
	private ArrayList SmallDumbPeople = new ArrayList();

	// Use this for initialization
	void Awake () {
		box = GetComponent<BoxCollider>();
	}

	public bool Contains(Vector3 position) {
		return box.bounds.Contains(position);
	}

	public void BlockAdded(GameObject b){
		BlockType type = b.GetComponent<BlockScript> ().type;

		if (blueprint.Contains (type, b.transform.position)) {
			print ("Indeholder");
			if (type == BlockType.Dirt) {
				dirtBlocks.Add (b);
			}
			if (type == BlockType.Wood) {
				woodBlocks.Add (b);
			}
			if (type == BlockType.Stone) {
				stoneBlocks.Add (b);
			}
		} else {
			print ("Nu er lort i den..!");
			SmallDumbPeople.Add(b);
		}

		if(IsDone()){
			foreach(GameObject o in woodBlocks){
				Destroy(o);
			}

			foreach(GameObject o in dirtBlocks){
				Destroy(o);
			}

			foreach(GameObject o in stoneBlocks){
				Destroy(o);
			}
		}
	}

	public void BlockRemoved(GameObject b){
		BlockType type = b.GetComponent<BlockScript> ().type;
		
		if (blueprint.Contains (type, b.transform.position)) {
			if (type == BlockType.Dirt) {
				dirtBlocks.Remove (b);
			}
			if (type == BlockType.Wood) {
				woodBlocks.Remove (b);
			}
			if (type == BlockType.Stone) {
				stoneBlocks.Remove (b);
			}
		} else {
			SmallDumbPeople.Remove(b);
		}

		if(IsDone()){
			foreach(GameObject o in woodBlocks){
				Destroy(o);
			}
			
			foreach(GameObject o in dirtBlocks){
				Destroy(o);
			}
			
			foreach(GameObject o in stoneBlocks){
				Destroy(o);
			}


		}
	}

	private bool IsDone(){
		if(SmallDumbPeople.Count > 0){
			return false;
		}

		if(blueprint.CheckSizes(woodBlocks,stoneBlocks,dirtBlocks)){
			return true;
		}
		return false;
	}
}
