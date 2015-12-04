using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class DataLoader {

	public static BlueprintScript LoadBlueprint(string fileName) {
		TextAsset file = (TextAsset)Resources.Load("Blueprints/Data/" + fileName, typeof(TextAsset));
		System.IO.StringReader reader = new System.IO.StringReader(file.text);

		// create new blueprint
		GameObject obj = (GameObject)Resources.Load("Blueprints/Blueprint", typeof(GameObject));
		BlueprintScript blueprint = obj.AddComponent<BlueprintScript> ();

		// read data file
		string line = reader.ReadLine();
		// read dimensions
		String[] data = line.Split (',');
		int w = int.Parse(data[0]);
		int h = int.Parse(data[1]);
		int d = int.Parse(data[2]);
		int ax = int.Parse(data[3]);
		int ay = int.Parse(data[4]);
		blueprint.SetDimensions (new Vector3(w, h, d), new Vector2(ax, ay));


		// read block data
		byte[,,] bpData = new byte[w, h, d];
		for (int x = 0; x < w; x++) {
			line = reader.ReadLine ();
			data = line.Split(';');
			for (int y = 0; y < h; y++) {
				string[] data2 = data[y].Split(',');
				for (int z = 0; z < d; z++) {
					bpData[x, y, z] = byte.Parse(data2[z]);
				}
			}
		}
		blueprint.data = bpData;
		reader.Close();
		return blueprint;
	}

	public static void SaveBlueprint(string fileName, int w, int h, int d, int ax, int az, byte[,,] data) {
		var sr = File.CreateText("Assets/Resources/Blueprints/Data/" + fileName + ".txt");
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
}
