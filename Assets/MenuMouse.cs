using UnityEngine;
using System.Collections;

public class MenuMouse : MonoBehaviour {

	public ParticleSystem ps;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				string tag = hit.collider.gameObject.tag;
				if (tag == "Planet") {
					ClickedPlanet b = hit.collider.gameObject.GetComponent<ClickedPlanet> ();

					StopPs();

					b.Clicked ();
				}
			}
		} else {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				string tag = hit.collider.gameObject.tag;
				if (tag == "Planet") {
					ps = hit.collider.gameObject.GetComponentInChildren<ParticleSystem>();
					ps.Play();
				}
			}
			else if(ps != null){
				StopPs();
			}
		}



	}

	void StopPs(){
		ps.Stop();
		ps.Clear();
		ps = null;
	} 
}
