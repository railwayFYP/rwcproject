using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour 
{
	public Text tutorialText;
	public int i = 0;

	string tutorialString;

	GameObject tutorialCanvas;

	// Use this for initialization
	void Start () 
	{
		tutorialText = GameObject.Find ("Tutorial Text").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		tutorialText.text = tutorialString;

		if (i == 0) 
		{
			tutorialString = "Welcome to the Casual Mode of Choo Choo! In this mode, you will be able to build everything based on your creativity!";
		}

		else if (i == 1)
		{
			tutorialString = "We will start off by introducing the different buttons. The options menu can be accessed through the button on the top left hand corner. On the bottom left hand corner, you can press on the button to play/pause the train. On the right hand side, we got the different sections for construction.";
		}

		else if (i == 2)
		{
			tutorialString = "To build a track, simply go to the Tracks section and click on the desired track direction. Next, move your cursor to wherever you want to construct it and press left click.";
		}

		else if (i == 3)
		{
			tutorialString = "Once the track has been built, click on the Play/Pause button on the bottom left hand corner and the train will start moving!";
		}

		else if (i == 4)
		{
			tutorialString = "Have fun!";
		}

		else if (i == 5)
		{
			tutorialCanvas = GameObject.Find ("Tutorial Canvas");

			foreach (Transform child in tutorialCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
		}
	}

	public void pressContinue()
	{
		i++;
	}
}
