using UnityEngine;
using System.Collections;

// Written by NCA
// Raycast by Jack

public enum GameMode
{
    Casual,
    Mission,
    TimeAttack
};

public class InputController : MonoBehaviour {
    // Train conrol
    private bool moveTrain = false;

	RaycastHit hit;

	public string currentItemSelected;

    public Track trackSelected;
    public Building buildingSelected;
    public TrainType trainSelected;

    public GameMode currentMode = GameMode.Casual;

	public bool currentlyBuilding;
    public bool isTrack;
    public bool isBuilding;
    public bool isTrain;

    public bool paused = false;

	private float raycastLength = 500;

	//placement plane items
	private GameObject lastHitObj;

    public GameObject trainControl;
    
    private MissionControl rMissionControl;

    void Start()
    { 
         rMissionControl = this.GetComponent<MissionControl>(); 
    }

    void Update()
    {
        if (!paused)
        {
            handleInputs();
        }    
    }

    // NCA: Check within clickable screen
    bool checkWithinBounds(float mouseX, float mouseY)
    {
        
        // mouse x - 200 cause of the UIs
        if (mouseX < 0 || mouseX > Screen.width - 200 || mouseY < 0 || mouseY > Screen.height)
        {
            return false;
        }
        return true;
    }

    void handleInputs()
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
                            bool removeObj = false;

                            GridData temp = lastHitObj.GetComponent<GridData>();

                            if (!temp.isNotRemoveable)
                            {
                                if (temp.isBuilding && !temp.isOccupied)
                                {
                                    // User cannot remove any building in any other modes
                                    if (currentMode == GameMode.Casual)
                                    {
                                        isBuilding = true;
                                        buildingSelected = temp.buildingType;
                                        removeObj = true;
                                    }
                                }
                                else if (temp.isTrack && !temp.isOccupied)
                                {
                                    isTrack = true;
                                    trackSelected = temp.TrackType;
                                    removeObj = true;

                                    // Refund of track should be done here
                                    if (currentMode == GameMode.Mission)
                                    {
                                        rMissionControl.updateTrackUsage(trackSelected, true);
                                    }
                                }
                            }
                            
                            if (removeObj)
                            {
                                //destroy child of grid clicked
                                foreach (Transform child in lastHitObj.transform)
                                {
                                    Destroy(child.gameObject);
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

                    Vector3 offset = new Vector3(0, 0, 0);
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

                            if (isTrack)
                            {
                                lastHitObj.tag = "Track";
                                temp.isTrack = true;
                                temp.TrackType = trackSelected;
                                TargetObj.name = "Track Created";

                                if (currentMode == GameMode.Mission)
                                {
                                    rMissionControl.updateTrackUsage(trackSelected,false);
                                }
                            }
                            else if (isBuilding)
                            {
                                lastHitObj.tag = "Building";
                                temp.isBuilding = true;
                                temp.buildingType = buildingSelected;
                                TargetObj.name = "Building Created";

                                if (buildingSelected == Building.Depot)
                                {
                                    lastHitObj.tag = "Track";
                                    temp.isDepot = true;
                                    temp.isTrack = true;
                                    temp.TrackType = Track.Vertical;
                                    TargetObj.name = "Depot Created";
                                }
                            }

                            //creates the obj as the child of the grid
                            TargetObj.transform.parent = lastHitObj.transform;

                            //temporarily hide the track that is following the cursor by changing its position to be the same as the new track created
                            Target.transform.position = lastHitObj.transform.position * 100;

                            isTrack = false;
                            isTrain = false;
                            isBuilding = false;
                            currentlyBuilding = false;
                        }
                        else if (lastHitObj.tag == "Track" && isTrain)
                        {
                            GridData temp = lastHitObj.GetComponent<GridData>();
                            // Check if the track is occuppied by another track
                            if (!temp.isOccupied && temp.TrackType == Track.Vertical)
                            {
                                //create Target (current track selected) at  lastHitObj.transform.position (center of the grid which cursor is in)
                                GameObject TargetObj = Instantiate(Target, lastHitObj.transform.position, Quaternion.identity) as GameObject;

                                // Give the gameobject a name
                                TargetObj.name = "Steam";

                                // Get the TrainAI to set the position of the train

                                if (moveTrain)
                                {
                                    TargetObj.GetComponent<TrainAI>().moveTrain();
                                }

                                if (temp.isDepot)
                                {
                                    Transform depot = lastHitObj.transform.FindChild("Depot Created");
                                    TargetObj.GetComponent<TrainAI>().setDepotRef(depot.GetComponent<DepotAI>());
                                    TargetObj.GetComponent<TrainAI>().m_bInDepot = true;
                                }

                                TargetObj.GetComponent<TrainAI>().setTrainPos(temp.posX, temp.posY);
                                // Set the train track to be occupied so it cant be removed.
                                // no more new train can put on the track.
                                temp.isOccupied = true;

                                //temporarily hide the train that is following the cursor by changing its position to be the same as the new track created
                                Target.transform.position += new Vector3(0, 100, 0);

                                // Add train to train control
                                TargetObj.transform.parent = trainControl.transform;

                                isTrack = false;
                                isTrain = false;
                                isBuilding = false;
                                currentlyBuilding = false;
                            }
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
    }

    // NCA: This function will hide the gameobject selected if it is not the same as the currentSelected
    // NCA: E.g Previous selection is horizontal track, and current selection is vertical track. 
    // NCA: It will hide the horizontal track and change the one following the mouse to vertical track.
    void hidePrevious()
    {
        GameObject hideTarget;

        if (isTrack)
        {
            hideTarget = GameObject.Find(trackSelected.ToString());
        }
        else if (isBuilding)
        {
            hideTarget = GameObject.Find(buildingSelected.ToString());
        }
        else
        {
            hideTarget = GameObject.Find(trainSelected.ToString());
        }
        if (hideTarget != null)
        {
            hideTarget.transform.position = transform.up * 100;
        }

    }

	//----- FUNCTIONS FOR BUTTON PRESSED ON GUI -----
	public void DownUpPressed()
	{
        if (rMissionControl.checkTrackAvail(Track.Vertical) || currentMode != GameMode.Mission)
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
	}

	public void LeftRightPressed()
	{
        if (rMissionControl.checkTrackAvail(Track.Horizontal) || currentMode != GameMode.Mission)
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
	}

	public void UpLeftPressed()
	{
        if (rMissionControl.checkTrackAvail(Track.UpLeft) || currentMode != GameMode.Mission)
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
	}

	public void UpRightPressed()
	{
        if (rMissionControl.checkTrackAvail(Track.UpRight) || currentMode != GameMode.Mission)
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
	}

	public void DownLeftPressed()
	{
        if (rMissionControl.checkTrackAvail(Track.DownLeft) || currentMode != GameMode.Mission)
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
	}

	public void DownRightPressed()
	{
        if (rMissionControl.checkTrackAvail(Track.DownRight) || currentMode != GameMode.Mission)
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
	}
    // Buildings
    public void DepotPressed()
    {
        if (buildingSelected != Building.Depot || !isBuilding)
        {
            hidePrevious();
            isTrack = false;
            isTrain = false;
        }
        buildingSelected = Building.Depot;
        isBuilding = true;
        currentlyBuilding = true;
    }

    public void WindmillPressed()
    {
        if (buildingSelected != Building.Windmill || !isBuilding)
        {
            hidePrevious();
            isTrack = false;
            isTrain = false;
        }
        buildingSelected = Building.Windmill;
        isBuilding = true;
        currentlyBuilding = true;
    }
    // Trains
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

    public void PlayPausePressed()
    {
        moveTrain = !moveTrain;
         foreach (Transform child in trainControl.transform)
        {
            child.GetComponent<TrainAI>().moveTrain();
        }
    }
}
