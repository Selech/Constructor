using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlockScript : MonoBehaviour {
	private float HP;
	public byte type;

	// returns true if block breaks
	public bool Hit(Vector3 hitPos, float dmg){
		HP -= dmg;

		if(HP <= 0){
			return true;
		}
		else {
			return false;
		}
	}
}
