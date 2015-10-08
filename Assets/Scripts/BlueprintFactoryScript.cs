using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

public class BlueprintFactoryScript : MonoBehaviour {

	public GameObject blueprint;
    public Vector3 dimensions;
    public string fileName;

	// Use this for initialization
	void Start () {
        LoadPrefabs();
        //Save();
        Load();
    }

	private void NewTemplate() {

	}

    private void SaveBlueprint()
    {
        var sr = File.CreateText(fileName + ".txt");
        sr.WriteLine("dim:" + dimensions);
        sr.WriteLine(GetBlockPositions("dirt:", GameObject.Find ("Dirt blocks")));
		sr.WriteLine(GetBlockPositions("stone:", GameObject.Find ("Stone blocks"));
		sr.WriteLine(GetBlockPositions("stone:", GameObject.Find ("Wood blocks"));
        sr.Close();
        print("Saved blueprint succesfully!");
    }

    private void LoadBlueprint()
    {
		DestroyChildren (blueprint);
		var prefabs = LoadPrefabs ();
        var sr = File.OpenText(fileName + ".txt");
        // read lines
		string line = sr.ReadLine();
        while (line != null)
        {
            string[] data = line.Split(':');
            if (data[0] == "dim")
            {
                Vector3 dim = ToVector(data[1]);
            }
            else
            {
                // find prefab and parent
                GameObject prefab;
                GameObject parent;
                switch(data[0])
                {
                    case "dirt": prefab = dirtPrefab; parent = dirtBlocks; break;
                    case "stone": prefab = stonePrefab; parent = stoneBlocks; break;
                    case "wood": prefab = woodPrefab; parent = woodBlocks; break;
                    default: prefab = dirtPrefab; parent = dirtBlocks; break;
                }
                // creates the blocks
                foreach(string v in data[1].Split(';'))
                {
                    block = Instantiate(prefab);
                    block.transform.position = ToVector(v);
                    block.transform.SetParent(parent.transform);
                }
            }
            line = sr.ReadLine();
        }
        sr.Close();
        print("Loaded blueprint succesfully!");
    }

    private Vector3 ToVector(string v)
    {
		print ("<" + v + ">");
        string[] v2 = v.Split(',');
        float x = float.Parse(v2[0].TrimStart('('));
        float y = float.Parse(v2[1].TrimStart(' '));
        float z = float.Parse(v2[2].TrimEnd(')').TrimStart(' '));
        return new Vector3(x, y, z);
    }

    private void DestroyChildren(GameObject obj)
    {
		foreach (Transform child in obj.transform) {
			GameObject.Destroy(child);
		}
//        foreach (ParticleSystem child in obj.GetComponentsInChildren<ParticleSystem>())
//        {
//            print(child.name);
//            Destroy(child.transform.parent.gameObject);
//        }
    }

	private Dictionary<string, GameObject> LoadPrefabs()
	{
		Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
		prefabs["dirt"] = (GameObject)Resources.Load("Blocks/dirt", typeof(GameObject));
		prefabs["stone"] = (GameObject)Resources.Load("Blocks/stone", typeof(GameObject));
		prefabs["wood"] = (GameObject)Resources.Load("Blocks/wood", typeof(GameObject));
		return prefabs;
	}

    private string GetBlockPositions(string type, GameObject parent)
    {
        Transform[] arr = parent.GetComponentsInChildren<Transform>();
        string result = type;

        foreach (Transform t in arr)
        {
			if (t.gameObject.tag == "Block")
            	result += t.position + ";";
        }
		return result.TrimEnd(';');
    }
}
