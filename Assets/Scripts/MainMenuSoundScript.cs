using UnityEngine;
using System.Collections;

public class MainMenuSoundScript : MonoBehaviour 
{
	public AudioSource[] sounds;
	
	IEnumerator playSounds()
	{
		sounds[0].Play();						//play choo-choo
		yield return new WaitForSeconds (2);	//wait 2 seconds
		sounds[1].Play();						//play bgm
	}
	
	void Awake()
	{
		StartCoroutine (playSounds());
	}
}
