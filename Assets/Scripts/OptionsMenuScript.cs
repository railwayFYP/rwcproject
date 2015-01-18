using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour 
{
	//refrence for the pause menu panel in the hierarchy
	public GameObject optionsMenuPanel;

	//animator reference
	private Animator anim;

	public GameObject tutorialCanvas;

	public Toggle fullScreenToggle;
	public Toggle windowedToggle;
	
	// Use this for initialization
	void Start () 
	{
		//unpause the game on start
		Time.timeScale = 1;

		//get the animator component
		anim = optionsMenuPanel.GetComponent<Animator>();

		//disable it on start to stop it from playing the default animation
		anim.enabled = false;

		//initialise tutorialCanvas
		tutorialCanvas = GameObject.Find ("Tutorial Canvas");

//------------------------------------------------------------------------------------------------------------------

		fullScreenToggle = GameObject.Find ("Fullscreen Toggle").GetComponent<Toggle>();
		windowedToggle = GameObject.Find ("Windowed Toggle").GetComponent<Toggle>();

		if (Screen.fullScreen == true) 
		{
			fullScreenToggle.isOn = true;
		}
		else if (Screen.fullScreen == false) 
		{
			windowedToggle.isOn = true;
		}
	}

	//function to enter options
	public void EnterOptions()
	{
		//enable the animator component
		anim.enabled = true;

		//play the Slidein animation
		anim.Play("OptionsMenuSlideIn");

		//freeze the timescale
		Time.timeScale = 0;

		//disable Tutorial Canvas when entering options (so it does not obstruct the Back button)
		foreach (Transform child in tutorialCanvas.transform)
		{
			child.gameObject.SetActive(false);
		}
	}

	//function to exit options
	public void ExitOptions()
	{
		//play the SlideOut animation
		anim.Play("OptionsMenuSlideOut");

		//set back the time scale to normal time scale
		Time.timeScale = 1;

		//enable Tutorial Canvas when exiting options
		foreach (Transform child in tutorialCanvas.transform)
		{
			child.gameObject.SetActive(true);
		}
	}

	public void exitFullScreen()
	{
		if (Screen.fullScreen == true) 
		{
			Screen.fullScreen = false;
			windowedToggle.isOn = true;
		}
	}

	public void enterFullScreen()
	{
		if (Screen.fullScreen == false) 
		{
			Screen.fullScreen = true;
			fullScreenToggle.isOn = true;
		}
	}
}