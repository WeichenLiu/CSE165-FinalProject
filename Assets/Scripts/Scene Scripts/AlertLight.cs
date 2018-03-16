using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertLight : MonoBehaviour {

    public float loopInterval = 4;
    public float offset = 0.7f;

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
            float c = perc * (1 - offset) + offset;
            light.color = new Color(1, c, c);
        }
        else
        {
            float perc = (currTime - loopInterval / 2) / (loopInterval / 2);
            float c = (1 - perc) * (1 - offset) + offset;
            light.color = new Color(1, c, c);
        }
	}
}
