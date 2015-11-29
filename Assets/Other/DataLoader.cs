using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class DataLoader : MonoBehaviour {

	public static void LoadBlueprint(string fileName, BlueprintScript blueprint) {
		//var sr = File.OpenText("Assets/Resources/Blueprints/" + fileName + ".txt");

		TextAsset file = (TextAsset)Resources.Load("Blueprints/" + fileName, typeof(TextAsset));
		string text = file.text;
		
		System.IO.StringReader reader = new System.IO.StringReader(text);
		
		// read data file
		string line = reader.ReadLine();
		while (line != null)
		{
			string[] data = line.Split(':');
			if (data[0] == "Position")
			{
				blueprint.gameObject.transform.localPosition = ToVector(data[1]);
				line = reader.ReadLine();
				continue;
			}

			// if data exists
			if(data[1] != ""){
				BlockType blockType = (BlockType)Enum.Parse(typeof(BlockType), data[0]);

				// creates the blocks
				foreach(string v in data[1].Split(';'))
				{
					if (v != "")
						blueprint.AddBlock(blockType, ToVector(v));
				}
			}
			line = reader.ReadLine();
		}
		reader.Close();
		print("Loaded blueprint succesfully!");
	}

	public static void SaveBlueprint(string fileName, BlueprintScript blueprint) {
		var sr = File.CreateText("Assets/Resources/Blueprints/" + fileName + ".txt");
		// Write local position of blueprint
		sr.WriteLine("Position:" + blueprint.gameObject.transform.localPosition);
		// saves all positions of the blocks
		foreach(BlockType bt in Enum.GetValues(typeof(BlockType))) {
			string line = bt + ":";
			GameObject container = blueprint.transform.Find(bt + " Blocks").gameObject;
			foreach (Vector3 v in GetBlockPositions(container))
			{
				line += v + ";";
			}
			line.TrimEnd(';');
			sr.WriteLine(line);
		}
		sr.Close();
		print("Saved blueprint succesfully!");
	}

	private static ArrayList GetBlockPositions(GameObject container) {
		ArrayList positions = new ArrayList();
		foreach (Transform child in container.transform) {
			positions.Add(child.transform.localPosition);
		}
		return positions;
	}

	private static Vector3 ToVector(string v)
	{
		string[] v2 = v.Split(',');
		float x = float.Parse(v2[0].TrimStart('('));
		float y = float.Parse(v2[1].TrimStart(' '));
		float z = float.Parse(v2[2].TrimEnd(')').TrimStart(' '));
		return new Vector3(x, y, z);
	}
}
