using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public GameObject buildZone;
	public int layers;
	public GameObject stone;
	public GameObject dirt;
	public GameObject wood;
	private GameObject container;

	// Use this for initialization
	void Start () {
		container = GameObject.Find ("Blocks");
		GenerateMap (layers,50,50);
	}

	void GenerateMap(int height, int depth, int width){
		BuildZoneScript bs = buildZone.GetComponent<BuildZoneScript>();
		for(int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				for(int z = 0; z < depth; z++){
					Vector3 blockPos = new Vector3(x, y, z);
					// Ignore positions colliding with the build zone
					if (bs.Contains(blockPos)) {
						continue;
					}
					// Generates blocks
					PickBlock(blockPos);
				}
			}
		}
	}

	GameObject PickBlock(Vector3 position){

		if(position.y < 2){
			return BlockScript.Create(BlockType.Stone, position, container);
		}
		if(position.y < 4 && Random.Range (0,2) == 0){
			return BlockScript.Create(BlockType.Stone, position, container);
		}

		return BlockScript.Create(BlockType.Dirt, position, container);
	}
}
