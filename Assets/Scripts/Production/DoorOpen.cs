using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour {

    public GameController Controller;
    public bool open = false;

    private bool openCached = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // If there is a change in open then open/close the door
        if (open != openCached)
        {
            Controller.openDoor(open);
        }

        // Finally update open
        openCached = open;
	}
}
