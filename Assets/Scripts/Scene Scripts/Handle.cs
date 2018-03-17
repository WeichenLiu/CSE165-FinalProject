using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{

    //public Material oMaterial;
    public Color normal;
    public Color hover;
    public Color drag;
    public bool left;
    public bool right;
    public bool dragged;
    private Collider self;

	// Use this for initialization
	void Start ()
	{
	    Material m = Instantiate(GetComponent<Renderer>().material);
	    GetComponent<Renderer>().material = m;
	    left = false;
	    right = false;
	    dragged = false;
	    self = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    bool updateState(Collider c, bool flag = true)
    {
        bool ret = false;
        if (c.name == "controller_left")
        {
            left = flag;
            ret = true;
        }

        if (c.name == "controller_right")
        {
            right = flag;
            ret = true;
        }

        return ret;
    }

    void updateColor()
    {
        if (dragged)
        {
            GetComponent<Renderer>().material.color = drag;
        }
        else if (left || right)
        {
            GetComponent<Renderer>().material.color = hover;
        }
        else
        {
            GetComponent<Renderer>().material.color = normal;
        }
    }

    

    void OnTriggerEnter(Collider c)
    {
        if (updateState(c))
        {
            updateColor();
        }
    }

    void OnTriggerStay(Collider c)
    {
        if (updateState(c))
        {
            MotionController mc = c.GetComponentInParent<MotionController>();
            if (left && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.11f)
            {
                mc.setPivot(0, self);
            }
            else if (right &&
                     OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.11f)
            {
                mc.setPivot(1, self);
            }
            updateColor();
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (updateState(c, false))
        {
            updateColor();
        }
    }
}
