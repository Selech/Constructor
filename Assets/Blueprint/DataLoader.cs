using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class DataLoader {

	public static BlueprintScript LoadBlueprint(string fileName) {
//		TextAsset file = (TextAsset)Resources.Load("Blueprints/" + fileName, typeof(TextAsset));
//		string text = file.text;
//		System.IO.StringReader reader = new System.IO.StringReader(text);
//
//		// create new blueprint
//		GameObject obj = new GameObject ();
//		BlueprintScript blueprint = obj.AddComponent<BlueprintScript> ();
//
//		// read data file
//		string line = reader.ReadLine();
//		// read dimensions
//		String[] data = line.Split (',');
//		int w = int.Parse(data);
//		Vector3 dim = new Vector3(int.Parse(data[1]));
//		Vector3 dimensions = ToVector (data[1]);
//		int ax = int.Parse (data[2].Trim());
//		int ay = int.Parse (data[3].Trim());
//		Vector2 anchor = new Vector2(ax, ay);
//		line = reader.ReadLine ();
//
//		byte[,,] bpData = new byte[(int)dimensions.x, (int)dimensions.y, (int)dimensions.z];
//
//		// read block data
//		while (line != null)
//		{
//			// if data exists
//			if(data[1] != ""){
//				byte blockType = byte.Parse(data[0]);
//
//				// loads the blocks
//				foreach(string v in data[1].Split(';'))
//				{
//					if (v != "") {
//						Vector3 pos = ToVector(v);
//						bpData[(int)pos.x, (int)pos.y, (int)pos.z] = blockType;
//					}
//				}
//				blueprint.SetBlockData(type, blockData);
//			}
//			line = reader.ReadLine();
//		}
//		reader.Close();
//		print("Loaded blueprint succesfully!");
		return null;
	}

	public static void SaveBlueprint(string fileName, int w, int h, int d, int ax, int az, byte[,,] data) {
		var sr = File.CreateText("Assets/Resources/Blueprints/" + fileName + ".txt");
		// Write the data of the blueprint
		sr.WriteLine(+ w + "," + h + "," + d + "," + ax + "," + az);
		// saves all positions of the blocks
		for (int x = 0; x < w; x++) {
			string line = "";
			for (int y = 0; y < h; y++) {
				for (int z = 0; z < d; z++) {
					line += data[x, y, z] + ",";
				}
				line = line.TrimEnd(',') + ";";
			}
			sr.WriteLine(line.TrimEnd(';'));
		}
		sr.Close();
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
