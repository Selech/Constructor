using UnityEngine;
using System.Collections;

public class BlueprintScript : MonoBehaviour {
	
	private ArrayList blueprintWood;
	private ArrayList blueprintDirt;
	private ArrayList blueprintStone;

	void Awake(){
		blueprintWood = new ArrayList ();
		blueprintDirt = new ArrayList ();
		blueprintStone = new ArrayList ();
	}

	public void AddPosition(BlockType type, Vector3 vec){
		if(type == BlockType.Dirt){
			blueprintDirt.Add(vec);
		}
		if(type == BlockType.Wood){
			blueprintWood.Add(vec);
		}
		if(type == BlockType.Stone){
			blueprintStone.Add(vec);
		}
	}

	public bool Contains(BlockType type, Vector3 vec){

		if(type == BlockType.Dirt){
			return blueprintDirt.Contains(vec);
		}
		if(type == BlockType.Wood){
			return blueprintWood.Contains(vec);
		}
		if (type == BlockType.Stone) {
			return blueprintStone.Contains (vec);
		} else {
			return false;
		}
	}

	public bool CheckSizes(ArrayList woods, ArrayList stones, ArrayList dirts){
		if (woods.Count == blueprintWood.Count && stones.Count == blueprintStone.Count && dirts.Count == blueprintDirt.Count) {
			RemoveBlueprint();
			return true;
		} else {
			return false;
		}
	}

	private void RemoveBlueprint(){
//		foreach (Transform child in this.gameObject.transform) {
//			GameObject.Destroy(child.gameObject);
//		}

		this.GetComponentInParent<BlueprintFactoryScript>().fileName = "SmallStoneHouse";
		this.GetComponentInParent<BlueprintFactoryScript>().LoadBlueprint();

		Destroy (this.gameObject);
	}
}
