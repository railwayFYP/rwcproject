using UnityEngine;
using System.Collections;

public class ChangeMenu : MonoBehaviour 
{
	//1=trains, 2=tracks, 3=buildings, 4=miscs
	
	public bool trainsEnabled = true;
	public bool tracksEnabled = false;
	public bool buildingsEnabled = false;
	public bool objectsEnabled = false;

	public void trainsButtonPressed (GameObject trainsCanvas)
	{
		if (tracksEnabled) 
		{
			GameObject tracksCanvas = GameObject.Find ("Tracks Canvas");
			
			foreach (Transform child in tracksCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in trainsCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
			
			trainsEnabled = true;
			tracksEnabled = false;
		}
		
		else if (buildingsEnabled) 
		{
			GameObject buildingsCanvas = GameObject.Find ("Buildings Canvas");
			
			foreach (Transform child in buildingsCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in trainsCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
			
			trainsEnabled = true;
			buildingsEnabled = false;
		}
		
		else if (objectsEnabled) 
		{
			GameObject miscsCanvas = GameObject.Find ("Objects Canvas");
			
			foreach (Transform child in miscsCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in trainsCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
			
			trainsEnabled = true;
			objectsEnabled = false;
		}
	}

	public void tracksButtonPressed (GameObject tracksCanvas)
	{
		if (trainsEnabled) 
		{
			GameObject trainsCanvas = GameObject.Find ("Trains Canvas");

			foreach (Transform child in trainsCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in tracksCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}

			tracksEnabled = true;
			trainsEnabled = false;
		}

		else if (buildingsEnabled) 
		{
			GameObject buildingsCanvas = GameObject.Find ("Buildings Canvas");
			
			foreach (Transform child in buildingsCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in tracksCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}

			tracksEnabled = true;
			buildingsEnabled = false;
		}

		else if (objectsEnabled) 
		{
			GameObject miscsCanvas = GameObject.Find ("Objects Canvas");
			
			foreach (Transform child in miscsCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in tracksCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
			
			tracksEnabled = true;
			objectsEnabled = false;
		}
	}

	public void buildingsButtonPressed (GameObject buildingsCanvas)
	{
		if (trainsEnabled) 
		{
			GameObject trainsCanvas = GameObject.Find ("Trains Canvas");
			
			foreach (Transform child in trainsCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in buildingsCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
			
			buildingsEnabled = true;
			trainsEnabled = false;
		}
		
		else if (tracksEnabled) 
		{
			GameObject tracksCanvas = GameObject.Find ("Tracks Canvas");
			
			foreach (Transform child in tracksCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in buildingsCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
			
			buildingsEnabled = true;
			tracksEnabled = false;
		}
		
		else if (objectsEnabled) 
		{
			GameObject miscsCanvas = GameObject.Find ("Objects Canvas");
			
			foreach (Transform child in miscsCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in buildingsCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
			
			buildingsEnabled = true;
			objectsEnabled = false;
		}
	}

	public void objectsButtonPressed (GameObject objectsCanvas)
	{
		if (trainsEnabled) 
		{
			GameObject trainsCanvas = GameObject.Find ("Trains Canvas");
			
			foreach (Transform child in trainsCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in objectsCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
			
			objectsEnabled = true;
			trainsEnabled = false;
		}
		
		else if (tracksEnabled) 
		{
			GameObject tracksCanvas = GameObject.Find ("Tracks Canvas");
			
			foreach (Transform child in tracksCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in objectsCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
			
			objectsEnabled = true;
			tracksEnabled = false;
		}
		
		else if (buildingsEnabled) 
		{
			GameObject buildingsCanvas = GameObject.Find ("Buildings Canvas");
			
			foreach (Transform child in buildingsCanvas.transform)
			{
				child.gameObject.SetActive(false);
			}
			
			foreach (Transform child in objectsCanvas.transform)
			{
				child.gameObject.SetActive(true);
			}
			
			objectsEnabled = true;
			buildingsEnabled = false;
		}
	}
}
