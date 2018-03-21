using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulatableHandle : Handle
{

    public AudioSource coverAudio;
    public HandCollisionController player;
    public float lifeTime = 15.0f;
    public bool locked = true;
    public GameObject bindedPart;
    public GameObject hand;
    public int handIndex = -1;
    private Vector3 velocity;
    private Vector3 pivotRot;
    private bool fired = true;
    private bool triggered = false;
    private float startTime = 0;

    protected override void Start()
    {
        base.Start();
        coverAudio = GetComponent<AudioSource>();
    }

    protected override void OnTriggerStay(Collider c)
    {
        if (locked)
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
    }

    

    void Update()
    {
        if (!locked) 
        {
            if (!fired)
            {
                coverAudio.Play();
                BoxCollider bc = bindedPart.GetComponent<BoxCollider>();
                player.disableCollision(bc);
                BoxCollider[] bcs = bindedPart.GetComponentsInChildren<BoxCollider>();
                for (int i = 0; i < bcs.Length; i++)
                {
                    player.disableCollision(bcs[i]);
                }

                bindedPart.transform.parent = transform;
                Rigidbody r = GetComponent<Rigidbody>();
                r.isKinematic = false;
                r.AddForceAtPosition(new Vector3(0.4f, 0.4f, 0.6f),
                    new Vector3(r.position.x - 0.1f, r.position.y, r.position.z - 0.2f), ForceMode.VelocityChange);

                triggered = false;
                left = false;
                right = false;
                fired = true;
                startTime = Time.time;
            }
            else
            {
                if (Time.time - startTime > lifeTime)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }

        if (triggered)
        {
            if ((left && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.11f) ||
                ((right &&
                  OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.11f)))
            {
                if (locked)
                {
                    
                    
                    Vector3 deltaRot = hand.transform.localRotation.eulerAngles - pivotRot;
                    float z = deltaRot.z;
                    if (z < -250)
                    {
                        z += 360;
                    }else if (z > 250)
                    {
                        z -= 360;
                    }
                    pivotRot = hand.transform.localRotation.eulerAngles;
                    Vector3 old = transform.rotation.eulerAngles;
                    Debug.Log(z + ";" + old);
                    old.x -= z;
                    old.y = 0.0f;
                    old.z = 0.0f;
                    if (old.x < 268)

                    {
                        old.x = 268;
                    }

                    if (old.x > 358)
                    {
                        old.x = 358;
                    }

                    if (old.x > 357)
                    {
                        locked = false;
                    }

                    transform.rotation = Quaternion.Euler(old);
                }
                
            }
            else
            {
                triggered = false;
                left = false;
                right = false;
            }
        }

    }
    
}
