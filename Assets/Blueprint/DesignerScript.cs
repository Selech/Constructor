using UnityEngine;
using System.Collections;

public class DesignerScript : MonoBehaviour {

	public string fileName;
	public bool saveMode;


	// Use this for initialization
	void Start () {
		if (saveMode)
			Save ();
		else
			Load ();
	}

	private void Save() {
		int w = 0;
		int h = 0;
		int d = 0;
		// find the dimensions
		foreach (Transform child in this.transform) {
			Vector3 pos = child.transform.position;
			if (pos.x > w)
				w = (int)pos.x;
			if (pos.y > h)
				h = (int)pos.y;
			if (pos.z > d)
				d = (int)pos.z;
		}
		int ax = -(int)this.transform.position.x;
		int az = -(int)this.transform.position.z;

		// collect data
		byte[,,] data = new byte[w, h, d];
		foreach (Transform child in this.transform) {
			// determine block type
			byte type = BlockType.EMPTY;
			switch(child.gameObject.name) {
				case "Dirt":
				type = BlockType.DIRT;
				break;
			case "Stone":
				type = BlockType.STONE;
				break;
			case "Wood":
				type = BlockType.WOOD;
				break;
			}

			// find block positions
			foreach (Transform block in child) {
				data[(int)block.position.x, (int)block.position.y, (int)block.position.z] = type;
			}
		}

		DataLoader.SaveBlueprint (fileName, w, h, d, ax, az, data);
	}
	
	private void Load() {
		BlueprintScript bp = DataLoader.LoadBlueprint (fileName);

	}

}
