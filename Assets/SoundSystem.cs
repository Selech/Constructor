using UnityEngine;
using System.Collections;

public class SoundSystem : MonoBehaviour {

	public static void PlaySound(string clip){
		AudioSource.PlayClipAtPoint((AudioClip)Resources.Load("Sounds/"+clip, typeof(AudioClip)),Camera.main.transform.position);
	}
}
