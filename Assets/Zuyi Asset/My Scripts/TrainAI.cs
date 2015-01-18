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
public class TrainAI : MonoBehaviour
{
    // Debug use 
    public bool reset = false;
    public float facing;
    // Constant For Tracks
    const int INVALID = 0;
    const int VALID = 1;
    const int OUT = 2;
    const int USED = 3;
    const int DEPOT = 4;

    // Constant Direction for the train movement
    const int UP = 1;
    const int RIGHT = 2;
    const int DOWN = 3;
    const int LEFT = 4;

    // Const angle for the train to Steer toward
    const float STEERUP = 0;
    const float STEERLEFT = 270;
    const float STEERRIGHT = 90;
    const float STEERDOWN = 180;

    // Bool for the train to move
    public bool m_bMove = false;
    public bool m_bWaiting = false;
    public bool m_bInDepot = false;
    public bool m_bGettingOut = false;

    // Reference for depot the train is in
    private DepotAI r_Depot;

    // Bool for the train to trigger the snapping when steering
    public bool m_bSnap = true;
    public float m_fSnapStrength = 5;

    // Bool for the train to check for next track / If it is still on a track.
    public bool m_bNextTrack = true;
    public bool m_bStopOnTrack = false;
    public int m_nTrackData = INVALID;

    // Train Steering and movement speed
    public float m_fMovespeed = 0.1f;
    public float m_fTurnspeed = 2.0f;

    // Beta Feature
    // False will make it minus from rotation and True will make it increase from rotation
    public bool m_bTurnFactor = false;

    // Grid data that store the x and y of the grid the train is on
    public int m_nCurrentGridX = 0;
    public int m_nCurrentGridY = 0;

    // Grid data that store the x and y of the next grid the train should move on
    public int m_nNextGridX = -1;
    public int m_nNextGridY = -1;

    // Int that store the direction of the next movement for the train
    public int m_nNextMove = 1;
    public int m_nPrevMove = 1;

    // Vector that store the position to move to
    private Vector3 m_vNextPos;

    // Vector that store the current rotation of the train.
    // Reason: Using the Transform rotation.eulerangle will not give a correct value
    private Vector3 m_vRotation;

    // Train own's animator
    private Animator m_anim;

    // Grid GameObject
    public GameObject rGrid;

    // Use this for initialization
    void Start()
    {
        m_anim = GetComponent<Animator>();

        //this.transform.position = new Vector3(0,8.75f,-2.5f);

        m_vRotation = new Vector3(0, 0, 0);

        rGrid = GameObject.Find("Grid");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bMove)
        {
            if (m_nTrackData == VALID && !m_bNextTrack && !m_bWaiting)
            {
                m_anim.Play("Moving");

                steer(); // Steers the train (controls the rotation)
                this.transform.Translate(0, 0, m_fMovespeed); // Move the train forward according to it rotation (Direction)

                //checkAheadBeta();
                // moved out of current grid
                if (!withinGrid())
                {
                    m_bNextTrack = true;
                }
                else if (checkAhead())
                {
                    // check next grid if it is a valid track
                    // Check if it destination and if there is a track
                    if (checkTrack() != VALID)
                    {
                        //Debug.Log("Train Ahead on use, train set to wait mode");
                        GameObject.Find("Controllers").GetComponent<MissionControl>().setLose();
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
        }
        else
        {
            m_anim.Play("Idle");
        }
    }

    // Controls the rotation of the train
    void steer()
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
                        }
                        else
                        {
                            this.transform.Rotate(new Vector3(0, m_fTurnspeed, 0));
                            m_vRotation.y += m_fTurnspeed;
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
                        }
                        else
                        {
                            this.transform.Rotate(new Vector3(0, m_fTurnspeed, 0));
                            m_vRotation.y += m_fTurnspeed;
                        }
                    }
                    break;
                }
            case LEFT:
                {
                    if (facing != STEERLEFT)
                    {
                        if (!m_bTurnFactor)
                        {
                            this.transform.Rotate(new Vector3(0, -m_fTurnspeed, 0));
                            m_vRotation.y -= m_fTurnspeed;
                        }
                        else
                        {
                            this.transform.Rotate(new Vector3(0, m_fTurnspeed, 0));
                            m_vRotation.y += m_fTurnspeed;
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
                        }
                        else
                        {
                            this.transform.Rotate(new Vector3(0, m_fTurnspeed, 0));
                            m_vRotation.y += m_fTurnspeed;
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
                                m_bSnap = false;
                            }
                        }
                        break;
                    }
                case LEFT:
                    {
                        if (facing != STEERLEFT)
                        {
                            if (Mathf.Abs(facing - STEERLEFT) < m_fSnapStrength)
                            {
                                this.transform.rotation = Quaternion.Euler(new Vector3(0, STEERLEFT, 0));
                                m_vRotation.y = STEERLEFT;
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
                                m_bSnap = false;
                            }
                        }
                        break;
                    }
                default: break;
            }
        }
    }

    // Check Ahead  uses Line checking than box tracking.
    bool checkAhead()
    {
        //Debug.Log("Beta test called");
        float checkValue = 0;

        float left = (m_nCurrentGridX * 10) - checkValue;
        float right = (m_nCurrentGridX * 10) + checkValue;
        float up = (m_nCurrentGridY * 10) + checkValue;
        float down = (m_nCurrentGridY * 10) - checkValue;

        Vector3 trainPos = this.transform.position;

        // Offset the train Z position as it starts negative
        //trainPos.z += 10;

        switch (m_nNextMove)
        {
            case UP:
                {
                    //Debug.Log("Z = " + trainPos.z);
                    //Debug.Log("Up = " + up);
                    if (m_nPrevMove == LEFT || m_nPrevMove == RIGHT)
                    {
                        up += 1;
                        if (trainPos.z > up && trainPos.z < up + 2)
                        {
                            //Debug.Log("Check top");
                            return true;
                        }
                    }
                    else if (trainPos.z > up && trainPos.z < up + 2)
                    {
                        //Debug.Log("Check top");
                        return true;
                    }
                    break;
                }

            case DOWN:
                {
                    if (m_nPrevMove == LEFT || m_nPrevMove == RIGHT)
                    {
                        down -= 1;
                        if (trainPos.z < down && trainPos.z > down - 2)
                        {
                            //Debug.Log("Check Bot");
                            return true;
                        }
                    }
                    if (trainPos.z < down && trainPos.z > down - 2)
                    {                       
                        //Debug.Log("Check Bot");
                        return true;
                    }
                    break;
                }
            case LEFT:
                {
                    if (m_nPrevMove == UP || m_nPrevMove == DOWN)
                    {
                        if (trainPos.x < left && trainPos.x < left - 2)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (trainPos.x < left && trainPos.x > left - 2)
                        {
                            return true;
                        }
                    }
                    break;
                }
            case RIGHT:
                {
                    if (m_nPrevMove == UP || m_nPrevMove == DOWN)
                    {
                        if (trainPos.x > right + 2 && trainPos.x < right + 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (trainPos.x > right && trainPos.x < right + 2)
                        {
                            return true;
                        }
                    }
                    break;
                }

            default:
                break;
        }
        return false;
    }

    // Check if train is within the track grid TBC
    bool withinGrid()
    {
        int left = (m_nCurrentGridX * 10) - 5;
        int right = (m_nCurrentGridX * 10) + 5;
        int up = (m_nCurrentGridY * 10) + 5;
        int down = (m_nCurrentGridY * 10) - 5;

        Vector3 trainPos = this.transform.position;

        if (trainPos.x > left && trainPos.x < right && trainPos.z < up && trainPos.z > down)
        {
            return true;
        }
        return false;
    }

    int requestFDoorOpen()
    {
        // This function handle the opening of the front door. (Door facing the camera in game view)\
        // If train is in depot it will check if the door is open before it leave works the same as back
        if (m_bInDepot)
        {
            if (!r_Depot.isFOpen)
            {
                r_Depot.openFDepotDoor();
                return USED;
            }
            else if (r_Depot.isFOpen && !r_Depot.inTransition)
            {
                return VALID;
            }
        }
        else
        {
            if (m_nNextMove == UP)
            {
                if (!r_Depot.isFOpen && !r_Depot.inTransition)
                {
                    // check if the door is opened. if it is not, request it to open and set the train to wait
                    // it will not be able to request if the door is another in transition mode
                    r_Depot.openFDepotDoor();
                    return USED;
                }
                else if (r_Depot.isFOpen && !r_Depot.inTransition)
                {
                    // if it is open and not in transition. 
                    return VALID;
                }
            }
        }
        return INVALID;
    }

    int requestBDoorOpen()
    {
        // Before it can go out the door must be opened
        if (m_bInDepot)
        {
            if (!r_Depot.isBOpen)
            {
                r_Depot.openBDepotDoor();
                return USED;
            }
            else if (r_Depot.isBOpen && !r_Depot.inTransition)
            {
                return VALID;
            }
        }
        else
        {
            if (m_nNextMove == DOWN)
            {
                if (!r_Depot.isBOpen && !r_Depot.inTransition)
                {
                    // check if the door is opened. if it is not, request it to open and set the train to wait
                    // it will not be able to request if the door is another in transition mode
                    r_Depot.openBDepotDoor();
                    return USED;
                }
                else if (r_Depot.isBOpen && !r_Depot.inTransition)
                {
                    // if it is open and not in transition. 
                    return VALID;
                }
            }           
        }
        return INVALID;
    }

    void requestCloseDoors()
    {
        if (m_bInDepot)
        {
            // Based on the prev move of the train. it will request the door that should be closed
            if (m_nPrevMove == UP)
            {
                // Up means F doors need to be closed
                if (r_Depot.isFOpen)
                {
                    r_Depot.closeFDepotDoor();
                }      
            }
            else if (m_nPrevMove == DOWN)
            {
                // Down means B doors need to be closed
                if (r_Depot.isBOpen)
                {
                    r_Depot.closeBDepotDoor();
                }
            }
        }
        else if (r_Depot != null)
        {
            if (m_nPrevMove == DOWN)
            {
                // Up means F doors need to be closed
                if (r_Depot.isFOpen)
                {
                    r_Depot.closeFDepotDoor();
                }
            }
            else if (m_nPrevMove == UP)
            {
                // Down means B doors need to be closed
                if (r_Depot.isBOpen)
                {
                    r_Depot.closeBDepotDoor();
                }
            }
            r_Depot = null;
        }
    }

    // Check if there is track ahead of the train
    // NCA: IMPORTANT : Currently the next grid is not update so its hard to check next grid, need rearrange the logic here
    // NCA: Difference between gettrack, this does not set the next move
    int checkTrack()
    {
        int result = INVALID;
        bool depotAgain = false;
        requestCloseDoors();

        // When it is not in the depot
        foreach (Transform child in rGrid.transform)
        {
            GridData gData = child.GetComponent<GridData>();

            if (gData.posX == m_nCurrentGridX && gData.posY == m_nCurrentGridY && gData.isDestination)
            {
                GameObject.Find("Controllers").GetComponent<MissionControl>().setWin();
            }
            if (gData.posX == m_nNextGridX && gData.posY == m_nNextGridY)
            {
                // Checks that there is a track ahead but it is 
                if (gData.isTrack && gData.isOccupied)
                {
                    return USED;
                }
                else if (gData.isDepot)
                {
                    // Only controls the entry of the train. Exit will be called on another part of this
                    // Train is 1 track away from depot. 
                    // Returns used if door of depot is not opened based on what direction the train is facing
                    // returns valid if the door is opened.
                    Transform depot = child.FindChild("Depot Created");

                    DepotAI dData = depot.GetComponent<DepotAI>();

                    r_Depot = dData;

                    // Hack fix for chain depots
                    if (m_bInDepot)
                    {
                        depotAgain = true;
                        result = VALID;
                        break;
                    }

                    result = requestBDoorOpen();

                    if (result != INVALID)
                    {
                        break;
                    }

                    result = requestFDoorOpen();

                    if (result != INVALID)
                    {
                        break;
                    }
                }
                else if (gData.isTrack && !gData.isOccupied)
                {
                    if (gData.TrackType == Track.Vertical)
                    {
                        if (m_nNextMove == UP)
                        {
                            result = VALID;
                        }
                        else if (m_nNextMove == DOWN)
                        {
                            result = VALID;
                        }
                    }
                    else if (gData.TrackType == Track.Horizontal)
                    {
                        if (m_nNextMove == LEFT)
                        {
                            result = VALID;
                        }
                        else if (m_nNextMove == RIGHT)
                        {
                            result = VALID;
                        }
                    }
                    else if (gData.TrackType == Track.UpRight)
                    {
                        if (m_nNextMove == DOWN)
                        {
                            result = VALID;
                        }
                        else if (m_nNextMove == LEFT)
                        {
                            result = VALID;
                        }                       
                    }
                    else if (gData.TrackType == Track.UpLeft)
                    {
                        if (m_nNextMove == DOWN)
                        {
                            result = VALID;
                        }
                        else if (m_nNextMove == RIGHT)
                        {
                            result = VALID;
                        }
                    }
                    else if (gData.TrackType == Track.DownRight)
                    {
                        if (m_nNextMove == LEFT)
                        {
                            result = VALID;
                        }
                        else if (m_nNextMove == UP)
                        {
                            result = VALID;
                        }
                    }
                    else if (gData.TrackType == Track.DownLeft)
                    {
                        if (m_nNextMove == RIGHT)
                        {
                            result = VALID;
                        }
                        else if (m_nNextMove == UP)
                        {
                            result = VALID;
                        }
                    }
                }
                break;
            }
        }

        if (result == VALID && m_bInDepot && !depotAgain)
        {
            // Check if the door is opened
            if (m_nPrevMove == UP)
            {
                result = requestBDoorOpen();
            }
            else if (m_nPrevMove == DOWN)
            {
                result = requestFDoorOpen();
            }
        }
        return result;
    }

    // Returns VALID,INVALID,OUT in int
    int getTrack()
    {
        foreach (Transform child in rGrid.transform)
        {
            GridData gData = child.GetComponent<GridData>();
            if (gData.posX == m_nNextGridX && gData.posY == m_nNextGridY)
            {
                // set it to be in depot once its moving into it.
                if (gData.isDepot)
                {
                    m_bInDepot = true;
                }
                else
                {
                    m_bInDepot = false;
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
                            m_nPrevMove = m_nNextMove;    
                            m_nNextMove = UP;
                            return VALID;
                        }
                        else if (m_nNextMove == DOWN)
                        {
                            // Steer DOWN
                            m_nPrevMove = m_nNextMove;
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
                            m_nPrevMove = m_nNextMove;
                            m_nNextMove = LEFT;
                            return VALID;
                        }
                        else if (m_nNextMove == RIGHT)
                        {
                            // Steer Right
                            m_nPrevMove = m_nNextMove;
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
                            m_nPrevMove = m_nNextMove;
                            m_nNextMove = RIGHT;
                            m_bTurnFactor = false;
                            return VALID;
                        }
                        else if (m_nNextMove == LEFT)
                        {
                            // Facing Left so steer Up
                            m_nPrevMove = m_nNextMove;
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
                            m_nPrevMove = m_nNextMove;
                            m_nNextMove = LEFT;
                            m_bTurnFactor = true;
                            return VALID;
                        }
                        else if (m_nNextMove == RIGHT)
                        {
                            // Facing Right so steer Up
                            m_nPrevMove = m_nNextMove;
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
                            m_nPrevMove = m_nNextMove;
                            m_nNextMove = DOWN;
                            m_bTurnFactor = false;
                            return VALID;
                        }
                        else if (m_nNextMove == UP)
                        {
                            // Facing up so steer right
                            m_nPrevMove = m_nNextMove;
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
                            m_nPrevMove = m_nNextMove;
                            m_nNextMove = DOWN;
                            m_bTurnFactor = true;
                            return VALID;
                        }
                        else if (m_nNextMove == UP)
                        {
                            // Facing up so steer Left
                            m_nPrevMove = m_nNextMove;
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
    void getGridPos()
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
    }

    // Set Train Position
    public void setTrainPos(int _gridX, int _gridY)
    {
        float offset = 1;

        if (m_bInDepot)
        {
            offset = 0.05f;
        }
        // Set the train to the correct position offseted by the given value.
        this.transform.position = new Vector3(_gridX * 10, 0, (_gridY * 10) - offset);

        // Set the train Grid Position
        m_nCurrentGridX = _gridX;
        m_nCurrentGridY = _gridY;

        m_nNextGridX = _gridX;
        m_nNextGridY = _gridY;
    }

    public void setDepotRef(DepotAI _depot)
    {
        r_Depot = _depot;
    }

    // Set the train facing based on an angle given
    public void setOrientation(float _angle)
    {
        m_vRotation = new Vector3(0, _angle, 0);
        this.transform.Rotate(new Vector3(0, _angle, 0));
    }

    // Button Press
    public void moveTrain()
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
    public void resetTrain()
    {
        reset = true;
    }
}
