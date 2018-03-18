using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulatableHandle : Handle
{

    public bool locked = true;
    public Rigidbody bindedPart;
    public GameObject hand;
    public int handIndex = -1;
    private Vector3 velocity;
    private Vector3 pivotRot;
    private bool fired = true;
    private bool triggered = false;
    protected override void OnTriggerStay(Collider c)
    {
        if (!triggered)
        {
            base.OnTriggerStay(c);
            if (left && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.11f)
            {
                handIndex = 0;
                hand = c.gameObject;
                dragged = true;
                fired = false;
                triggered = true;
            }
            else if (right &&
                     OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.11f)
            {
                handIndex = 1;
                hand = c.gameObject;
                dragged = true;
                fired = false;
                triggered = true;
            }

            if (handIndex > -1)
            {
                pivotRot = hand.transform.localRotation.eulerAngles;
            }
        }
    }

    void Update()
    {
        
        if (triggered)
        {
            if ((left && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.11f) ||
                ((right &&
                  OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.11f)))
            {
                if (locked)
                {
                    Debug.Log(hand.transform.localRotation.eulerAngles);
                    if (pivotRot.z < 50)
                    {
                        if (hand.transform.localRotation.eulerAngles.z > 300)
                        {
                            pivotRot.z = 360 + pivotRot.z;
                        }
                    }
                    else if(pivotRot.z > 300)
                    {
                        if (hand.transform.localRotation.eulerAngles.z < 50)
                        {
                            pivotRot.z = pivotRot.z - 360;
                        }
                    }
                    Vector3 deltaRot = hand.transform.localRotation.eulerAngles - pivotRot;
                    pivotRot = hand.transform.localRotation.eulerAngles;
                    Vector3 old = transform.rotation.eulerAngles;
                    old.x -= deltaRot.z;
                    if (old.x < 269)

                    {
                        old.x = 269;
                    }

                    if (old.x > 358)
                    {
                        old.x = 358;
                    }

                    if (old.x > 0)
                    {
                        //locked = false;
                    }

                    transform.rotation = Quaternion.Euler(old);
                }
            }
            else
            {
                triggered = false;
            }
        }

    }
    
}
