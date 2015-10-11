using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public GameObject buildZone;
	public GameObject stone;
	public GameObject dirt;
	public GameObject wood;

	// Use this for initialization
	void Start () {
		GenerateMap (6,50,50);
	}

	void GenerateMap(int height, int depth, int width){
		BuildZoneScript bs = buildZone.GetComponent<BuildZoneScript>();
		for(int i = 0; i < height; i++){
			for(int j = 0; j < depth; j++){
				for(int k = 0; k < width; k++){
					if (bs.Contains(new Vector3(k,i,j) + transform.position)) {
						continue;
					}
					GameObject g = PickBlock(i);
					g.transform.parent = this.transform;
					g.transform.localPosition = new Vector3(k,i,j);
				}
			}
		}
	}

	GameObject PickBlock(int height){
		if(height < 2){
			return Instantiate(stone);
		}

		if(height < 4){
			if(Random.Range (0,2) == 0){
				return Instantiate(stone);
			}
			else{
				return Instantiate(dirt);
			}
		}

		return Instantiate(dirt);
	}
}
