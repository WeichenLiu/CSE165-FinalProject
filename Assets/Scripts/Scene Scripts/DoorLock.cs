using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour {

    public GameObject Card;
    public float ActivateDistance;
    public GameController Controller;
    public float ActivateTime;

    private bool activated;
    private bool hovering;

	// Use this for initialization
	void Start () {
		
	}

    float GetDistance()
    {
        return (transform.position - Card.transform.position).magnitude;
    }
	
	// Update is called once per frame
	void Update () {
        if (!hovering && GetDistance() < ActivateDistance)
        {
            hovering = true;
            if (!activated)
            {
                
            }
        }
        else if (hovering && GetDistance() >= ActivateDistance)
        {
            hovering = false;
        }
	}

    public void SwitchOn()
    {
        
    }

    public void Activate()
    {
        activated = true;
        // Controller.openDoor();
        StartCoroutine("ActivateUI", Time.time);
    }

    IEnumerator ActivateUI(float startTime)
    {
        while (Time.time - startTime < ActivateTime)
        {
            yield return null;
        }
        SwitchOn();
    }
}
