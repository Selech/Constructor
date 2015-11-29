using UnityEngine;
using System.Collections;

public class DesignerScript : MonoBehaviour {

	public string fileName;
	public bool saveMode;


	// Use this for initialization
	void Start () {
		BlueprintScript blueprint = this.transform.Find ("Blueprint").gameObject.GetComponent<BlueprintScript> ();
		if (saveMode) {
			DataLoader.SaveBlueprint(fileName, blueprint);
		} else {
			DataLoader.LoadBlueprint(fileName, blueprint);
		}
	}
}
