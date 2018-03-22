using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenAfter : MonoBehaviour {

    public GameController Controller;
    public float OpenDelay;
    public float CloseDelay;

    // Use this for initialization
    void Start () {
        StartCoroutine("DelayOpenDoor", Time.time);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator DelayOpenDoor(float start)
    {
        while (Time.time - start < OpenDelay)
        {
            yield return null;
        }
        Controller.openDoor();
        while (Time.time - start < CloseDelay)
        {
            yield return null;
        }
        Controller.openDoor(false);
    }
}
