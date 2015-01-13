using UnityEngine;
using System.Collections;

public class MissionControl : MonoBehaviour {
    public int currentLevel = 1;

    public bool startingCine = true;

    public bool pauseGame = false;
    public bool gameCleared = false;
    public bool startUp = false;

    // Number of tracks
    // Tracks that are given on the level
    public int VertTrack = 0;
    public int HoriTrack = 0;
    public int URTrack = 0;
    public int ULTrack = 0;
    public int DRTrack = 0;
    public int DLTrack = 0;

    // Game Data
    private LevelData   levelData;
    private GameObject  rGrid;
    private GameObject  trainControl;

	// Use this for initialization
	void Start () {
        // This will call the loading for the all requirement for the level given.
        // everything will be loaded on the spot. Once it is done, it will move into dialogue screen.
        levelData = new LevelData();
        levelData.loadGameLevel(1);

        // Find the gameobject for Grid

        rGrid = GameObject.Find("Grid");

        trainControl = GameObject.Find("TrainControl");

        // Prepare the grid
        this.GetComponent<GenerateGrid>().createGrid(levelData.mSizeX, levelData.mSizeY);

        populateGrid();

        loadTrackGiven();
	}
	
	// Update is called once per frame
	void Update () {
        if (startingCine)
        {
            // Starting Cinematic added here
        }
        else if (pauseGame)
        {
            // What to do during pause game lies here
        }
        else if (gameCleared)
        {
            // Winning Branch
        }
        else
        {
            // Game logic lies here
        }        
	}

    void populateGrid()
    { 
        // Change the list of building into array for easy reading
        BuildingData[] temp = levelData.vBuildings.ToArray();
        for (int i = 0; i < temp.Length; i++)
        {
            foreach (Transform PlacementPlane in rGrid.transform)
            {
                GridData gData = PlacementPlane.GetComponent<GridData>();

                if (gData.posX == levelData.eGridX && gData.posY == levelData.eGridY)
                {
                    gData.isDestination = true;
                }
                else if (gData.posX == temp[i].posX && gData.posY == temp[i].posY)
                {
                    // Create object
                    GameObject buildingType;

                    buildingType = GameObject.Find(temp[i].type.ToString());

                    GameObject TargetObj = Instantiate(buildingType, PlacementPlane.transform.position, Quaternion.identity) as GameObject;

                    PlacementPlane.tag = "Building";
                    gData.isBuilding = true;
                    gData.buildingType = temp[i].type;

                    TargetObj.name = "Building Created";

                    // Need to do something to stop the user from moving the buildings
                    // probably need to add in a bool in inputcontroller concerning the modes
                    gData.isOccupied = true;

                    buildingType.transform.position += new Vector3(0, 100, 0);

                    //creates the obj as the child of the grid
                    TargetObj.transform.parent = PlacementPlane.transform;

                    if (temp[i].type == Building.Depot)
                    {
                        TargetObj.name = "Depot Created";
                        TargetObj.tag = "Track";

                        gData.isDepot = true;
                        gData.isTrack = true;
                        gData.TrackType = Track.Vertical;

                        // Add a train here
                        GameObject TrainObj;

                        TrainObj = GameObject.Find("Steam");

                        //create Target (current track selected) at  lastHitObj.transform.position (center of the grid which cursor is in)
                        GameObject Train = Instantiate(TrainObj, PlacementPlane.transform.position, Quaternion.identity) as GameObject;

                        // Give the gameobject a name
                        Train.name = "Steam";

                        Transform depot = PlacementPlane.FindChild("Depot Created");
                        Train.GetComponent<TrainAI>().setDepotRef(depot.GetComponent<DepotAI>());
                        Train.GetComponent<TrainAI>().m_bInDepot = true;

                        Train.GetComponent<TrainAI>().setTrainPos(gData.posX,gData.posY);

                        //temporarily hide the train that is following the cursor by changing its position to be the same as the new track created
                        TrainObj.transform.position += new Vector3(0, 100, 0);

                        // Add train to train control
                        Train.transform.parent = trainControl.transform;
                    }
                }
            }
        }
    }

    void loadTrackGiven()
    {
        VertTrack   = levelData.VertTrack;
        HoriTrack   = levelData.HoriTrack;
        URTrack     = levelData.URTrack;
        ULTrack     = levelData.ULTrack;
        DRTrack     = levelData.DRTrack;
        DLTrack     = levelData.DLTrack;
    }

    // Input controller will call this function to check if the track is avail before allowing player 
    // to build the item
    public bool checkTrackAvail(Track _type)
    {
        switch(_type)
        {
            case Track.Vertical:
                {
                    if (VertTrack > 0)
                    {
                        return true;
                    }
                    break;
                }
            case Track.Horizontal:
                {
                    if (HoriTrack > 0)
                    {
                        return true;
                    }
                    break;
                }
            case Track.UpLeft:
                {
                    if (ULTrack > 0)
                    {
                        return true;
                    }
                    break;
                }
            case Track.UpRight:
                {
                    if (URTrack > 0)
                    {
                        return true;
                    }
                    break;
                }
            case Track.DownLeft:
                {
                    if (DLTrack > 0)
                    {
                        return true;
                    }
                    break;
                }
            case Track.DownRight:
                {
                    if (DRTrack > 0)
                    {
                        return true;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
        return false;
    }

    // Input controller will call this function when player 
    public void updateTrackUsage(Track _type, bool refund)
    {
        switch (_type)
        {
            case Track.Vertical:
                {
                    if (refund)
                    {
                        VertTrack++;
                    }
                    else
                    {
                        VertTrack--;
                    }
                    break;
                }
            case Track.Horizontal:
                {
                    if (refund)
                    {
                        HoriTrack++;
                    }
                    else
                    {
                        HoriTrack--;
                    }
                    break;
                }
            case Track.UpLeft:
                {
                    if (refund)
                    {
                        ULTrack++;
                    }
                    else
                    {
                        ULTrack--;
                    }
                    break;
                }
            case Track.UpRight:
                {
                    if (refund)
                    {
                        URTrack++;
                    }
                    else
                    {
                        URTrack--;
                    }
                    break;
                }
            case Track.DownLeft:
                {
                    if (refund)
                    {
                        DLTrack++;
                    }
                    else
                    {
                        DLTrack--;
                    }
                    break;
                }
            case Track.DownRight:
                {
                    if (refund)
                    {
                        DRTrack++;
                    }
                    else
                    {
                        DRTrack--;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

}
