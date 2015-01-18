using UnityEngine;
using System.Collections;

public class AdjustScript : MonoBehaviour 
{
	void OnGUI()
	{
		if (GUI.Button (new Rect (10, 100, 100, 30), "Mission Up")) 
		{
			GameControl.control.numOfMissionUnlocked += 1;
		}
		if (GUI.Button (new Rect (10, 140, 100, 30), "Mission Down")) 
		{
			GameControl.control.numOfMissionUnlocked -= 1;
		}
		if (GUI.Button (new Rect (10, 180, 130, 30), "Time Attack Up")) 
		{
			GameControl.control.numOfTimeAttackUnlocked += 1;
		}
		if (GUI.Button (new Rect (10, 220, 130, 30), "Time Attack Down")) 
		{
			GameControl.control.numOfTimeAttackUnlocked -= 1;
		}
		if (GUI.Button (new Rect (10, 260, 100, 30), "Casual Up")) 
		{
			GameControl.control.numOfCasualUnlocked += 1;
		}
		if (GUI.Button (new Rect (10, 300, 100, 30), "Casual Down")) 
		{
			GameControl.control.numOfCasualUnlocked -= 1;
		}
		if (GUI.Button (new Rect (10, 340, 100, 30), "Save")) 
		{
			GameControl.control.Save ();
		}
		if (GUI.Button (new Rect (10, 380, 100, 30), "Load")) 
		{
			GameControl.control.Load ();
		}
	}
}
