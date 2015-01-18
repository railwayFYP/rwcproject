using UnityEngine;
using System.Collections;

public class MissionModeMenuScript : MonoBehaviour 
{
	public void backButton()
	{
		Application.LoadLevel(1);
	}

	public void mission1Button()
	{
		Application.LoadLevel(10); //MissionModeLevel1
	}

	public void mission2Button()
	{
		Application.LoadLevel(10); //MissionModeLevel2
	}

	public void mission3Button()
	{
		Application.LoadLevel(10); //MissionModeLevel3
	}
}
