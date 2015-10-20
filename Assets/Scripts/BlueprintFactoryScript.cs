using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System;

public class BlueprintFactoryScript : MonoBehaviour {

	public GameObject blueprint;
	private BlueprintScript bpScript;
	private GameObject instansBlueprint;
    public string fileName;

	// Use this for initialization
	void Start () {
		//NewTemplate();
		//SaveBlueprint();
		LoadBlueprint();
    }

	private void NewTemplate() {
		instansBlueprint = Instantiate (blueprint);
		instansBlueprint.transform.parent = this.transform;
		instansBlueprint.transform.localPosition = new Vector3 ();

		bpScript = instansBlueprint.GetComponent<BlueprintScript> ();

		this.GetComponent<BuildZoneScript> ().blueprint = bpScript;
	}

    private void SaveBlueprint()
    {
        var sr = File.CreateText(fileName + ".txt");
		sr.WriteLine("Position:" + blueprint.transform.position);
		// saves all positions of the blocks
		foreach(BlockType bp in Enum.GetValues(typeof(BlockType))) {
			sr.WriteLine(GetBlockPositions(bp.ToString(), GameObject.Find(bp + " Blocks")));
		}
        sr.Close();
        print("Saved blueprint succesfully!");
    }

    public void LoadBlueprint()
    {
		NewTemplate();
        var sr = File.OpenText("Assets/Resources/Blueprints/" + fileName + ".txt");

        // read data file
		string line = sr.ReadLine();
        while (line != null)
        {
            string[] data = line.Split(':');
            if (data[0] == "Position")
            {
                blueprint.transform.localPosition = ToVector(data[1]);
            }
            else
            {
				if(data[1] == ""){
					line = sr.ReadLine();
					continue;	
				}

				BlockType blockType = (BlockType)Enum.Parse(typeof(BlockType), data[0]);

				GameObject container = null;
				ContainerScript[] css = instansBlueprint.GetComponentsInChildren<ContainerScript>();
				foreach(ContainerScript cs in css){
					if(cs.gameObject.name == blockType + " Blocks"){
						container = cs.gameObject;
					}
				}

                // creates the blocks
                foreach(string v in data[1].Split(';'))
                {
					GameObject block = BlockScript.CreateFaded(blockType, ToVector(v), container);
					bpScript.AddPosition(blockType, block.transform.position);
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
    }
	
    private string GetBlockPositions(string blockType, GameObject container)
    {
		Transform[] arr = container.GetComponentsInChildren<Transform>();
		string result = blockType + ":";

        foreach (Transform t in arr)
        {
			if (t.gameObject.tag == "Collectable")
            	result += t.position + ";";
        }
		return result.TrimEnd(';');
    }
}
