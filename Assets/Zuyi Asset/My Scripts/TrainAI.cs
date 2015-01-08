using UnityEngine;
using System.Collections;

// Written by NCA

// EASY AI SPEED
// MOVE 0.1
// TURN 1.15
// SNAP 5

// MED AI SPEED
// MOVE 0.2
// TURN 2.3
// SNAP 8

// HARD AI SPEED
// MOVE 0.3
// TURN 3.3
// SNAP 15

public enum TrainType
{
    Steam,
    Diesel,
    Electric
};

// NCA: Train Basic AI
public class TrainAI : MonoBehaviour {
    // Debug use 
    public bool         reset = false;
    public float        facing;
    // Constant For Tracks
    const int           INVALID = 0;
    const int           VALID   = 1;
    const int           OUT     = 2;
    const int           USED    = 3;
    const int           DEPOT   = 4;

    // Constant Direction for the train movement
    const int           UP      = 1;
    const int           RIGHT   = 2;
    const int           DOWN    = 3;
    const int           LEFT    = 4;

    // Const angle for the train to Steer toward
    const float         STEERUP     = 0;
    const float         STEERLEFT   = 270;
    const float         STEERRIGHT  = 90;
    const float         STEERDOWN   = 180;

    // Bool for the train to move
    public bool         m_bMove     = false;
    public bool         m_bWaiting  = false;
    public bool         m_bInDepot  = false;

    // Bool for the train to trigger the snapping when steering
    public bool         m_bSnap         = true;
    public float        m_fSnapStrength = 5;

    // Bool for the train to check for next track / If it is still on a track.
    public bool         m_bNextTrack = true;
    public bool         m_bStopOnTrack = false;
    public int          m_nTrackData = INVALID;

    // Train Steering and movement speed
    public float        m_fMovespeed = 0.1f;
    public float        m_fTurnspeed = 2.0f;

    // Beta Feature
    // False will make it minus from rotation and True will make it increase from rotation
    public bool         m_bTurnFactor = false;

    // Grid data that store the x and y of the grid the train is on
    public int          m_nCurrentGridX = 0;
    public int          m_nCurrentGridY = 0;

    // Grid data that store the x and y of the next grid the train should move on
    public int          m_nNextGridX = -1;
    public int          m_nNextGridY = -1;

    // Int that store the direction of the next movement for the train
    public int          m_nNextMove = 1;

    // Vector that store the position to move to
    private Vector3     m_vNextPos;

    // Vector that store the current rotation of the train.
    // Reason: Using the Transform rotation.eulerangle will not give a correct value
    private Vector3     m_vRotation;

    // Train own's animator
    private Animator    m_anim;

    // Grid GameObject
    public GameObject   rGrid;

	// Use this for initialization
	void            Start () 
    {
	    m_anim = GetComponent<Animator>();  
 
        //this.transform.position = new Vector3(0,8.75f,-2.5f);

        m_vRotation = new Vector3(0, 0, 0);
        
        rGrid = GameObject.Find("Grid");
	}
	
	// Update is called once per frame
	void            Update () {
        if (reset)
        {
            // Move train to Grid 0, 0
            this.transform.position = new Vector3(0, 8.75f, -2.5f);
            // Reset rotation
            this.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            m_vRotation = new Vector3(0,0,0);

            m_bMove = false;
            m_nCurrentGridX = 0;
            m_nCurrentGridY = 0;

            m_nNextGridX = 0;
            m_nNextGridY = 0;

            m_bNextTrack = true;
            m_nTrackData = INVALID;

            m_nNextMove = UP;

            reset = false;
        }

        if (m_bMove)
        {
            if (m_nTrackData == VALID && !m_bNextTrack && !m_bWaiting)
            {
                m_anim.Play("Moving");

                steer(); // Steers the train (controls the rotation)
                this.transform.Translate(0, 0, m_fMovespeed); // Move the train forward according to it rotation (Direction)

                // moved out of current grid
                if (!withinGrid())
                {
                    m_bNextTrack = true;
                }
                else if (checkAhead())
                {
                    // check next grid if it is a valid track
                    // Check if it destination and if there is a track
                    if (checkTrack() == USED)
                    {
                        //Debug.Log("Train Ahead on use, train set to wait mode");
                        m_bWaiting = true;
                    }
                    else if (checkTrack() == OUT)
                    {
                        //Debug.Log("No Track Train going into wait mode");
                        m_bWaiting = true;
                    }
                }
                
            }
            else if (m_bWaiting)
            {
                m_anim.Play("Idle");
                // Train will enter this area if it is waiting for the track to clear before it moves
                if (checkAhead())
                {
                    if (checkTrack() == VALID)
                    {
                        m_bWaiting = false;
                    }
                }
            }
            else
            {
                // Train attempted to retreive the next track
                m_nTrackData = getTrack();

                //Debug.Log("At Get new Track:" + m_nTrackData);

                // If track is valid and is not occupied, train will tag that track as occupied and untag current track
                if (m_nTrackData == VALID)
                {
                    // So that once they tag and untag the 2 track it will quit and stop looping through
                    int checkCounter = 0;

                    // This is for tagging the track so that other track cannot access this track.
                    foreach (Transform child in rGrid.transform)
                    {
                        // break for the loop, save calculation.
                        if (checkCounter == 2)
                        {
                            //Debug.Log("Done Tagging");
                            break;
                        }

                        GridData gData = child.GetComponent<GridData>();
                        if (gData.posX == m_nNextGridX && gData.posY == m_nNextGridY)
                        {
                            gData.isOccupied = true;
                            checkCounter++;
                        }
                        else if (gData.posX == m_nCurrentGridX && gData.posY == m_nCurrentGridY)
                        {
                            gData.isOccupied = false;
                            checkCounter++;
                        }
                    }

                    // Change the currentGrid to next grid
                    m_nCurrentGridX = m_nNextGridX;
                    m_nCurrentGridY = m_nNextGridY;

                    getGridPos();
                    m_bNextTrack = false;
                    m_bSnap = true;

                }    
            }

            // Check if it reached the outside of the grid
        }
        else
        {
            m_anim.Play("Idle");
        }
	}

    // Controls the rotation of the train
    void            steer()
    {
        facing = m_vRotation.y;

        if (facing < 0)
        {
            facing = 360 + facing;
        }

        switch (m_nNextMove)
        {
            case UP:
                {
                    if (facing != STEERUP)
                    {
                        if (!m_bTurnFactor)
                        {
                            this.transform.Rotate(new Vector3(0, -m_fTurnspeed, 0));
                            m_vRotation.y -= m_fTurnspeed;

                            //Debug.Log("Rotating Left");
                        }
                        else
                        {
                            this.transform.Rotate(new Vector3(0, m_fTurnspeed, 0));
                            m_vRotation.y += m_fTurnspeed;
                            //Debug.Log("Rotating Right");
                        }
                    }
                    break;
                }
            case DOWN:
                {
                    if (facing != STEERDOWN)
                    {
                        if (!m_bTurnFactor)
                        {
                            this.transform.Rotate(new Vector3(0, -m_fTurnspeed, 0));
                            m_vRotation.y -= m_fTurnspeed;
                            //Debug.Log("Rotating Left");
                        }
                        else
                        {
                            this.transform.Rotate(new Vector3(0, m_fTurnspeed, 0));
                            m_vRotation.y += m_fTurnspeed;
                            //Debug.Log("Rotating Right");
                        }
                    }
                    break;
                }
            case LEFT:
                {
                    if (facing != STEERLEFT)
                    {
                        //Debug.Log(facing);
                        if (!m_bTurnFactor)
                        {
                            this.transform.Rotate(new Vector3(0, -m_fTurnspeed, 0));
                            m_vRotation.y -= m_fTurnspeed;
                            // Debug.Log("Rotating Left");
                        }
                        else
                        {
                            this.transform.Rotate(new Vector3(0, m_fTurnspeed, 0));
                            m_vRotation.y += m_fTurnspeed;
                            //Debug.Log("Rotating Right");
                        }
                    }
                    break;
                }
            case RIGHT:
                {
                    if (facing != STEERRIGHT)
                    {
                        if (!m_bTurnFactor)
                        {
                            this.transform.Rotate(new Vector3(0, -m_fTurnspeed, 0));
                            m_vRotation.y -= m_fTurnspeed;
                            //Debug.Log("Rotating Left");
                        }
                        else
                        {
                            this.transform.Rotate(new Vector3(0, m_fTurnspeed, 0));
                            m_vRotation.y += m_fTurnspeed;
                            //Debug.Log("Rotating Right");
                        }
                    }
                    break;
                }
            default: break;
        }

        if (m_bSnap)
        {
            switch (m_nNextMove)
            {
                case UP:
                    {
                        if (facing != STEERUP)
                        {
                            if (Mathf.Abs(facing - STEERUP) < m_fSnapStrength || 360 - facing < m_fSnapStrength)
                            {
                                this.transform.rotation = Quaternion.Euler(new Vector3(0, STEERUP, 0));
                                m_vRotation.y = STEERUP;
                                //Debug.Log("Snap Up");
                                m_bSnap = false;
                            }
                        }
                        break;
                    }
                case DOWN:
                    {
                        if (facing != STEERDOWN)
                        {
                            if (Mathf.Abs(facing - STEERDOWN) < m_fSnapStrength)
                            {
                                this.transform.rotation = Quaternion.Euler(new Vector3(0, STEERDOWN, 0));
                                m_vRotation.y = STEERDOWN;
                                //Debug.Log("Snap Down");
                                m_bSnap = false;
                            }
                        }
                        break;
                    }
                case LEFT:
                    {
                        if (facing != STEERLEFT)
                        {
                            //Debug.Log(Mathf.Abs(facing) - STEERLEFT);
                            if (Mathf.Abs(facing - STEERLEFT) < m_fSnapStrength)
                            {
                                this.transform.rotation = Quaternion.Euler(new Vector3(0, STEERLEFT, 0));
                                m_vRotation.y = STEERLEFT;
                                //Debug.Log("Snap Left");
                                m_bSnap = false;
                            }
                        }
                        break;
                    }
                case RIGHT:
                    {
                        if (facing != STEERRIGHT)
                        {
                            if (Mathf.Abs(facing - STEERRIGHT) < m_fSnapStrength)
                            {
                                this.transform.rotation = Quaternion.Euler(new Vector3(0, STEERRIGHT, 0));
                                m_vRotation.y = STEERRIGHT;
                                //Debug.Log("Snap Right");
                                m_bSnap = false;
                            }
                        }
                        break;
                    }
                default: break;
            }
        }
    }


    // Check if train is within the checkAhead grid
    bool            checkAhead()
    {
        float checkValue = 2;

        if (m_bInDepot)
        {
            checkValue = 0.1f;
        }

        float left = (m_nCurrentGridX * 10) - checkValue;
        float right = (m_nCurrentGridX * 10) + checkValue;
        float up = (m_nCurrentGridY * 10) + checkValue;
        float down = (m_nCurrentGridY * 10) - checkValue;

        Vector3 trainPos = this.transform.position;

        if (trainPos.x > left && trainPos.x < right && trainPos.z < up && trainPos.z > down)
        {
            return true;
        }
        return false;
    }

    // Check if train is within the track grid
    bool            withinGrid()
    {
        int left    = (m_nCurrentGridX * 10) - 5;
        int right   = (m_nCurrentGridX * 10) + 5;
        int up      = (m_nCurrentGridY * 10) + 5;
        int down    = (m_nCurrentGridY * 10) - 5;

        Vector3 trainPos = this.transform.position;

        if (trainPos.x > left && trainPos.x < right && trainPos.z < up && trainPos.z > down)
        {
            return true;
        }
        return false;
    }

    // Check if there is track ahead of the train
    // NCA: IMPORTANT : Currently the next grid is not update so its hard to check next grid, need rearrange the logic here
    // NCA: Difference between gettrack, this does not set the next move
    int             checkTrack()
    {
        foreach (Transform child in rGrid.transform)
        {
            GridData gData = child.GetComponent<GridData>();
            if (gData.posX == m_nNextGridX && gData.posY == m_nNextGridY)
            {
                if (gData.isTrack && gData.isOccupied)
                {
                    return USED;
                }
                else if (gData.isDepot)
                {
                    Debug.Log("Checking depot");

                    Transform depot = child.FindChild("Depot Created");

                    DepotAI dData = depot.GetComponent<DepotAI>();

                    // If the train found a depot ahead, it will try to ask the depot to open the door
                    // If the door is opened, it will move into the depot and stop.
                    // This set of codes is for handling when the train found the depot ahead.
                    if(!m_bInDepot)
                    {
                        
                        if (m_nNextMove == UP)
                        {
                            if (!dData.isFOpen)
                            {
                                dData.openFDepotDoor();
                                return USED;
                            }
                            else if (dData.isFOpen && !dData.inTransition)
                            {
                                return VALID;                 
                            }
                        }
                        else if (m_nNextMove == DOWN)
                        {
                            if (!dData.isBOpen)
                            {
                                dData.openBDepotDoor();
                                return USED;
                            }
                            else if (dData.isBOpen && !dData.inTransition)
                            {
                                return VALID; 
                            } 
                        }
                    }
                    // This part of the code will handle when the train is in the depot
                    else if (m_bInDepot)
                    {                      
                        if (m_nNextMove == UP)
                        {

                            if (dData.isFOpen)
                            {
                                dData.closeFDepotDoor();
                                return USED;
                            }
                            else if (!dData.isBOpen && !dData.inTransition)
                            {
                                dData.openBDepotDoor();
                                return USED;
                            }
                            else if (dData.isBOpen && !dData.inTransition)
                            {
                                return VALID;
                            }
                        }
                        else if (m_nNextMove == DOWN)
                        {
                            
                            if (dData.isBOpen)
                            {
                                dData.closeBDepotDoor();
                                return USED;
                            }
                            else if (!dData.isFOpen && !dData.inTransition)
                            {
                                dData.openFDepotDoor();
                                return USED;
                            }
                            else if (dData.isFOpen && !dData.inTransition)
                            {
                                return VALID;
                            }
                        }
                    }
                }
                else if (gData.isTrack && !gData.isOccupied)
                {
                    if (gData.TrackType == Track.Vertical)
                    {
                        if (m_nNextMove == UP)
                        { 
                            return VALID;
                        }
                        else if (m_nNextMove == DOWN)
                        {
                            return VALID;
                        }
                        return INVALID;
                    }
                    else if (gData.TrackType == Track.Horizontal)
                    {
                        if (m_nNextMove == LEFT)
                        {
                            return VALID;
                        }
                        else if (m_nNextMove == RIGHT)
                        {
                            return VALID;
                        }
                        return INVALID;
                    }
                    else if (gData.TrackType == Track.UpRight)
                    {
                        if (m_nNextMove == DOWN)
                        {
                            return VALID;
                        }
                        else if (m_nNextMove == LEFT)
                        {
                            return VALID;
                        }
                        return INVALID;
                    }
                    else if (gData.TrackType == Track.UpLeft)
                    {
                        if (m_nNextMove == DOWN)
                        {
                            return VALID;
                        }
                        else if (m_nNextMove == RIGHT)
                        {
                            return VALID;
                        }
                        return INVALID;
                    }
                    else if (gData.TrackType == Track.DownRight)
                    {
                        if (m_nNextMove == LEFT)
                        {
                            return VALID;
                        }
                        else if (m_nNextMove == UP)
                        {
                            return VALID;
                        }
                        return INVALID;
                    }
                    else if (gData.TrackType == Track.DownLeft)
                    {
                        if (m_nNextMove == RIGHT)
                        {
                            return VALID;
                        }
                        else if (m_nNextMove == UP)
                        {
                            return VALID;
                        }
                        return INVALID;
                    }
                }
                return OUT;
            }
        }
        return OUT;
    }

    // Returns VALID,INVALID,OUT in int
    int             getTrack()
    {
        foreach (Transform child in rGrid.transform)
        {
            GridData gData = child.GetComponent<GridData>();
            if (gData.posX == m_nNextGridX && gData.posY == m_nNextGridY)
            {
                // If it is moving into a depot
                if (gData.isDepot)
                {
                    m_bInDepot = true;
                }

                if (gData.isTrack)
                {
                    if (gData.TrackType == Track.Vertical)
                    {
                        // ^
                        // |
                        // |
                        // v
                        // Vertical track therefore 2 ways Up or Down
                        if (m_nNextMove == UP)
                        {
                            // Steer UP         
                            m_nNextMove = UP;
                            return VALID;
                        }
                        else if (m_nNextMove == DOWN)
                        {
                            // Steer DOWN
                            m_nNextMove = DOWN;
                            return VALID;
                        }
                        //Debug.Log("DownUp is invalid");
                        return INVALID;
                    }
                    else if (gData.TrackType == Track.Horizontal)
                    {
                        // < ---------- >
                        // Horizontal Track
                        if (m_nNextMove == LEFT)
                        {
                            // Steer Left
                            m_nNextMove = LEFT;
                            return VALID;
                        }
                        else if (m_nNextMove == RIGHT)
                        {
                            // Steer Right
                            m_nNextMove = RIGHT;
                            return VALID;
                        }
                        //Debug.Log("LeftRight is invalid");
                        return INVALID;
                    }
                    else if (gData.TrackType == Track.UpRight)
                    {
                        // ^
                        // |
                        // - - >
                        if (m_nNextMove == DOWN)
                        {
                            // Going downward so steer right
                            m_nNextMove = RIGHT;
                            m_bTurnFactor = false;
                            return VALID;
                        }
                        else if (m_nNextMove == LEFT)
                        {
                            // Facing Left so steer Up
                            m_nNextMove = UP;
                            m_bTurnFactor = true;
                            return VALID;
                        }
                        //Debug.Log("UpRight is invalid");
                        return INVALID;
                    }
                    else if (gData.TrackType == Track.UpLeft)
                    {
                        //     ^
                        //     |
                        //  <- - 
                        if (m_nNextMove == DOWN)
                        {
                            // Going downward so steer Left
                            m_nNextMove = LEFT;
                            m_bTurnFactor = true;
                            return VALID;
                        }
                        else if (m_nNextMove == RIGHT)
                        {
                            // Facing Right so steer Up
                            m_nNextMove = UP;
                            m_bTurnFactor = false;
                            return VALID;
                        }
                        //Debug.Log("UpLeft is Invalid");
                        return INVALID;
                    }
                    else if (gData.TrackType == Track.DownRight)
                    {
                        // - - >
                        // |
                        // v
                        if (m_nNextMove == LEFT)
                        {
                            // Facing Left so steer Down
                            m_nNextMove = DOWN;
                            m_bTurnFactor = false;
                            return VALID;
                        }
                        else if (m_nNextMove == UP)
                        {
                            // Facing up so steer right
                            m_nNextMove = RIGHT;
                            m_bTurnFactor = true;
                            return VALID;
                        }
                        //Debug.Log("DownRight is invalid");
                        return INVALID;
                    }
                    else if (gData.TrackType == Track.DownLeft)
                    {
                        // <- - 
                        //    |
                        //    v
                        if (m_nNextMove == RIGHT)
                        {
                            // Facing Right so steer Down
                            m_nNextMove = DOWN;
                            m_bTurnFactor = true;
                            return VALID;
                        }
                        else if (m_nNextMove == UP)
                        {
                            // Facing up so steer Left
                            m_nNextMove = LEFT;
                            m_bTurnFactor = false;
                            return VALID;
                        }
                        //Debug.Log("DownLeft is invalid");
                        return INVALID;
                    }
                }
                return INVALID;
            }
        }
        return INVALID;
    }

    // Set the grid based on the train heading
    bool            getGridPos()
    {
        switch (m_nNextMove)
        {
            case UP:
                {
                    m_nNextGridY = m_nCurrentGridY + 1;
                    m_nNextGridX = m_nCurrentGridX;
                    break;
                }
            case DOWN:
                {
                    m_nNextGridY = m_nCurrentGridY - 1;
                    m_nNextGridX = m_nCurrentGridX;
                    break;
                }
            case LEFT:
                {
                    m_nNextGridX = m_nCurrentGridX - 1;
                    m_nNextGridY = m_nCurrentGridY;
                    break;
                }
            case RIGHT:
                {
                    m_nNextGridX = m_nCurrentGridX + 1;
                    m_nNextGridY = m_nCurrentGridY;
                    break;
                }
            default: break;
        }

        return false;
    }

    // Set Train Position
    public void     setTrainPos(int _gridX, int _gridY)
    {
        // Set the train to the correct position offseted by the given value.
        this.transform.position = new Vector3(_gridX * 10, 0, (_gridY * 10) - 1);

        // Set the train Grid Position
        m_nCurrentGridX = _gridX;
        m_nCurrentGridY = _gridY;

        m_nNextGridX = _gridX;
        m_nNextGridY = _gridY;
    }

    // Set the train facing based on an angle given
    public void     setOrientation(float _angle)
    {
        m_vRotation = new Vector3(0, _angle, 0);
        this.transform.Rotate(new Vector3(0, _angle, 0));
    }

    // Button Press
    public void     moveTrain()
    {
        m_bMove = !m_bMove;

        if (checkAhead())
        {
            // Check if it destination and if there is a track
            if (checkTrack() == INVALID)
            {
                Debug.Log("Invalid Track Train Stopping");
                m_bMove = false;
            }
            else if (checkTrack() == OUT)
            {
                Debug.Log("No Track Train Stopping");
                m_bMove = false;
            }
        }
    }

    // Button Press
    public void     resetTrain()
    {
        reset = true;
    }
}
