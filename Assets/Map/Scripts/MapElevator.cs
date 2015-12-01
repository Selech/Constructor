using UnityEngine;
using System.Collections;

public class MapElevator : MonoBehaviour {
	public int cycle;
	public int elevationSpan;
	public int maxElevationChange;
	public int surfaceLevel; 
	private float[,] elevation;

	public void GenerateElevation(int seed) {
		Random.seed = seed;
		elevation = new float[cycle, cycle];
		for (int i = 0; i < cycle; i++) {
			for (int j = 0; j < cycle; j++) {
				elevation[i, j] = 2 * Random.value - 1;
			}
		}
	}

	public int GetElevation(int x, int z) {
		int y = z;

		// get the elevation index associated with the coordinates
		int x0 = x < 0 ? x / elevationSpan - 1 : x / elevationSpan;
		int y0 = y < 0 ? y / elevationSpan - 1 : y / elevationSpan;
		int x1 = x0 + 1;
		int y1 = y0 + 1;

		// calculate weights
		float dx = (x - x0 * elevationSpan) / (float)elevationSpan;
		float dy = (y - y0 * elevationSpan) / (float)elevationSpan;

		// Calculate elevation change from the weights of all elevation points
		float v0 = Mathf.Lerp (elevation[x0, y0], elevation[x1, y0], dx);
		float v1 = Mathf.Lerp (elevation[x0, y1], elevation[x1, y1], dx);
		return surfaceLevel + (int)(Mathf.Lerp (v0, v1, dy) * maxElevationChange);
	}
}
