using UnityEngine;
using System.Collections;

public class OptionsMenuScript : MonoBehaviour {
	
	//refrence for the pause menu panel in the hierarchy
	public GameObject optionsMenuPanel;
	//animator reference
	private Animator anim;
	//variable for checking if the game is paused 
	private bool inOptions = false;
	// Use this for initialization
	void Start () {
		//unpause the game on start
		Time.timeScale = 1;
		//get the animator component
		anim = optionsMenuPanel.GetComponent<Animator>();
		//disable it on start to stop it from playing the default animation
		anim.enabled = false;
	}
	
	//function to pause the game
	public void EnterOptions(){
		//enable the animator component
		anim.enabled = true;
		//play the Slidein animation
		anim.Play("OptionsMenuSlideIn");
		//set the isPaused flag to true to indicate that the game is paused
		inOptions = true;
		//freeze the timescale
		Time.timeScale = 0;
	}
	//function to unpause the game
	public void ExitOptions(){
		//set the isPaused flag to false to indicate that the game is not paused
		inOptions = false;
		//play the SlideOut animation
		anim.Play("OptionsMenuSlideOut");
		//set back the time scale to normal time scale
		Time.timeScale = 1;
	}
}