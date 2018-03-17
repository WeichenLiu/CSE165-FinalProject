using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardDebug : MonoBehaviour
{

    public Door down;
    public Door up;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown("space"))
	    {
            Debug.Log("Door toggle");
	        up.Open();
	        down.Open();
	    }
	}
}
