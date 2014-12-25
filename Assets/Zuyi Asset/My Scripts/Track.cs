using UnityEngine;
using System.Collections;

public class Track : MonoBehaviour {

	public enum trackID
	{
		Horizontal,
		Vertical,
		DownRight,
		DownLeft,
		UpRight,
		UpLeft
	}

	public trackID trackType;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
