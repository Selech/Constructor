using UnityEngine;
using System.IO;
using System.Globalization;

public class BlueprintFactoryScript : MonoBehaviour {

    public Vector3 dimensions;
    public GameObject dirtBlocks;
    public GameObject stoneBlocks;
    public GameObject woodBlocks;
    public string fileName;
    private GameObject dirtPrefab, stonePrefab, woodPrefab;

	// Use this for initialization
	void Start () {
        LoadPrefabs();
        //Save();
        Load();
    }

    private void LoadPrefabs()
    {
        dirtPrefab = (GameObject)Resources.Load("Blocks/dirt", typeof(GameObject));
        stonePrefab = (GameObject)Resources.Load("Blocks/stone", typeof(GameObject));
        woodPrefab = (GameObject)Resources.Load("Blocks/wood", typeof(GameObject));
    }

    private void Save()
    {
        var sr = File.CreateText(fileName + ".txt");
        sr.WriteLine("dim:" + dimensions);
        sr.WriteLine(GetBlockPositions("dirt:", dirtBlocks));
        sr.WriteLine(GetBlockPositions("stone:", stoneBlocks));
        sr.Close();
        print("Saved blueprint succesfully!");
    }

    public void Load()
    {
        Reset();
        GameObject block;
        var sr = File.OpenText(fileName + ".txt");
        var line = sr.ReadLine();
        while (line != null)
        {
            string[] data = line.Split(':');
            if (data[0] == "dim")
            {
                Vector3 dim = ToVector(line.Split(':')[1]);
                print(dim);
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

    private void Reset()
    {
        DestroyAll(dirtBlocks);
        DestroyAll(stoneBlocks);
        DestroyAll(woodBlocks);
    }

    private void DestroyAll(GameObject obj)
    {
        foreach (ParticleSystem child in obj.GetComponentsInChildren<ParticleSystem>())
        {
            print(child.name);
            Destroy(child.transform.parent.gameObject);
        }
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
