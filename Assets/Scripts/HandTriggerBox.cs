using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTriggerBox : MonoBehaviour
{

    public GameObject parent;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    this.transform.position = parent.transform.position;
	    this.transform.rotation = parent.transform.rotation;
	}
}
