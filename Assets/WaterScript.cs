using UnityEngine;
using System.Collections;

public class WaterScript : MonoBehaviour {
	private bool rising = true;

	// Update is called once per frame
	void Update () {
		if(this.transform.position.y > 15 && rising){
			rising = false;
		}

		if(this.transform.position.y < 0 && !rising){
			rising = true;
		}

		if (rising) {
			this.transform.Translate(new Vector3(0, 0.005f, 0));
		} else {
			this.transform.Translate(new Vector3(0, -0.01f, 0));
		}
	}
}
