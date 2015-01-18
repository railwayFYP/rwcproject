using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueScript2 : MonoBehaviour 
{
	public Text dialogueText;
	public int i = 0;

	string dialogueString;

	GameObject staffCanvas;
	GameObject robertCanvas;
	GameObject dialogueCanvas;

	// Use this for initialization
	void Start () 
	{
		dialogueText = GameObject.Find ("Dialogue Text").GetComponent<Text> ();
		staffCanvas = GameObject.Find ("Staff Canvas");
		robertCanvas = GameObject.Find ("Robert Canvas");
		dialogueCanvas = GameObject.Find ("Mission Mode Dialogue 2 Canvas");
	}
	
	// Update is called once per frame
	void Update () 
	{
		dialogueText.text = dialogueString;

		if (i == 0) 
		{
			dialogueString = "Robert: Excuse me sir, here is a letter for you. It was from an old man saying it is important. ";
		}

		else if (i == 1)
		{
			dialogueString = "Station Staff: (reading the letter). Oh my god!! It is a bomb threat warning!! There is a few stations involved!! Can you pass this information to the other stations involving about this as well? I need to close off the area.";
			
			foreach (Transform child in staffCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
			
			foreach (Transform child in robertCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
		}

		else if (i == 2)
		{
			dialogueString = "Robert: (shocked). Yes!! I will go immediately";

			foreach (Transform child in staffCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in robertCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
		}

		else if (i == 3)
		{
			dialogueString = "Station Staff: Faster we need to inform to all stations by 5pm. Here are the locations of all the bombs in the other stations. Please inform all the other stations and their locations. ";

			foreach (Transform child in staffCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
			
			foreach (Transform child in robertCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
		}

		else if (i == 4)
		{
			dialogueString = "Robert: Yes sir!! (Rushing out of the office).";
			
			foreach (Transform child in staffCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in robertCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
		}

		else if (i == 5)
		{
			foreach (Transform child in dialogueCanvas.transform)
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
