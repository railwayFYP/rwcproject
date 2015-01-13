using UnityEngine;
using System.Collections;

public class GenerateGrid : MonoBehaviour {

    public bool autoCreate = false;

	// Use this for initialization
    public GameObject plain;
    public int plainX;
    public int plainY;

    void Start()
    {  

    }
	
	// Update is called once per frame
	void Update () {
        if (autoCreate)
        {
            createGrid(plainX,plainY);
            autoCreate = false;
        }
	}

    public void createGrid(int SizeX, int SizeY)
    {
        Transform gparent = GameObject.Find("Grid").transform;
        GameObject temp;

        for (int y = 0; y < SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
            {
                temp = Instantiate(plain, new Vector3(x * 10, 0, y * 10), Quaternion.identity) as GameObject;

                // Get the GridData component and set the X and Y of the grid for train access
                temp.gameObject.GetComponent<GridData>().posX = x;
                temp.gameObject.GetComponent<GridData>().posY = y;

                // Parent to Grid
                temp.transform.parent = gparent;
            }
        }
    }
}
