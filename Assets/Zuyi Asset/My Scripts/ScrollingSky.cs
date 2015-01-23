using UnityEngine;
using System.Collections;

public class ScrollingSky : MonoBehaviour {

    float scrollSpeed = 0.05f;
    Vector2 scrollVector = new Vector2(1,0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        renderer.material.SetTextureOffset("_MainTex", scrollVector * Time.time * scrollSpeed);
	}
}
