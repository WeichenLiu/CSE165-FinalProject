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
    private bool fired = true;
    protected override void OnTriggerStay(Collider c)
    {
        if (locked)
        {
            base.OnTriggerStay(c);
        }
        else
        {
            if (left && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.11f)
            {
                handIndex = 0;
                hand = c.gameObject;
                dragged = true;
                fired = false;
            }
            else if (right &&
                     OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.11f)
            {
                handIndex = 1;
                hand = c.gameObject;
                dragged = true;
                fired = false;
            }

            if (dragged)
            {
                this.transform.parent = bindedPart.transform;

            }
            updateColor();
        }
    }

    void Update()
    {

        if (!locked)
        {
            if (((handIndex == 0 &&
                  OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.11f)
                 || (handIndex == 1 &&
                     OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.11f)))
            {
                if (dragged)
                {
                    velocity = transform.position - hand.transform.position;

                    bindedPart.velocity = new Vector3(0f, 0f, 0f);
                    //player.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
                    //player.GetComponent<Rigidbody>().ResetInertiaTensor();
                    bindedPart.MovePosition(bindedPart.transform.position + velocity);
                    

                }
            }
        }
        else
        {
            dragged = false;
            if (!fired)
            {
                //velocity = transform.position - lastCoord;
                if ((velocity / Time.deltaTime).magnitude >= 0.001f)
                {
                    bindedPart.AddForce(velocity / Time.deltaTime, ForceMode.VelocityChange);
                }
                handIndex = -1;
                fired = true;
            }
        }
    }
    
}
