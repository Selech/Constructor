using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	private const byte EMPTY = 0;
	private const byte DIRT = 1;
	private const byte STONE = 2;
	private const byte WOOD = 3;

	public PhysicMaterial quadPhysics;

	public Vector3 dimensions;
	public GameObject buildZone;
	private GameObject container;
	public Material dirt;
	public Material stone;
	public Material wood;

	private byte[,,] mapData;
	
	// Use this for initialization
	void Start () {
		mapData = new byte[(int)dimensions.x, (int)dimensions.y, (int)dimensions.z];
		container = GameObject.Find ("Blocks");
		GenerateMap();
		CalculateQuads ();
	}

	void GenerateMap(){
		BuildZoneScript bs = buildZone.GetComponent<BuildZoneScript>();
		MapElevator elevator = GetComponent<MapElevator> ();//
		Random.seed = 100;

		for (int x = 0; x < dimensions.x; x++) {
			for(int z = 0; z < dimensions.z; z++){
				// get the elevation of the coordinate
				int e = elevator.GetElevation(x, z);

				for(int y = 0; y < e; y++) {
					Vector3 blockPos = new Vector3(x, y, z);
					// Ignore positions colliding with the build zone
					if (bs.Contains(blockPos)) {
						continue;
					}
					// Generates blocks
					mapData[x, y, z] = PickBlock(blockPos);
				}
			}
		}
	}

	byte PickBlock(Vector3 position){
		// bottom 4 layers are all stone
		if(position.y < 4){
			return STONE;
		}
		// 80 % chance of stone, 20 % dirt if layer is [5-10]
		if(position.y < 10){
			return Random.value < 0.1f ? DIRT : STONE;
		}
		// 50 % chance of stone, 50 % dirt if layer is [11-20]
		if(position.y < 13){
			return Random.value < 0.4f ? DIRT : STONE;
		}

		if(position.y < 15){
			return Random.value < 0.7f ? DIRT : STONE;
		}

		// 5 % chance of stone, 95 % dirt if layer is 15+
		return Random.value < 0.95f ? DIRT : STONE;
	}

	void CalculateQuads() {
		// calculates surfaces of all blocks
		for (int x = 0; x < dimensions.x; x++) {
			for(int z = 0; z < dimensions.z; z++) {
				for(int y = 0; y < dimensions.y; y++){
					if(mapData[x,y,z] == EMPTY){
						break;
					}
					CalculateSurface(x, y, z);
				}
			}
		}
	}

	private void CreateQuad(Vector3 pos, string face) {
		GameObject quad = GameObject.CreatePrimitive (PrimitiveType.Quad);

		switch (face) {
		case "top":
			quad.transform.position = pos + new Vector3 (0f, 0.5f, 0f);
			quad.transform.rotation = Quaternion.Euler (90f, 0f, 0f);
			break;
		case "bottom":
			quad.transform.position = pos + new Vector3 (0, -0.5f, 0);
			quad.transform.rotation = Quaternion.Euler (-90f, 0, 0f);
			break;
		case "left":
			quad.transform.position = pos + new Vector3 (-0.5f, 0f, 0f);
			quad.transform.rotation = Quaternion.Euler (0f, 90f, 0f);
			break;
		case "right":
			quad.transform.position = pos + new Vector3 (0.5f, 0f, 0f);
			quad.transform.rotation = Quaternion.Euler (0f, -90f, 0f);
			break;
		case "back":
			quad.transform.position = pos + new Vector3 (0f, 0f, 0.5f);
			quad.transform.rotation = Quaternion.Euler (0f, -180f, 0f);
			break;
		case "front":
			quad.transform.position = pos + new Vector3 (0f, 0f, -0.5f);
			quad.transform.rotation = Quaternion.Euler (0f, 0f, 0f);
			break;
		default: 
			Destroy (quad);
			return;
		}

		// initialize quad
		quad.name = face;
		BlockScript bs = quad.AddComponent<BlockScript> ();
		Destroy (quad.GetComponent<MeshCollider> ());
		BoxCollider bc = quad.AddComponent<BoxCollider> ();
		bc.material = quadPhysics;
		quad.tag = "Collectable";
		quad.transform.parent = container.transform;

		// set texture of the block
		switch (mapData [(int)pos.x, (int)pos.y, (int)pos.z]) {
		case 1:
			bs.type = 1;
			quad.GetComponent<MeshRenderer> ().material = dirt;
			break;
		case 2:
			bs.type = 2;
			quad.GetComponent<MeshRenderer> ().material = stone;
			break;
		case 3:
			bs.type = 3;
			quad.GetComponent<MeshRenderer> ().material = wood;
			break;
		default:
			Destroy (quad);
			break;
		}
	}

	void CalculateSurface(int x, int y, int z){
		if(y < 0 || y >= dimensions.y || x < 0 || x >= dimensions.x || z < 0 || z >= dimensions.z){
			return;
		}

		if (mapData [x, y, z] == EMPTY) {
			return;
		}

		Vector3 pos = new Vector3 (x, y, z);
		DestroySurface (pos);

		// check top
		if (y < dimensions.y - 1 && mapData [x, y + 1, z] == EMPTY) {
			CreateQuad(pos, "top");
		}
		// check bottom
		if (y > 0 && mapData [x, y - 1, z] == EMPTY) {
			CreateQuad(pos, "bottom");
		}
		// check left
		if (x > 0 && mapData [x - 1, y, z] == EMPTY) {
			CreateQuad(pos, "left");
		}
		// check right
		if (x < dimensions.x - 1 && mapData [x + 1, y, z] == EMPTY) {
			CreateQuad(pos, "right");
		}
		// check front
		if (z < dimensions.z - 1 && mapData [x, y, z + 1] == EMPTY) {
			CreateQuad(pos, "back");
		}
		// check back
		if (z > 0 && mapData [x, y, z - 1] == EMPTY) {
			CreateQuad(pos, "front");
		}
	}

	public void PlaceBlock(Vector3 pos, byte type) {
		int x = (int)pos.x;
		int y = (int)pos.y;
		int z = (int)pos.z;

		mapData[x, y, z] = type;
		CalculateSurface (x, y, z);
		CalculateSurface (x-1, y, z);
		CalculateSurface (x+1, y, z);
		CalculateSurface (x, y-1, z);
		CalculateSurface (x, y+1, z);
		CalculateSurface (x, y, z-1);
		CalculateSurface (x, y, z+1);

	}

	public void DestroyBlock(GameObject quad){
		// find position of the block of this quad
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
		// destroy the block
		DestroySurface(pos);
		int x = (int)pos.x;
		int y = (int)pos.y;
		int z = (int)pos.z;
		mapData[(int)pos.x, (int)pos.y, (int)pos.z] = EMPTY;

		// calculate surface of the neightbors
		// top
		CalculateSurface (x, y + 1, z);
		// bottom
		CalculateSurface (x, y - 1, z);
		// left
		CalculateSurface (x - 1, y, z);
		// right
		CalculateSurface (x + 1, y, z);
		// front
		CalculateSurface (x, y, z + 1);
		// back
		CalculateSurface (x, y, z - 1);
	}

	private void DestroySurface(Vector3 pos){
		Ray ray = new Ray (pos, Vector3.up);
		RaycastHit hit;
		// top
		if (Physics.Raycast (ray, out hit, maxDistance: 0.5f)) {
			if(hit.collider.tag == "Collectable"){
				Destroy(hit.collider.gameObject);
			}
		}
		// right
		ray.direction = Vector3.right;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.5f)) {
			if(hit.collider.tag == "Collectable"){
				Destroy(hit.collider.gameObject);
			}
		}
		// bottom
		ray.direction = Vector3.down;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.5f)) {
			if(hit.collider.tag == "Collectable"){
				Destroy(hit.collider.gameObject);
			}
		}
		// left
		ray.direction = Vector3.left;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.5f)) {
			if(hit.collider.tag == "Collectable"){
				Destroy(hit.collider.gameObject);
			}
		}
		// front
		ray.direction = Vector3.forward;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.5f)) {
			if(hit.collider.tag == "Collectable"){
				Destroy(hit.collider.gameObject);
			}
		}
		// back
		ray.direction = Vector3.back;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.5f)) {
			if(hit.collider.tag == "Collectable"){
				Destroy(hit.collider.gameObject);
			}
		}
	}
}
