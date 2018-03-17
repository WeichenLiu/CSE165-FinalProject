using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    //public Material oMaterial;
    public static Color normal = new Color(0.2f, 0.6f, 1f);
    public static Color hover = new Color(0.4f, 0.78f, 1f);
    public static Color drag = new Color(1f, 0.92f, 0.4f);
    public bool left;
    public bool right;
    public bool dragged;
    public Collider self;

    protected Renderer r;

    // Use this for initialization
    void Start()
    {
        r = GetComponent<Renderer>();
        Material m = Instantiate(r.material);
        r.material = m;
        left = false;
        right = false;
        dragged = false;
        self = GetComponent<Collider>();
    }


    protected bool updateState(Collider c, bool flag = true)
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

    protected void updateColor()
    {
        if (dragged)
        {
            r.material.color = drag;
        }
        else if (left || right)
        {
            r.material.color = hover;
        }
        else
        {
            r.material.color = normal;
        }
    }


    protected void OnTriggerEnter(Collider c)
    {
        if (updateState(c))
        {
            updateColor();
        }
    }

    protected virtual void OnTriggerStay(Collider c)
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

    protected void OnTriggerExit(Collider c)
    {
        if (updateState(c, false))
        {
            updateColor();
        }
    }
}