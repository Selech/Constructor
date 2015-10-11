using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

public class BlueprintFactoryScript : MonoBehaviour {

	public GameObject blueprint;
    public string fileName;
	private string[] blockTypes = new string[] {"Dirt", "Stone", "Wood"};

	// Use this for initialization
	void Start () {
		//NewTemplate();
		//SaveBlueprint();
		LoadBlueprint();
    }

	private void NewTemplate() {
		foreach(string bt in blockTypes) {
			GameObject container = new GameObject(bt + " Blocks");
			container.transform.parent = blueprint.transform;
		}
	}

    private void SaveBlueprint()
    {
        var sr = File.CreateText(fileName + ".txt");
		sr.WriteLine("Position:" + blueprint.transform.position);
		// saves all positions of the blocks
		foreach(string bp in blockTypes) {
			sr.WriteLine(GetBlockPositions(bp, GameObject.Find(bp + " Blocks")));
		}
        sr.Close();
        print("Saved blueprint succesfully!");
    }

    private void LoadBlueprint()
    {
		DestroyChildren (blueprint);
		NewTemplate();
		var prefabs = LoadPrefabs ();
        var sr = File.OpenText(fileName + ".txt");

        // read data file
		string line = sr.ReadLine();
        while (line != null)
        {
            string[] data = line.Split(':');
            if (data[0] == "Position")
            {
                blueprint.transform.position = ToVector(data[1]);
            }
            else
            {
				string blockType = data[0];
				GameObject container = GameObject.Find((blockType + " Blocks"));
				GameObject prefab = prefabs[blockType];

                // creates the blocks
                foreach(string v in data[1].Split(';'))
                {
                    GameObject block = Instantiate(prefab);
                    block.transform.position = ToVector(v);
                    block.transform.SetParent(container.transform);
                }
            }
            line = sr.ReadLine();
        }
        sr.Close();
        print("Loaded blueprint succesfully!");
    }

    private Vector3 ToVector(string v)
    {
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
		foreach(string bt in blockTypes) {
			prefabs[bt] = (GameObject)Resources.Load("Blocks/" + bt, typeof(GameObject));
		}
		return prefabs;
	}

    private string GetBlockPositions(string blockType, GameObject container)
    {
		Transform[] arr = container.GetComponentsInChildren<Transform>();
		string result = blockType + ":";

        foreach (Transform t in arr)
        {
			if (t.gameObject.tag == "Block")
            	result += t.position + ";";
        }
		return result.TrimEnd(';');
    }
}
