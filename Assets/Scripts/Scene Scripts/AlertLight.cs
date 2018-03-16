using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertLight : MonoBehaviour {

    public float loopInterval = 2;

    private Light light;
    private float startTime;
    private float currTime;

	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        currTime = (Time.time - startTime) % loopInterval;
        if (currTime < loopInterval / 2)
        {
            float perc = currTime / (loopInterval / 2);
            light.color = new Color(1, perc, perc);
        }
        else
        {
            float perc = (currTime - loopInterval / 2) / (loopInterval / 2);
            light.color = new Color(1, 1 - perc, 1 - perc);
        }
	}
}
