using UnityEngine;
using System.Collections;

public class GameModeMenuScript : MonoBehaviour 
{
	public void backButton()
	{
		Application.LoadLevel(0);
	}

	public void casualButton()
	{
		Application.LoadLevel(2);
	}

	public void missionButton()
	{
		if (GameControl.control.numOfMissionUnlocked == 1) 
		{
			Application.LoadLevel(3); //MissionModeMenu1
		}
		else if (GameControl.control.numOfMissionUnlocked == 2) 
		{
			Application.LoadLevel(4); //MissionModeMenu2
		}
		else if (GameControl.control.numOfMissionUnlocked == 3) 
		{
			Application.LoadLevel(5); //MissionModeMenu3
		}
	}

	public void timeAttackButton()
	{
		if (GameControl.control.numOfTimeAttackUnlocked == 0) 
		{
			Application.LoadLevel(6); //TimeAttackModeMenu0
		}
		else if (GameControl.control.numOfTimeAttackUnlocked == 1) 
		{
			Application.LoadLevel(7); //TimeAttackModeMenu1
		}
		else if (GameControl.control.numOfTimeAttackUnlocked == 2) 
		{
			Application.LoadLevel(8); //TimeAttackModeMenu2
		}
		else if (GameControl.control.numOfTimeAttackUnlocked == 3) 
		{
			Application.LoadLevel(9); //TimeAttackModeMenu3
		}
	}
}
