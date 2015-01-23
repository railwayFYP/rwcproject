using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionControl : MonoBehaviour {
    public int currentLevel = 1;

    public bool startingCine = true;

    public bool pauseGame = false;
    public bool gameCleared = false;
    public bool gameFailed = false;
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

    public GameObject rDiag1;
    public GameObject rGUI;
    public GameObject rCamera;
    public GameObject rGUItext;

    public GameObject rWinCanvas;
    public GameObject rLoseCanvas;

    public GameObject r_5xTerrain;
    public GameObject r_7xTerrain;
    public GameObject r_9xTerrain;
    public GameObject r_10xTerrain;

	// Use this for initialization
	void Start () {
        // This will call the loading for the all requirement for the level given.
        // everything will be loaded on the spot. Once it is done, it will move into dialogue screen.
        levelData = new LevelData();
        levelData.loadGameLevel(currentLevel);

        // Find the gameobject for Grid

        rGrid = GameObject.Find("Grid");

        trainControl = GameObject.Find("TrainControl");

        // Prepare the grid
        this.GetComponent<GenerateGrid>().createGrid(levelData.mSizeX, levelData.mSizeY);

        populateGrid();

        loadTrackGiven();
        
        generateTerrian();

        prepareCamera();

        updateGUItext(Track.Vertical);
        updateGUItext(Track.Horizontal);
        updateGUItext(Track.UpLeft);
        updateGUItext(Track.UpRight);
        updateGUItext(Track.DownLeft);
        updateGUItext(Track.DownRight);
	}
	
	// Update is called once per frame
	void Update () {
        if (startingCine)
        {
            // Starting Cinematic added here
            if (!rDiag1.activeSelf)
            {
                startingCine = false;
                rGUI.SetActive(true);
                rCamera.GetComponent<CameraScript>().toggleCameraUpdate();
            }   
        }
        else if (pauseGame)
        {
            // What to do during pause game lies here
        }
        else if (gameCleared)
        {
            // Winning Branch
            rWinCanvas.SetActive(true);
        }
        else if(gameFailed)
        {
            // end game -> show screen -> back to level selection
            rLoseCanvas.SetActive(true);
        }        
	}

    // NCA: Function that will create the object on the grid
    // Currently to create train on start grid.
    void populateGrid()
    { 
        // Change the list of building into array for easy reading
        ObjData[] temp = levelData.vObjects.ToArray();
        for (int i = 0; i < temp.Length; i++)
        {
            foreach (Transform PlacementPlane in rGrid.transform)
            {
                GridData gData = PlacementPlane.GetComponent<GridData>();

                if (gData.posX == temp[i].posX && gData.posY == temp[i].posY)
                {
                    // Create object
                    GameObject buildingObj;

                    if (temp[i].isTrack)
                    {
                        // Track
                        buildingObj = GameObject.Find(temp[i].trackType.ToString());
                    }
                    else if (temp[i].isBuilding)
                    {
                        // Building
                        buildingObj = GameObject.Find(temp[i].type.ToString());
                    }
                    else
                    {
                        // Train
                        buildingObj = GameObject.Find(temp[i].trainType.ToString());
                    }

                    GameObject TargetObj = Instantiate(buildingObj, PlacementPlane.transform.position, Quaternion.identity) as GameObject;

                    if (temp[i].isTrack)
                    {
                        // Track
                        buildingObj = GameObject.Find(temp[i].trackType.ToString());

                        PlacementPlane.tag = "Track";
                        gData.isTrack = true;
                        gData.TrackType = temp[i].trackType;

                        TargetObj.name = "Track Created";

                        //creates the obj as the child of the grid
                        TargetObj.transform.parent = PlacementPlane.transform;

                        gData.isNotRemoveable = true;
                    }
                    else if (temp[i].isBuilding)
                    {
                        // Building
                        buildingObj = GameObject.Find(temp[i].type.ToString());

                        PlacementPlane.tag = "Building";
                        gData.isBuilding = true;
                        gData.buildingType = temp[i].type;

                        TargetObj.name = "Building Created";                       

                        if (temp[i].type == Building.Depot)
                        {
                            TargetObj.name = "Depot Created";
                            TargetObj.tag = "Track";

                            gData.isDepot = true;
                            gData.isTrack = true;
                            gData.TrackType = Track.Vertical;
                        }
                        if (temp[i].type == Building.Windmill || temp[i].type == Building.Windmill2)
                        {
                            TargetObj.transform.Rotate(0,180,0);
                        }
                        //creates the obj as the child of the grid
                        TargetObj.transform.parent = PlacementPlane.transform;

                        // Need to do something to stop the user from moving the buildings
                        // probably need to add in a bool in inputcontroller concerning the modes
                        gData.isNotRemoveable = true;
                    }
                    else
                    {
                        // Train
                        buildingObj = GameObject.Find(temp[i].trainType.ToString());
                    }

                    buildingObj.transform.position += new Vector3(0, 100, 0);

                    if (gData.posX == levelData.eGridX && gData.posY == levelData.eGridY)
                    {
                        gData.isDestination = true;
                    }
                    // If it is starting grid create a train
                    if (gData.posX == levelData.sGridX && gData.posY == levelData.sGridY)
                    {
                        // Create Train
                        // Add a train here
                        GameObject TrainObj;

                        TrainObj = GameObject.Find("Electric");

                        //create Target (current track selected) at  lastHitObj.transform.position (center of the grid which cursor is in)
                        GameObject Train = Instantiate(TrainObj, PlacementPlane.transform.position, Quaternion.identity) as GameObject;

                        // Give the gameobject a name
                        Train.name = "Diesel";

                        Train.GetComponent<TrainAI>().setTrainPos(gData.posX, gData.posY);

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

    void generateTerrian()
    {
        // x = width y = height z = length
        //Debug.Log(r_terrian.GetComponent<Terrain>().terrainData.size.x);
        //Debug.Log(r_terrian.GetComponent<Terrain>().terrainData.size.y);
        //Debug.Log(r_terrian.GetComponent<Terrain>().terrainData.size.z);
        switch (currentLevel)
        {
            case 1:
                {
                   // r_terrian.GetComponent<Terrain>().transform
                    // .size = new Vector3(80,400,80);
                    r_5xTerrain.SetActive(true);
                    r_5xTerrain.transform.Translate(-19.2f, -0.1f, -5.2f);
                    //r_terrian;
                    break;
                }
            case 2:
                {
                    //r_terrian.GetComponent<Terrain>().terrainData.size.Set(115, 400, 105);
                    r_7xTerrain.SetActive(true);
                    r_7xTerrain.transform.Translate(-26.7f, -0.1f, -4.9f);
                    //r_terrian.transform.
                    break;
                }
            case 3:
                {
                    r_9xTerrain.SetActive(true);
                    r_9xTerrain.transform.Translate(-21.1f, -0.1f, -4.9f);
                    //r_terrian.transform.Translate(-38.9f, -0.1f, -5.2f);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    void prepareCamera()
    {        
        switch (currentLevel)
        {
            case 1:
                {
                    GameObject.Find("Main Camera").GetComponent<CameraScript>().setCameraBound(11.5f, -14.5f, 12, 50);
                    break;
                }
            case 2:
                {
                    GameObject.Find("Main Camera").GetComponent<CameraScript>().setCameraBound(10.5f, -14.5f, 20, 56);
                    break;
                }
            case 3:
                {
                    GameObject.Find("Main Camera").GetComponent<CameraScript>().setCameraBound(12.5f, -14.5f, 27, 80);
                    break;
                }
        }
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

        updateGUItext(_type);
    }

    public void updateGUItext(Track _type)
    {
        switch (_type)
        {
            case Track.Vertical:
                {
                    rGUItext.transform.FindChild("Vertical Available").GetComponent<Text>().text = VertTrack.ToString() + " Vertical Track" ;
                    break;
                }
            case Track.Horizontal:
                {
                    rGUItext.transform.FindChild("Horizontal Available").GetComponent<Text>().text = HoriTrack.ToString() + " Horizontal Track";
                    break;
                }
            case Track.UpLeft:
                {
                    rGUItext.transform.FindChild("Up Left Available").GetComponent<Text>().text = ULTrack.ToString() + " Up Left Track";
                    break;
                }
            case Track.UpRight:
                {
                    rGUItext.transform.FindChild("Up Right Available").GetComponent<Text>().text = URTrack.ToString() + " Up Right Track";
                    break;
                }
            case Track.DownLeft:
                {
                    rGUItext.transform.FindChild("Down Left Available").GetComponent<Text>().text = DLTrack.ToString() + " Down Left Track";
                    break;
                }
            case Track.DownRight:
                {
                    rGUItext.transform.FindChild("Down Right Available").GetComponent<Text>().text = DRTrack.ToString() + " Down Right Track";
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void setWin()
    {
        gameCleared = true;
    }

    public void setLose()
    {
        if (!gameCleared)
        {
            gameFailed = true;
        }
    }
}
