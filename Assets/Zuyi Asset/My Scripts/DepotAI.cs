using UnityEngine;
using System.Collections;

// Written by NCA
// This Class controls the animation for the depot when the train enter or leaving.

public class DepotAI : MonoBehaviour {

    public bool isFOpen = false;
    public bool isBOpen = false;
    public bool inTransition = false;

    private Animator m_anim;

	// Use this for initialization
	void Start () {
        m_anim = GetComponent<Animator>();  
	}
	
	// Update is called once per frame
	void Update () {
        if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("FOpened") && isFOpen)
        {
            inTransition = false;
        }
        else if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("FClosed") && !isFOpen)
        {
            inTransition = false;
        }
        else if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("BOpened") && !isBOpen)
        {
            inTransition = false;
        }
        else if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("BClosed") && !isBOpen)
        {
            inTransition = false;
        }

	}

    public void openFDepotDoor()
    {
        if (!isFOpen && !isBOpen && !inTransition)
        {
            m_anim.Play("FOpening");
            isFOpen = true;
            inTransition = true;
        }
        
    }

    public void closeFDepotDoor()
    {
        if (isFOpen && !inTransition)
        { 
            m_anim.Play("FClosing");
            isFOpen = false;
            inTransition = true;
        }
    }

    public void openBDepotDoor()
    {
        if (!isBOpen && !isFOpen && !inTransition)
        {
            m_anim.Play("BOpening");
            isBOpen = true;
            inTransition = true;
        }

    }

    public void closeBDepotDoor()
    {
        if (isBOpen && !inTransition)
        {
            m_anim.Play("BClosing");
            isBOpen = false;
            inTransition = true;
        }
    }
}
