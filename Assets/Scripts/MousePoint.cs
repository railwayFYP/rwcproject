using UnityEngine;
using System.Collections;

public class MousePoint : MonoBehaviour 
{
	RaycastHit hit;
	public string currentItemSelected;
  
	public bool currentlyBuilding;

	private float raycastLength = 500;

	//placement plane items
	private GameObject lastHitObj;

	void Update ()
	{
		GameObject Target = GameObject.Find (currentItemSelected);

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

        if (checkWithinBounds(Input.mousePosition.x, Input.mousePosition.y))
        {

		    if (Physics.Raycast (ray, out hit, raycastLength))
		    {
                //if building mode is not enabled
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

                            if      (lastHitObj.tag == "DownUp"     )       currentItemSelected = "Straight Track"  ; 
                            else if (lastHitObj.tag == "LeftRight"  )       currentItemSelected = "Vert Track"      ;
                            else if (lastHitObj.tag == "UpRight"    )       currentItemSelected = "UpRight"         ;
                            else if (lastHitObj.tag == "UpLeft"     )       currentItemSelected = "UpLeft"          ;
                            else if (lastHitObj.tag == "DownRight"  )       currentItemSelected = "DownRight"       ;
                            else if (lastHitObj.tag == "DownLeft"   )       currentItemSelected = "DownLeft"        ;

                            currentlyBuilding = true;
                            lastHitObj.tag = "Open";
                        }
                   
                    }
                }

			    //if building mode is enabled
                else if (hit.collider.name == "PlacementPlane(Clone)" && currentlyBuilding)
			    {
				    //position of Target (current track selected) follows the position of hit.point (mouse cursor)
				    Target.transform.position = hit.point;

				    //lastHitObj = the grid which cursor is in
				    lastHitObj = hit.collider.gameObject;

				    //0 = left, 1 = right, 2 = middle mouse button
				    if(Input.GetMouseButtonDown(0))
				    {
					    if(lastHitObj.tag == "Open")
					    {
                            //create Target (current track selected) at  lastHitObj.transform.position (center of the grid which cursor is in)
                            GameObject TargetObj = Instantiate(Target, lastHitObj.transform.position, Quaternion.identity) as GameObject;
                        
                            if      (currentItemSelected == "Straight Track")   { lastHitObj.tag = "DownUp";    }
                            else if (currentItemSelected == "Vert Track")       { lastHitObj.tag = "LeftRight"; }
                            else if (currentItemSelected == "UpRight")          { lastHitObj.tag = "UpRight";   }
                            else if (currentItemSelected == "UpLeft")           { lastHitObj.tag = "UpLeft";    }
                            else if (currentItemSelected == "DownRight")        { lastHitObj.tag = "DownRight"; }
                            else if (currentItemSelected == "DownLeft")         { lastHitObj.tag = "DownLeft";  }
                        
                            TargetObj.name = "Track Created";

                            //creates the track as the child of the grid
                            TargetObj.transform.parent = lastHitObj.transform;

                            //temporarily hide the track that is following the cursor by changing its position to be the same as the new track created
                            Target.transform.position = lastHitObj.transform.position;
                            currentItemSelected = null;
                            currentlyBuilding = false;
					    }
				    }

				    //right click or press esc to cancel building
                    if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
				    {
					    //temporarily hide the track that is following the cursor by moving it up above the camera
					    Target.transform.position = transform.up * 100;
					    currentItemSelected = null;
					    currentlyBuilding = false;
				    }
			    }


		    }
        }
		Debug.DrawRay (ray.origin, ray.direction * raycastLength, Color.yellow);
	}

    bool checkWithinBounds(float mouseX, float mouseY)
    {
        // NCA: Check within screen
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

    public void DownUpPressed()
    {
        if (currentItemSelected != "Straight Track")
        {
            hidePrevious();
        }
        currentItemSelected = "Straight Track";
        currentlyBuilding = true;
    }

    public void LeftRightPressed()
    {
        if (currentItemSelected != "Vert Track")
        {
            hidePrevious();
        }
        currentItemSelected = "Vert Track";
        currentlyBuilding = true;
    }

    public void UpLeftPressed()
    {
        if (currentItemSelected != "UpLeft")
        {
            hidePrevious();
        }
        currentItemSelected = "UpLeft";
        currentlyBuilding = true;
    }

    public void UpRightPressed()
    {
        if (currentItemSelected != "UpRight")
        {
            hidePrevious();
        }
        currentItemSelected = "UpRight";
        currentlyBuilding = true;
    }

    public void DownLeftPressed()
    {
        if (currentItemSelected != "DownLeft")
        {
            hidePrevious();
        }
        currentItemSelected = "DownLeft";
        currentlyBuilding = true;
    }

    public void DownRightPressed()
    {
        if (currentItemSelected != "DownRight")
        {
            hidePrevious();
        }
        currentItemSelected = "DownRight";
        currentlyBuilding = true;
    }

}
