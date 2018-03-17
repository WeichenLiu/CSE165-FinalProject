using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardDebug : MonoBehaviour
{

    public Cable down;
    public Cable up;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown("space"))
	    {
            Debug.Log("Door toggle");
	        up.activate();
	        down.activate();
	    }
	}
}
