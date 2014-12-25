using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour 
{
	public GameObject plane;
	public int width = 100;
	public int height = 100;

	private GameObject [,] grid = new GameObject[100,100]; 

	void Awake ()
	{
		for (int x = 0; x < width; x++)
		{
			for (int z = 0; z < height; z++)
			{
				GameObject gridPlane = (GameObject)Instantiate(plane); //force it to be of type GameObject
				gridPlane.transform.position = new Vector3 (gridPlane.transform.position.x + x, 
				                                            gridPlane.transform.position.y,
				                                            gridPlane.transform.position.z + z);
				grid[x,z] = gridPlane;
			}
		}
	}

	void OnGUI ()
	{
		if (GUI.Button (new Rect (10, 10, 150, 100), "Delete Grid [3,3]"))
			Destroy (grid [3, 3]);
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
