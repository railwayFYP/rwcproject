using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionTutorialScript : MonoBehaviour 
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
			tutorialString = "Welcome to the Mission Mode of Choo Choo! Help Robert get to the other station by building tracks leading the train to its destination.";
		}

		else if (i == 1)
		{
			tutorialString = "To start constructing, simply click on the desired type of track on the right column followed by clicking on where you want to build it!";
		}

		else if (i == 2)
		{
			tutorialString = "Plan wisely as there is a limited number of tracks provided. Check the top right hand corner to know how many tracks you are left with!";
		}

		else if (i == 3)
		{
			tutorialString = "Once the train is ready to depart, click on the play button at the bottom left hand corner! Good luck and have fun!";
		}

		else if (i == 4)
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
