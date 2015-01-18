using UnityEngine;
using System.Collections;

// Written by NCA
public enum Building
{
	Depot,
	Windmill
};

public enum Track
{
	Horizontal,
	Vertical,
	DownLeft,
	DownRight,
	UpLeft,
	UpRight
};

public class GridData : MonoBehaviour {

	public int posX;
	public int posY;

	public bool isDepot;
	public bool isDestination;
	public bool isTrack;
	public bool isBuilding;
    public bool isOccupied;
    public bool isNotRemoveable = false;

	public Building buildingType;
    public Track TrackType;
 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
