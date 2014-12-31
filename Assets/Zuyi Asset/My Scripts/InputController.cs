using UnityEngine;
using System.Collections;

// Written by NCA
// Raycast by Jack

public class InputController : MonoBehaviour {

	RaycastHit hit;

	public string currentItemSelected;

    public Track trackSelected;
    public Building buildingSelected;
    public TrainType trainSelected;

	public bool currentlyBuilding;
    public bool isTrack;
    public bool isBuilding;
    public bool isTrain;

	private float raycastLength = 500;

	//placement plane items
	private GameObject lastHitObj;

    void Update()
    {
        // Check if the mouse is within the playable area
        if (checkWithinBounds(Input.mousePosition.x, Input.mousePosition.y))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Send out the ray for checking
            if (Physics.Raycast(ray, out hit, raycastLength))
            {
                //if building mode is not enabled, AKA Trying to move the item to another grid
                if (hit.collider.name == "PlacementPlane(Clone)" && !currentlyBuilding)
                {
                    //lastHitObj = the grid which cursor is in
                    lastHitObj = hit.collider.gameObject;

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (lastHitObj.tag != "Open")
                        {
                            //destroy child of grid clicked
                            foreach (Transform child in lastHitObj.transform)
                            {
                                Destroy(child.gameObject);
                            }

                            GridData temp = lastHitObj.GetComponent<GridData>();

                            if (temp.isBuilding)
                            {
                                isBuilding = true;
                                buildingSelected = temp.buildingType;
                            }
                            else if (temp.isTrack)
                            {
                                isTrack = true;
                                trackSelected = temp.TrackType;
                            }

                            // Reset the Grid Data
                            temp.isBuilding = false;
                            temp.isTrack = false;
                            temp.isDepot = false;


                            currentlyBuilding = true;
                            lastHitObj.tag = "Open";
                        }

                    }
                }
                //if building mode is enabled, AKA trying to add new stuff to the grid
                else if (hit.collider.name == "PlacementPlane(Clone)" && currentlyBuilding)
                {
                    GameObject Target;

                    if (isTrack)
                    {
                        Target = GameObject.Find(trackSelected.ToString());
                    }
                    else if (isBuilding)
                    {
                        Target = GameObject.Find(buildingSelected.ToString());
                    }
                    else
                    {
                        Target = GameObject.Find(trainSelected.ToString());
                    }

                    //position of Target (current track selected) follows the position of hit.point (mouse cursor)
                    Target.transform.position = hit.point;

                    Vector3 offset = new Vector3(0, 8.75f, -2.5f);
                    if (isTrain)
                    {      
                        Target.transform.position += offset;
                    }
                    //lastHitObj = the grid which cursor is in
                    lastHitObj = hit.collider.gameObject;

                    //0 = left, 1 = right, 2 = middle mouse button
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (lastHitObj.tag == "Open" && !isTrain)
                        {
                            //create Target (current track selected) at  lastHitObj.transform.position (center of the grid which cursor is in)
                            GameObject TargetObj = Instantiate(Target, lastHitObj.transform.position, Quaternion.identity) as GameObject;

                            GridData temp = lastHitObj.GetComponent<GridData>();
                            // LAST WORK HERE, NOW NEED TO UPDATE THE GRID DATA

                            if (isTrack)
                            {
                                lastHitObj.tag = "Track";
                                temp.isTrack = true;
                                temp.TrackType = trackSelected;
                                TargetObj.name = "Track Created";
                            }
                            else if (isBuilding)
                            {
                                lastHitObj.tag = "Building";
                                temp.isBuilding = true;
                                temp.buildingType = buildingSelected;
                                TargetObj.name = "Building Created";
                            }

                            //creates the obj as the child of the grid
                            TargetObj.transform.parent = lastHitObj.transform;

                            //temporarily hide the track that is following the cursor by changing its position to be the same as the new track created
                            Target.transform.position = lastHitObj.transform.position;

                            isTrack = false;
                            isTrain = false;
                            isBuilding = false;
                            currentlyBuilding = false;
                        }
                        else if (lastHitObj.tag == "Track" && isTrain)
                        {                            
                            //create Target (current track selected) at  lastHitObj.transform.position (center of the grid which cursor is in)
                            GameObject TargetObj = Instantiate(Target, lastHitObj.transform.position + offset, Quaternion.identity) as GameObject;

                            GridData temp = lastHitObj.GetComponent<GridData>();

                            TargetObj.name = "Steam";

                            //temporarily hide the track that is following the cursor by changing its position to be the same as the new track created
                            Target.transform.position += new Vector3(0,100,0);

                            isTrack = false;
                            isTrain = false;
                            isBuilding = false;
                            currentlyBuilding = false;
                        }
                    }

                    //right click or press esc to cancel building
                    if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                    {
                        //temporarily hide the track that is following the cursor by moving it up above the camera
                        Target.transform.position = transform.up * 100;
                        isTrack = false;
                        isTrain = false;
                        isBuilding = false;
                        currentlyBuilding = false;
                    }
                }


            }
        }
        //Debug.DrawRay(ray.origin, ray.direction * raycastLength, Color.yellow);
    }

    bool checkWithinBounds(float mouseX, float mouseY)
    {
        // NCA: Check within screen
        // mouse x - 200 cause of the UIs
        if (mouseX < 0 || mouseX > Screen.width - 200 || mouseY < 0 || mouseY > Screen.height)
        {
            return false;
        }
        return true;
    }

    void hidePrevious()
    {
        GameObject hideTarget = GameObject.Find(currentItemSelected);

        if (hideTarget != null)
        {
            hideTarget.transform.position = transform.up * 100;
        }

    }

	//----- FUNCTIONS FOR BUTTON PRESSED ON GUI -----
	public void DownUpPressed()
	{
        if (trackSelected != Track.Vertical || !isTrack)
        {
            hidePrevious();
            isBuilding = false;
        }
        trackSelected = Track.Vertical;
        isTrack = true;
        currentlyBuilding = true;
	}

	public void LeftRightPressed()
	{
        if (trackSelected != Track.Horizontal || !isTrack)
        {
            hidePrevious();
            isBuilding = false;
            isTrain = false;
        }
        trackSelected = Track.Horizontal;
        isTrack = true;
		currentlyBuilding = true;
	}

	public void UpLeftPressed()
	{
		if (trackSelected != Track.UpLeft || !isTrack)
		{
			hidePrevious();
            isBuilding = false;
            isTrain = false;
		}
        trackSelected = Track.UpLeft;
        isTrack = true;
		currentlyBuilding = true;
	}

	public void UpRightPressed()
	{
        if (trackSelected != Track.UpRight || !isTrack)
        {
            hidePrevious();
            isBuilding = false;
            isTrain = false;
        }
        trackSelected = Track.UpRight;
        isTrack = true;
        currentlyBuilding = true;
	}

	public void DownLeftPressed()
	{
        if (trackSelected != Track.DownLeft || !isTrack)
        {
            hidePrevious();
            isBuilding = false;
            isTrain = false;
        }
        trackSelected = Track.DownLeft;
        isTrack = true;
        currentlyBuilding = true;
	}

	public void DownRightPressed()
	{
        if (trackSelected != Track.DownRight || !isTrack)
        {
            hidePrevious();
            isBuilding = false;
            isTrain = false;
        }
        trackSelected = Track.DownRight;
        isTrack = true;
        currentlyBuilding = true;
	}

	public void SteamTrainPressed()
	{
        if (trainSelected != TrainType.Steam || !isTrain)
        {
            hidePrevious();
            isBuilding = false;
            isTrack = false;
        }
        trainSelected = TrainType.Steam;
        isTrain = true;
        currentlyBuilding = true;
	}
	//----- END FUNCTIONS FOR BUTTON PRESSED ON GUI -----
}
