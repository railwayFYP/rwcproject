using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour 
{

	public  float       fPosX               = 0;            // X Distance
	public  float       fPosY               = 40;           // Y Distance
	public  float       fPosZ               = 0;          // Z Distance
	public  float       fScrollDistance     = 20;           // How far the distance from the edges before the camera scrolls
	public  float       fScrollSpeed        = 1;
	public  bool        bUpdate             = false;         // Boolean to set if to update the camera movement or not

    // Camera Boundaries
    public float top = 0;
    public float bot = 0;
    public float left = 0;
    public float right = 0;

    public  bool        bScrollHori         = false;        // Boolean for Scrolling true = scroll false = no scroll
    public  bool        bScrollVert         = false;        // Boolean for Scrolling true = scroll false = no scroll
    public  bool        bScrollUp           = false;        // Boolean for directional true = up false = down
    public  bool        bScrollLeft         = false;        // Boolean for directional true = left false = right

	private Transform   attachedCamera;                     // For storing Camera Transform

	// UNITY: Init Function
	void Start () 
    {
		attachedCamera = camera.transform;                  // Store the camera transform
		attachedCamera.position = new Vector3(fPosX,fPosY,fPosZ);
	}
	
	// UNITY: Main Update Function runs every frames
	void Update () 
    {
		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;

		// Update camera Position
		if (bUpdate) 
        {
			// Check is it near the boundaries
            if (checkWithinBounds(mouseX, mouseY)) 
            {
                if (bScrollVert)
                {
                    if (bScrollUp)
                    {
                        // Scrolling down
                        if (attachedCamera.position.z > bot)
                        {
                            attachedCamera.Translate(0, 0, -fScrollSpeed, Space.World);
                        }             
                    }
                    else
                    {
                        // Scrolling up
                        if (attachedCamera.position.z < top)
                        {
                            attachedCamera.Translate(0, 0, fScrollSpeed, Space.World);
                        }
                    }
                }

                if (bScrollHori)
                {
                    if (bScrollLeft)
                    {
                        if (attachedCamera.position.x > left)
                        {
                            attachedCamera.Translate(-fScrollSpeed, 0, 0, Space.World);
                        }
                    }
                    else
                    {
                        if (attachedCamera.position.x < right)
                        {
                            attachedCamera.Translate(fScrollSpeed, 0, 0, Space.World);
                        }
                    }
                }
            }
		}
	}

    // NCA: Function to check the mouse position. And set the flags for the scrolling
	bool checkWithinBounds(float mouseX, float mouseY) 
    {
        bScrollHori = false;
        bScrollVert = false;

        bool bScroll = false;

        // NCA: Check within screen
        if (mouseX < 0 || mouseX > Screen.width || mouseY < 0 || mouseY > Screen.height)
        {
            return false;
        }

        // NCA: Checks for Horizontal Movements
        if (mouseX < fScrollDistance) 
        {
            // Scroll Left
            bScrollLeft = true;
            bScrollHori = true;
            bScroll = true;
        }
        else if (mouseX > Screen.width - fScrollDistance) 
        {
            // Scroll Right
            bScrollLeft = false;
            bScrollHori = true;
            bScroll = true;
        }

        // NCA: Checks for Vertical Movements
        if (mouseY < fScrollDistance) 
        {
            // Scroll Up
            bScrollUp = true;
            bScrollVert = true;
            bScroll = true;
        }
        else if (mouseY >= Screen.height - fScrollDistance)
        {
            // Scroll Down
            bScrollUp = false;
            bScrollVert = true;
            bScroll = true;
        }

        return bScroll;
	}

    public void setCameraBound(float _top, float _bot, float _left, float _right)
    {
        top = _top;
        bot = _bot;
        left = _left;
        right = _right;
    }

    public void toggleCameraUpdate()
    {
        bUpdate = !bUpdate;
    }
}
