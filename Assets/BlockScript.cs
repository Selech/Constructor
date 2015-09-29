using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour {
	
	public int HP;
	public ParticleSystem ps;
	public string type;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Hit(GameObject player, Vector3 hitPos){
		HP -= 10;
		
		ps.transform.position = hitPos;
		ps.Play ();
		
		if(HP == 0){
			player.GetComponent<Inventory>().AddBlock(this.type);
			Destroy(this.gameObject);
		}
	}
}
