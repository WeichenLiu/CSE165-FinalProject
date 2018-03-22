using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformHandle : MonoBehaviour
{
    public GameObject player;
    public Door doorUp;
    public Door doorDown;
    public GameObject camera;
    private GlitchEffect ge;
    private DarkenEffect de;
    private bool grabbed = false;
    private bool fired = false;
    public float speed = 0.05f;
    public float duration = 6.0f;
    public float darkenDuration = 3f;

    private Vector3 pivotPos;
    private int handIndex = -1;

	// Use this for initialization
	void Start ()
	{
	    pivotPos = transform.position;
	    ge = camera.GetComponent<GlitchEffect>();
	    de = camera.GetComponent<DarkenEffect>();
	}

    void OnTriggerStay(Collider c)
    {
        if (c.tag == "Hand" && !grabbed)
        {
            if (c.name == "controller_left")
            {
                handIndex = 0;
            }

            else if (c.name == "controller_right")
            {
                handIndex = 1;
            }

            if ((handIndex == 0 &&
                 OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.11f) ||
                (handIndex == 1 && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) >
                 0.11f))
            {
                grabbed = true;
            }

            if (!grabbed)
            {
                handIndex = -1;
            }
        }
    }

	// Update is called once per frame
	void Update () {

	    if (grabbed)
	    {
	        if (!fired)
	        {
	            StartCoroutine("move", Time.time);
	            ge.enabled = true;
	            float factor = 0.8f;
	            ge.intensity = factor;
	            ge.colorIntensity = factor;
	            ge.flipIntensity = factor;
	            fired = true;
	        }

	        Vector3 delta = this.transform.position - pivotPos;
	        player.transform.position += delta;
	        
	    }
	    pivotPos = transform.position;
    }

    IEnumerator move(float start)
    {
        bool doorClose = false;
        while (Time.time - start < duration)
        {
            if (Time.time - start > duration * 0.5f && !doorClose)
            {
                doorUp.Open(false);
                doorDown.Open(false);
                doorClose = true;
                StartCoroutine("darken", Time.time);
            }
            Vector3 delta = new Vector3(0, 0, -speed);
            transform.position += delta;
            yield return null;
        }
    }

    IEnumerator darken(float start)
    {
        de.enabled = true;

        while (Time.time - start < darkenDuration)
        {
            de.ratio = (darkenDuration - (Time.time - start)) / darkenDuration;
            yield return null;
        }

        de.ratio = 0;
    }
}
