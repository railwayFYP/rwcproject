using UnityEngine;
using System.Collections;

public class TimeAttackControl : MonoBehaviour {
    public int currentLevel = 1;

    public bool startingCine = true;

    public bool pauseGame = false;
    public bool gameCleared = false;
    public bool gameFailed = false;

    private LevelData levelData;
    private GameObject rGrid;
    private GameObject trainControl;

    public GameObject rDiag1;
    public GameObject rGUI;
    public GameObject rCamera;
    public GameObject rGUItext;

    public GameObject rWinCanvas;
    public GameObject rLoseCanvas;

    public GameObject r_9xTerrain;

    // Time Attack Scores and Bool
    public bool     starSpawned = false;
    int             starCollected = 0;

    int     starPrevX = -1;
    int     starPrevY = -1;

	// Use this for initialization
	void Start () {
        // This will call the loading for the all requirement for the level given.
        // everything will be loaded on the spot. Once it is done, it will move into dialogue screen.
        levelData = new LevelData();
        levelData.loadTimeLevel(currentLevel);

        // Find the gameobject for Grid

        rGrid = GameObject.Find("Grid");

        trainControl = GameObject.Find("TrainControl");

        // Prepare the grid
        this.GetComponent<GenerateGrid>().createGrid(levelData.mSizeX, levelData.mSizeY);

        populateGrid();

        generateTerrian();

        prepareCamera();
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
        //else if (pauseGame)
        //{
        //    // What to do during pause game lies here
        //}
        //else if (gameCleared)
        //{
        //    // Winning Branch
        //    rWinCanvas.SetActive(true);
        //}
        //else if (gameFailed)
        //{
        //    // end game -> show screen -> back to level selection
        //    rLoseCanvas.SetActive(true);
        //}        

        starSpawner();
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
                            TargetObj.transform.Rotate(0, 180, 0);
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

    void generateTerrian()
    {
          r_9xTerrain.SetActive(true);
          r_9xTerrain.transform.Translate(-21.1f, -0.1f, -5.1f);
    }

    void prepareCamera()
    {
        GameObject.Find("Main Camera").GetComponent<CameraScript>().setCameraBound(12.5f, -14, 26, 60);
    }

    void starSpawner()
    {
        if (!starSpawned)
        {
            while (!starSpawned)
            {
                int tempX = Random.Range(0, 9);
                int tempY = Random.Range(0, 9);

                foreach (Transform PlacementPlane in rGrid.transform)
                {
                    // Break if the x and y is same as the previous star
                    if (tempX == starPrevX && tempY == starPrevY)
                    {
                        break;
                    }
                    
                    // Get the grid data of the grid
                    GridData gData = PlacementPlane.GetComponent<GridData>();

                    // If this grid is same as the randomed
                    if (gData.posX == tempX && gData.posY == tempY)
                    {
                        // if is it not a building and not occupied
                        if (!gData.isBuilding && !gData.isOccupied)
                        {
                            // Set the plane tag to opened
                            PlacementPlane.tag = "Open";

                            // Create a star there
                            gData.isStar = true;

                            GameObject obj = GameObject.Find("Star");

                            GameObject createdObj = Instantiate(obj, PlacementPlane.transform.position, Quaternion.identity) as GameObject;

                            createdObj.name = "Star";

                            // ONLY CAUSE I AM USING MOUNTAIN AS STAR. TO BE REMOVED ONCE USING REAL STAR
                            createdObj.transform.localScale = new Vector3(0.5f,0.5f,0.5f);

                            //creates the obj as the child of the grid
                            createdObj.transform.parent = PlacementPlane.transform;

                            // Hide the obj
                            obj.transform.position += new Vector3(0, 100, 0);

                            starSpawned = true;

                            break;
                        }
                    }
                }
            }
        }
    }
}
