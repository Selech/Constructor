using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MapElevator))]
public class MapGenerator : MonoBehaviour
{
	// size of map
	public Vector3 dimensions;
	// seed for generation of the map
	public int seed;
	// materials for the blocks
	public Material dirt;
	public Material stone;
	public Material wood;
	public Material electric;
	public ParticleSystem electricParticles;
	public PhysicMaterial quadPhysics;
	public GameObject water;
	private MapElevator elevator;
	private GameObject container;
	private byte[,,] mapData;
	
	// Use this for initialization
	void Awake ()
	{
		mapData = new byte[(int)dimensions.x, (int)dimensions.y, (int)dimensions.z];
		container = new GameObject ("Blocks");
		elevator = GetComponent<MapElevator> ();
	}

	public Vector3 GetRandomSpawnPoint ()
	{
		int x = Random.Range (10, (int)dimensions.x - 10);
		int z = Random.Range (10, (int)dimensions.z - 10);
		int y = elevator.GetElevation (x, z) + 4;

		return new Vector3 (x, y, z);
	}

	// generates the map
	public void GenerateMap ()
	{
		// generate elevation
		elevator.GenerateElevation (seed + 10);
		Random.seed = seed;

		// generate ground
		for (int x = 0; x < dimensions.x; x++) {
			for (int z = 0; z < dimensions.z; z++) {
				// get the elevation of the coordinate
				int e = elevator.GetElevation (x, z);

				for (int y = 0; y < e; y++) {
					// Generates blocks
					mapData [x, y, z] = PickBlock (x, y, z);
				}
			}
		}

		// generate trees
		Random.seed = seed + 50;
		int numOfTrees = 35;
		for (int i = 0; i< numOfTrees; i++) {
			int x = (int) (Random.value * (dimensions.x - 1));
			int z = (int) (Random.value * (dimensions.z - 1));
			GenerateTree(x, z);
		}


		CalculateQuads ();

	}

	private void GenerateTree (int x, int z)
	{
		// find lowest elevation point for tree
		int ymin = elevator.GetElevation (x, z);
		ymin = Mathf.Min (ymin, elevator.GetElevation (x + 1, z));
		ymin = Mathf.Min (ymin, elevator.GetElevation (x, z + 1));
		ymin = Mathf.Min (ymin, elevator.GetElevation (x + 1, z + 1));
		int treeHeight = Random.Range (7, 10);

		for (int i = 0; i < treeHeight; i++) {
			mapData [x, ymin + i, z] = BlockType.WOOD;
			mapData [x + 1, ymin + i, z] = BlockType.WOOD;
			mapData [x, ymin + i, z + 1] = BlockType.WOOD;
			mapData [x + 1, ymin + i, z + 1] = BlockType.WOOD;
		}
	}

	// Generate a block based on the position
	byte PickBlock (int x, int y, int z)
	{
		// bottom 4 layers are all stone
		if (y < 4) {
			return BlockType.STONE;
		}
		// 80 % chance of stone, 20 % dirt if layer is [5-10]
		if (y < 10) {
			if (Random.value < 0.005f) {
				return BlockType.ELECTRIC;
			}
			return Random.value < 0.1f ? BlockType.DIRT : BlockType.STONE;
		}
		// 50 % chance of stone, 50 % dirt if layer is [11-20]
		if (y < 13) {
			return Random.value < 0.4f ? BlockType.DIRT : BlockType.STONE;
		}

		if (y < 15) {
			return Random.value < 0.7f ? BlockType.DIRT : BlockType.STONE;
		}

		// 5 % chance of stone, 95 % dirt if layer is 15+
		return Random.value < 0.95f ? BlockType.DIRT : BlockType.STONE;
	}

	// Calculates surfaces of all blocks
	void CalculateQuads ()
	{
		for (int x = 0; x < dimensions.x; x++) {
			for (int z = 0; z < dimensions.z; z++) {
				for (int y = 0; y < dimensions.y; y++) {
					if (mapData [x, y, z] == BlockType.EMPTY) {
						break;
					}
					CalculateSurface (x, y, z);
				}
			}
		}
	}
	
	private void CreateQuad (Vector3 pos, string face)
	{
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
		case BlockType.DIRT:
			bs.type = BlockType.DIRT;
			quad.GetComponent<MeshRenderer> ().material = dirt;
			break;
		case BlockType.STONE:
			bs.type = BlockType.STONE;
			quad.GetComponent<MeshRenderer> ().material = stone;
			break;
		case BlockType.WOOD:
			bs.type = BlockType.WOOD;
			quad.GetComponent<MeshRenderer> ().material = wood;
			break;

		case BlockType.ELECTRIC:
			bs.type = BlockType.ELECTRIC;
			quad.GetComponent<MeshRenderer> ().material = electric;
			GameObject ps = Instantiate (electricParticles.gameObject);
			ps.transform.position = quad.transform.position;
			ps.transform.SetParent (quad.transform);
			break;
		default:
			Destroy (quad);
			break;
		}
	}

	void CalculateSurface (int x, int y, int z)
	{
		if (y < 0 || y >= dimensions.y || x < 0 || x >= dimensions.x || z < 0 || z >= dimensions.z) {
			return;
		}

		if (mapData [x, y, z] == BlockType.EMPTY) {
			return;
		}

		Vector3 pos = new Vector3 (x, y, z);
		DestroySurface (pos);

		// check top
		if (y < dimensions.y - 1 && mapData [x, y + 1, z] == BlockType.EMPTY) {
			CreateQuad (pos, "top");
		}
		// check bottom
		if (y > 0 && mapData [x, y - 1, z] == BlockType.EMPTY) {
			CreateQuad (pos, "bottom");
		}
		// check left
		if (x > 0 && mapData [x - 1, y, z] == BlockType.EMPTY) {
			CreateQuad (pos, "left");
		}
		// check right
		if (x < dimensions.x - 1 && mapData [x + 1, y, z] == BlockType.EMPTY) {
			CreateQuad (pos, "right");
		}
		// check front
		if (z < dimensions.z - 1 && mapData [x, y, z + 1] == BlockType.EMPTY) {
			CreateQuad (pos, "back");
		}
		// check back
		if (z > 0 && mapData [x, y, z - 1] == BlockType.EMPTY) {
			CreateQuad (pos, "front");
		}
	}

	public void PlaceBlock (Vector3 pos, byte type)
	{
		SoundSystem.PlaySound ("Place");

		int x = (int)pos.x;
		int y = (int)pos.y;
		int z = (int)pos.z;

		mapData [x, y, z] = type;
		CalculateSurface (x, y, z);
		CalculateSurface (x - 1, y, z);
		CalculateSurface (x + 1, y, z);
		CalculateSurface (x, y - 1, z);
		CalculateSurface (x, y + 1, z);
		CalculateSurface (x, y, z - 1);
		CalculateSurface (x, y, z + 1);
	}

	public void DestroyBlock(Vector3 pos) {
		SoundSystem.PlaySound ("Chop");

		// destroy the block
		DestroySurface (pos);
		int x = (int)pos.x;
		int y = (int)pos.y;
		int z = (int)pos.z;
		mapData [(int)pos.x, (int)pos.y, (int)pos.z] = BlockType.EMPTY;
		
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

	public void DestroyBlock (GameObject quad)
	{
		// find position of the block of this quad
		Vector3 pos = quad.transform.position;

		switch (quad.name) {
		case "top": 
			pos += new Vector3 (0, -0.5f, 0);
			break;

		case "bottom": 
			pos += new Vector3 (0, 0.5f, 0);
			break;

		case "left": 
			pos += new Vector3 (0.5f, 0, 0);
			break;

		case "right": 
			pos += new Vector3 (-0.5f, 0, 0);
			break;

		case "back": 
			pos += new Vector3 (0, 0f, -0.5f);
			break;

		case "front": 
			pos += new Vector3 (0, 0f, 0.5f);
			break;

		default:
			break;
		}
		DestroyBlock (pos);
	}

	private void DestroySurface (Vector3 pos)
	{
		Ray ray = new Ray (pos, Vector3.up);
		RaycastHit hit;
		// top
		if (Physics.Raycast (ray, out hit, maxDistance: 0.5f)) {
			if (hit.collider.tag == "Collectable") {
				Destroy (hit.collider.gameObject);
			}
		}
		// right
		ray.direction = Vector3.right;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.5f)) {
			if (hit.collider.tag == "Collectable") {
				Destroy (hit.collider.gameObject);
			}
		}
		// bottom
		ray.direction = Vector3.down;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.5f)) {
			if (hit.collider.tag == "Collectable") {
				Destroy (hit.collider.gameObject);
			}
		}
		// left
		ray.direction = Vector3.left;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.5f)) {
			if (hit.collider.tag == "Collectable") {
				Destroy (hit.collider.gameObject);
			}
		}
		// front
		ray.direction = Vector3.forward;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.5f)) {
			if (hit.collider.tag == "Collectable") {
				Destroy (hit.collider.gameObject);
			}
		}
		// back
		ray.direction = Vector3.back;
		if (Physics.Raycast (ray, out hit, maxDistance: 0.5f)) {
			if (hit.collider.tag == "Collectable") {
				Destroy (hit.collider.gameObject);
			}
		}
	}
}
