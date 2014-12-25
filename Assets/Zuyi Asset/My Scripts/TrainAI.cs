using UnityEngine;
using System.Collections;

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

// NCA: Train Basic AI
public class TrainAI : MonoBehaviour {
    // Debug use 
    public bool reset = false;

    // Constant For Tracks
    const int INVALID = 0;
    const int VALID = 1;
    const int OUT = 2;

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

    // Bool for the train to trigger the snapping when steering
    public bool m_bSnap = true;
    public float m_fSnapStrength = 5;

    // Bool for the train to check for next track / If it is still on a track.
    public bool m_bNextTrack = true;
    public int m_nTrackData = INVALID;

    // Train Steering and movement speed
    public float m_fMovespeed = 0.1f;
    public float m_fTurnspeed = 2.0f;

    // Grid data that store the x and y of the grid the train is on
    public int m_nCurrentGridX = 0;
    public int m_nCurrentGridY = 0;

    // Grid data that store the x and y of the next grid the train should move on
    public int m_nNextGridX = 0;
    public int m_nNextGridY = 0;

    // Int that store the direction of the next movement for the train
    public int m_nNextMove = 0;

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
	void Start () 
    {
	    m_anim = GetComponent<Animator>();  
 
        this.transform.position = new Vector3(0,8.75f,-2.5f);

        m_vRotation = new Vector3(0, 0, 0);
        
	}
	
	// Update is called once per frame
	void Update () {
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

            reset = false;
        }
        if (m_bMove)
        {
            if (m_nTrackData == VALID && !m_bNextTrack)
            {
                steer(); // Steers the train (controls the rotation)
                this.transform.Translate(0, 0, m_fMovespeed); // Move the train forward according to it rotation (Direction)

                // moved out of current grid
                if (!withinGrid())
                {
                    getGridPos();
                    m_bNextTrack = true;
                    m_nCurrentGridX = m_nNextGridX;
                    m_nCurrentGridY = m_nNextGridY;
                }
                m_anim.Play("Moving");
            }
            else
            {
                m_nTrackData = getTrack();
                m_bNextTrack = false;
                m_bSnap = true;
            }

            // Check if it reached the outside of the grid
        }
        else
        {
            m_anim.Play("Idle");
        }
        //m_anim.Play("Moving");
        //m_anim.Play("Idle");
	}

    // Controls the rotation of the train
    public void steer()
    {
        float facing = m_vRotation.y;

        switch (m_nNextMove)
        {
            case UP:
                {
                    if (facing != STEERUP)
                    { 
                        if (facing <= 90)
                        {
                            this.transform.Rotate(new Vector3(0, -m_fTurnspeed, 0));
                            m_vRotation.y -= m_fTurnspeed;

                            //Debug.Log("Rotating Left");
                        }
                        else if (facing >= 270)
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
                        if (facing > 180)
                        {
                            this.transform.Rotate(new Vector3(0, -m_fTurnspeed, 0));
                            m_vRotation.y -= m_fTurnspeed;
                            //Debug.Log("Rotating Left");
                        }
                        else if (facing < 180)
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
                        if (facing == 0)
                        {
                            m_vRotation.y = 360;
                            //Debug.Log("Set to 360");
                        }
                        if (facing > 270)
                        {
                            this.transform.Rotate(new Vector3(0, -m_fTurnspeed, 0));
                            m_vRotation.y -= m_fTurnspeed;
                           // Debug.Log("Rotating Left");
                        }
                        else if (facing < 270)
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
                        if (facing > 90)
                        {
                            this.transform.Rotate(new Vector3(0, -m_fTurnspeed, 0));
                            m_vRotation.y -= m_fTurnspeed;
                            //Debug.Log("Rotating Left");
                        }
                        else if (facing < 90)
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
            switch(m_nNextMove)
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

    // Check if train is within the track grid
    public bool withinGrid()
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

    public int getTrack()
    {
        float facing = this.transform.rotation.eulerAngles.y;

        foreach (Transform child in rGrid.transform)
        {
            if (child.GetComponent<GridData>().posX == m_nCurrentGridX && child.GetComponent<GridData>().posY == m_nCurrentGridY)
            {
                if (child.tag == "DownUp")
                {
                    // ^
                    // |
                    // |
                    // v
                    // Vertical track therefore 2 ways Up or Down
                    if (facing < 90 || facing > 270)
                    {
                        // Steer UP
                        m_nNextMove = UP;
                        return VALID;
                    }
                    else if(facing > 160 && facing < 200)
                    {
                        // Steer DOWN
                        m_nNextMove = DOWN;
                        return VALID;
                    }
                    Debug.Log("DownUp is invalid");
                    return INVALID;
                }
                else if (child.tag == "LeftRight") 
                {
                    // < ---------- >
                    // Horizontal Track
                    if (facing > 250 && facing < 280)
                    {
                        // Steer Left
                        m_nNextMove = LEFT;
                        return VALID;
                    }
                    else if (facing > 70 && facing < 120)
                    {
                        // Steer Right
                        m_nNextMove = RIGHT;
                        return VALID;
                    }
                    Debug.Log("LeftRight is invalid");
                    return INVALID;
                }
                else if (child.tag == "UpRight") 
                {
                    // ^
                    // |
                    // - - >
                    if (facing > 135 && facing < 225)
                    {
                        // Going downward so steer right
                        m_nNextMove = RIGHT;
                        return VALID;
                    }
                    else if (facing > 225 && facing < 360)
                    {
                        // Facing Left so steer Up
                        m_nNextMove = UP;
                        return VALID;
                    }
                    Debug.Log("UpRight is invalid");
                    return INVALID;
                }
                else if (child.tag == "UpLeft") 
                {
                    //     ^
                    //     |
                    //  <- - 
                    if (facing > 135 && facing < 225)
                    {
                        // Going downward so steer Left
                        m_nNextMove = LEFT;
                        return VALID;
                    }
                    else if (facing > 0 && facing < 225)
                    {
                        // Facing Right so steer Up
                        m_nNextMove = UP;
                        return VALID;
                    }
                    Debug.Log("UpLeft is Invalid");
                    return INVALID;
                }
                else if (child.tag == "DownRight")
                {
                    // - - >
                    // |
                    // v
                    if (facing > 225 && facing < 330)
                    {
                        // Facing Left so steer Down
                        m_nNextMove = DOWN;
                        return VALID;
                    }
                    else if (facing == 0 || facing > 270)
                    {
                        // Facing up so steer right
                        m_nNextMove = RIGHT;
                        return VALID;
                    }
                    Debug.Log("DownRight is invalid");
                    return INVALID;
                }
                else if (child.tag == "DownLeft")
                {
                    // <- - 
                    //    |
                    //    v
                    if (facing > 0 && facing < 225)
                    {
                        // Facing Right so steer Down
                        m_nNextMove = DOWN;
                        return VALID;
                    }
                    else if (facing == 0)
                    {
                        // Facing up so steer Left
                        m_nNextMove = LEFT;
                        return VALID;
                    }
                    Debug.Log("DownLeft is invalid");
                    return INVALID;
                }
            }
        }
        return INVALID;
    }

    // Get the grid pos for next move
    public bool getGridPos()
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

    // Button Press
    public void moveTrain()
    {
        m_bMove = !m_bMove;
    }

    // Button Press
    public void resetTrain()
    {
        reset = true;
    }
}
