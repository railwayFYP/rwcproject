using UnityEngine;
using System.Collections;

public class TimeAttackModeMenuScript : MonoBehaviour 
{
	public void backButton()
	{
		Application.LoadLevel(1);
	}

	public void timeAttack1Button()
	{
		Application.LoadLevel(10); //TimeAttackModeLevel1
	}

	public void timeAttack2Button()
	{
		Application.LoadLevel(10); //TimeAttackModeLevel2
	}

	public void timeAttack3Button()
	{
		Application.LoadLevel(10); //TimeAttackModeLevel3
	}
}
