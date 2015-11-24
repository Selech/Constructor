using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public GameObject buildZone;
	public int layers;
	private GameObject container;

	private byte[,,] map;

	public Material dirt;
	public Material stone;

	public int quads = 0;

	// Use this for initialization
	void Start () {
		map = new byte[75,layers+30,75];
		container = GameObject.Find ("Blocks");
		GenerateMap (layers,75,75);
	}

	void GenerateMap(int height, int depth, int width){
		BuildZoneScript bs = buildZone.GetComponent<BuildZoneScript>();
		for(int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				for(int z = 0; z < depth; z++){
					Vector3 blockPos = new Vector3(x, y, z);
					// Ignore positions colliding with the build zone
					if (bs.Contains(blockPos)) {
						FindSides(x,y,z);
						continue;
					}
					// Generates blocks
					map[x, y, z] = PickBlock(blockPos);
					FindSides(x,y,z);
				}
			}
		}

		for (int x = 0; x < width; x++) {
			for (int z = 0; z < depth; z++) {
				FindSides(x,height,z);
			}
		}
	}

	byte PickBlock(Vector3 position){
		if(position.y < 2){
			return 2;
		}
		if(position.y < 3 && Random.Range (0,2) == 0){
			return 2;
		}

		return 1;
	}

	void FindSides(int x, int y, int z){
		if(x != 0){
			if(map[x, y, z] == 0){
				if (map [x - 1,  y,  z] != 0) {
					//Laver right quad
					GameObject left = GameObject.CreatePrimitive(PrimitiveType.Quad);
					left.transform.position = new Vector3(x-1,y,z) + new Vector3(0.5f,0,0);
					left.transform.rotation = Quaternion.Euler(0f,-90f,0f);
					left.name = "right";
					SetType(map[x-1,y,z], left);
				}
			}

			else if (map [x - 1,  y,  z] == 0) {
				//Laver left quad
				GameObject left = GameObject.CreatePrimitive(PrimitiveType.Quad);
				left.transform.position = new Vector3(x,y,z) + new Vector3(-0.5f,0,0);
				left.transform.rotation = Quaternion.Euler(0f,90f,0f);
				left.name = "left";
				SetType(map[x,y,z], left);
			}
		}

		if( y != 0){
			if(map[x, y, z] == 0){
				if (map [x,  y - 1,  z] != 0) {
					//Laver top quad
					GameObject left = GameObject.CreatePrimitive(PrimitiveType.Quad);
					left.transform.position = new Vector3(x,y-1,z) + new Vector3(0,0.5f,0);
					left.transform.rotation = Quaternion.Euler(90f,0f,0f);
					left.name = "top";
					SetType(map[x,y-1,z], left);
				}
			}
			else  if (map [ x,  y - 1,  z] == 0) {
				//Laver bottom quad
				GameObject left = GameObject.CreatePrimitive(PrimitiveType.Quad);
				left.transform.position = new Vector3(x,y,z) + new Vector3(0,-0.5f,0);
				left.transform.rotation = Quaternion.Euler(-90f,0,0f);
				left.name = "bottom";
				SetType(map[x,y,z], left);
			}
		}

		if( z != 0){
			if(map[x, y, z] == 0){
				if (map [x,  y,  z - 1] != 0) {
					//Laver back quad
					GameObject left = GameObject.CreatePrimitive(PrimitiveType.Quad);
					left.transform.position = new Vector3(x,y,z-1) + new Vector3(0f,0,0.5f);
					left.transform.rotation = Quaternion.Euler(0f,-180f,0f);
					left.name = "back";
					SetType(map[x,y,z-1], left);
				}
			}

			else if (map [ x,  y,  z - 1] == 0) {
				//Laver front quad
				GameObject left = GameObject.CreatePrimitive(PrimitiveType.Quad);
				left.transform.position = new Vector3(x,y,z) + new Vector3(0,0f,-0.5f);
				left.transform.rotation = Quaternion.Euler(0f,0,0f);
				left.name = "front";
				SetType(map[x,y,z], left);
			}
		}

	}

	private void SetType(int i, GameObject obj){
		quads++;

		BlockScript bs = obj.AddComponent<BlockScript>();
		Destroy (obj.GetComponent<MeshCollider> ());
		obj.AddComponent<BoxCollider> ();
		obj.tag = "Collectable";

		if(i == 1){
			bs.type = BlockType.Dirt;
			obj.GetComponent<MeshRenderer>().material = dirt;
		}
		else{
			bs.type = BlockType.Stone;
			obj.GetComponent<MeshRenderer>().material = stone;
		}
	}

	public void UpdateQuad(GameObject quad){
		Vector3 pos = quad.transform.position;

		switch (quad.name) {
		case "top": 
			pos += new Vector3(0,-0.5f,0); break;

		case "bottom": 
			pos += new Vector3(0,0.5f,0); break;

		case "left": 
			pos += new Vector3(0.5f,0,0); break;

		case "right": 
			pos += new Vector3(-0.5f,0,0); break;

		case "back": 
			pos += new Vector3(0,0f,-0.5f); break;

		case "front": 
			pos += new Vector3(0,0f,0.5f); break;

		default:
			break;
		}

		DestroyQuads (pos);
		map [(int)pos.x, (int)pos.y, (int)pos.z] = 0;

		if(map [(int)pos.x, (int)pos.y+1, (int)pos.z] != 0){
			GameObject left = GameObject.CreatePrimitive(PrimitiveType.Quad);
			left.transform.position = new Vector3((int)pos.x, (int)pos.y+1, (int)pos.z) + new Vector3(0,-0.5f,0);
			left.transform.rotation = Quaternion.Euler(-90f,0,0f);
			left.name = "bottom";
			SetType(map[(int)pos.x, (int)pos.y+1, (int)pos.z], left);
		}

		if(map [(int)pos.x, (int)pos.y-1, (int)pos.z] != 0){
			GameObject left = GameObject.CreatePrimitive(PrimitiveType.Quad);
			left.transform.position = new Vector3((int)pos.x, (int)pos.y-1, (int)pos.z) + new Vector3(0,0.5f,0);
			left.transform.rotation = Quaternion.Euler(90f,0f,0f);
			left.name = "top";
			SetType(map[(int)pos.x, (int)pos.y-1, (int)pos.z], left);
		}

		if(map [(int)pos.x-1, (int)pos.y, (int)pos.z] != 0){
			GameObject left = GameObject.CreatePrimitive(PrimitiveType.Quad);
			left.transform.position = new Vector3((int)pos.x-1, (int)pos.y, (int)pos.z) + new Vector3(0.5f,0,0);
			left.transform.rotation = Quaternion.Euler(0f,-90f,0f);
			left.name = "right";
			SetType(map[(int)pos.x-1, (int)pos.y, (int)pos.z], left);
		}

		if(map [(int)pos.x+1, (int)pos.y, (int)pos.z] != 0){
			GameObject left = GameObject.CreatePrimitive(PrimitiveType.Quad);
			left.transform.position = new Vector3((int)pos.x+1, (int)pos.y, (int)pos.z) + new Vector3(-0.5f,0,0);
			left.transform.rotation = Quaternion.Euler(0f,90f,0f);
			left.name = "left";
			SetType(map[(int)pos.x+1, (int)pos.y, (int)pos.z], left);
		}

		if(map [(int)pos.x, (int)pos.y, (int)pos.z-1] != 0){
			GameObject left = GameObject.CreatePrimitive(PrimitiveType.Quad);
			left.transform.position = new Vector3((int)pos.x, (int)pos.y, (int)pos.z-1) + new Vector3(0f,0,0.5f);
			left.transform.rotation = Quaternion.Euler(0f,-180f,0f);
			left.name = "back";
			SetType(map[(int)pos.x, (int)pos.y, (int)pos.z-1], left);
		}

		if(map [(int)pos.x, (int)pos.y, (int)pos.z+1] != 0){
			GameObject left = GameObject.CreatePrimitive(PrimitiveType.Quad);
			left.transform.position = new Vector3((int)pos.x, (int)pos.y, (int)pos.z) + new Vector3(0f,0,0.5f);
			left.transform.rotation = Quaternion.Euler(0f,0f,0f);
			left.name = "back";
			SetType(map[(int)pos.x, (int)pos.y, (int)pos.z+1], left);
		}
	}

	private void DestroyQuads(Vector3 pos){
		Ray ray = new Ray (pos, Vector3.up);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.85f)) {
			print ("hit up");
			if(hit.collider.tag == "Collectable"){
				quads--;
				Destroy(hit.collider.gameObject);
			}
		}

		ray.direction = Vector3.right;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.85f)) {
			if(hit.collider.tag == "Collectable"){
				quads--;
				Destroy(hit.collider.gameObject);
			}
		}

		ray.direction = Vector3.down;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.85f)) {
			if(hit.collider.tag == "Collectable"){
				quads--;
				Destroy(hit.collider.gameObject);
			}
		}

		ray.direction = Vector3.left;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.85f)) {
			if(hit.collider.tag == "Collectable"){
				quads--;
				Destroy(hit.collider.gameObject);
			}
		}

		ray.direction = Vector3.forward;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.85f)) {
			if(hit.collider.tag == "Collectable"){
				quads--;
				Destroy(hit.collider.gameObject);
			}
		}

		ray.direction = Vector3.back;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.85f)) {
			if(hit.collider.tag == "Collectable"){
				quads--;
				Destroy(hit.collider.gameObject);
			}
		}
	}
}
