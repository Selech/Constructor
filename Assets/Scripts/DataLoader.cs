using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class DataLoader : MonoBehaviour {

	public static void LoadBlueprint(string fileName, BlueprintScript blueprint) {
		var sr = File.OpenText("Assets/Resources/Blueprints/" + fileName + ".txt");
		
		// read data file
		string line = sr.ReadLine();
		while (line != null)
		{
			string[] data = line.Split(':');
			if (data[0] == "Position")
			{
				blueprint.gameObject.transform.localPosition = ToVector(data[1]);
				line = sr.ReadLine();
				continue;
			}

			// if data exists
			if(data[1] != ""){
				BlockType blockType = (BlockType)Enum.Parse(typeof(BlockType), data[0]);

				// creates the blocks
				foreach(string v in data[1].Split(';'))
				{
					blueprint.AddBlock(blockType, ToVector(v));
				}
			}
			line = sr.ReadLine();
		}
		sr.Close();
		print("Loaded blueprint succesfully!");
	}

	public static void SaveBlueprint(string fileName, BlueprintScript blueprint) {
		var sr = File.CreateText("Assets/Resources/Blueprints/" + fileName + ".txt");
		// Write local position of blueprint
		sr.WriteLine("Position:" + blueprint.gameObject.transform.localPosition);
		// saves all positions of the blocks
		foreach(BlockType bp in Enum.GetValues(typeof(BlockType))) {
			string line = bp + ":";
			foreach (Vector3 v in blueprint.GetBlockPositions(bp))
			{
				line += v + ";";
			}
			line.TrimEnd(';');
			sr.WriteLine(line);
		}
		sr.Close();
		print("Saved blueprint succesfully!");
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
