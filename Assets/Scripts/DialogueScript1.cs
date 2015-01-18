using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueScript1 : MonoBehaviour 
{
	public Text dialogueText;
	public int i = 0;

	string dialogueString;

	GameObject mentorCanvas;
	GameObject robertCanvas;
	GameObject dialogueCanvas;

	// Use this for initialization
	void Start () 
	{
		dialogueText = GameObject.Find ("Dialogue Text").GetComponent<Text> ();
		mentorCanvas = GameObject.Find ("Mentor Canvas");
		robertCanvas = GameObject.Find ("Robert Canvas");
		dialogueCanvas = GameObject.Find ("Dialogue1");
	}
	
	// Update is called once per frame
	void Update () 
	{
		dialogueText.text = dialogueString;

		if (i == 0) 
		{
			dialogueString = "Robert: Woohoo! First day of work, hope it will be fun!";
		}

		else if (i == 1)
		{
			dialogueString = "Mysterious Old Man: Hey there young man! *coughs* Excuse me but can you please help me out? It's an emergency!";
			
			foreach (Transform child in mentorCanvas.transform)
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
			dialogueString = "Robert: Sorry, I would love to help you but I need to report for work right now.";

			foreach (Transform child in mentorCanvas.transform)
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
			dialogueString = "Mysterious Old Man: There is not much time left, Robert! I need your help urgently!";

			foreach (Transform child in mentorCanvas.transform)
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
			dialogueString = "Robert: How do you know my name!? Who are you!?";
			
			foreach (Transform child in mentorCanvas.transform)
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
			dialogueString = "Mysterious Old Man: There is no time to explain! *coughs* Please deliver this letter to the train station you are going to immediately!";
			
			foreach (Transform child in mentorCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
			
			foreach (Transform child in robertCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
		}

		else if (i == 6)
		{
			dialogueString = "Robert: Alright, I will deliver it since I'm heading there anyway.";
			
			foreach (Transform child in mentorCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in robertCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
		}

		else if (i == 7)
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
