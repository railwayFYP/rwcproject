using UnityEngine;
using System.Collections;

// Written by NCA
// Raycast by Jack

public class InputController : MonoBehaviour {

	RaycastHit hit;

	public string currentItemSelected;

	public bool currentlyBuilding;

	private float raycastLength = 500;

	//placement plane items

	private GameObject lastHitObj;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//----- FUNCTIONS FOR BUTTON PRESSED ON GUI -----
	public void DownUpPressed()
	{
		if (currentItemSelected != "Straight Track")
		{
			hidePrevious();
		}
		currentItemSelected = "Straight Track";
		currentlyBuilding = true;
	}

	public void LeftRightPressed()
	{
		if (currentItemSelected != "Vert Track")
		{
			hidePrevious();
		}
		currentItemSelected = "Vert Track";
		currentlyBuilding = true;
	}

	public void UpLeftPressed()
	{
		if (currentItemSelected != "UpLeft")
		{
			hidePrevious();
		}
		currentItemSelected = "UpLeft";
		currentlyBuilding = true;
	}

	public void UpRightPressed()
	{
		if (currentItemSelected != "UpRight")
		{
			hidePrevious();
		}
		currentItemSelected = "UpRight";
		currentlyBuilding = true;
	}

	public void DownLeftPressed()
	{
		if (currentItemSelected != "DownLeft")
		{
			hidePrevious();
		}
		currentItemSelected = "DownLeft";
		currentlyBuilding = true;
	}

	public void DownRightPressed()
	{
		if (currentItemSelected != "DownRight")
		{
			hidePrevious();
		}
		currentItemSelected = "DownRight";
		currentlyBuilding = true;
	}

	public void SteamTrainPressed()
	{
		if (currentItemSelected != "SteamTrain")
		{
			hidePrevious();
		}
		currentItemSelected = "SteamTrain";
		currentlyBuilding = true;
	}
	//----- END FUNCTIONS FOR BUTTON PRESSED ON GUI -----
}
