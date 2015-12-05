using UnityEngine;
using System.Collections;

public class BuildingBlock : MonoBehaviour {

	public Material select;
	public Material dirt;
	public Material stone;
	public Material wood;

	public void SetMaterial(byte type){
		switch(type){
		case 0: this.GetComponent<MeshRenderer>().material = select; break;
		case 1: this.GetComponent<MeshRenderer>().material = dirt; break;
		case 2: this.GetComponent<MeshRenderer>().material = stone; break;
		case 3: this.GetComponent<MeshRenderer>().material = wood; break;
		}
	}

	public void SetHighlightPosition(GameObject quad){
		this.transform.localScale = new Vector3(1.01f,1.01f,1.01f);
		Vector3 pos = quad.transform.position;

		switch (quad.name) {
		case "top": 
			pos += new Vector3(0,-0.5f,0); break;
			
		case "bottom": 
			pos += new Vector3(0,0.5f,0); break;
			
		case "left": 
			pos += new Vector3(0.5f,0,0); break;
			
		case "right": 
			pos += new Vector3(-0.5f,0,0); break;
			
		case "back": 
			pos += new Vector3(0,0f,-0.5f); break;
			
		case "front": 
			pos += new Vector3(0,0f,0.5f); break;
			
		default:
			break;
		}

		this.transform.rotation = new Quaternion ();
		this.transform.position = pos;
	}

	public void SetBlockPosition(Vector3 pos){
		this.transform.localScale = new Vector3(1f,1f,1f);
		this.transform.rotation = new Quaternion ();
		this.transform.position = pos;

	}
}
