using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class HandCollisionController : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    public Rigidbody parentRigid;
    public Collider leftHand;
    public Collider rightHand;
    public Collider head;
    public GameObject pivot;
    public Vector3 lastPosL;
    public Vector3 lastPosR;
    public Vector3 vel0;
    public float breakForce = 10.0f;
    public float speed = 1.0f;
    public Collider lastCol;
    public float lastCollision;
    public float collisionThres = 300;
    public bool colliderSetup = false;

    // Use this for initialization
    void Start()
    {
        
    }

    void addCollider()
    {
        if (left.transform.childCount < 1)
        {
            return;
        }
        Transform hand_l = left.transform.GetChild(0).GetChild(0).GetChild(0);
        Transform hand_r = right.transform.GetChild(0).GetChild(0).GetChild(0);
        for (int i = 0; i < hand_l.childCount; i++)
        {
            Transform c = hand_l.GetChild(i);
            if (c.name.Contains("index"))
            {
                Transform index3 = c.GetChild(0).GetChild(0);
                BoxCollider bc = index3.gameObject.AddComponent<BoxCollider>() as BoxCollider;
                bc.size = new Vector3(0.05f, 0.015f, 0.015f);
                bc.tag = "Fingertip";
                bc.isTrigger = true;
                break;
            }
        }
        for (int i = 0; i < hand_r.childCount; i++)
        {
            Transform c = hand_r.GetChild(i);
            if (c.name.Contains("index"))
            {
                Transform index3 = c.GetChild(0).GetChild(0);
                BoxCollider bc = index3.gameObject.AddComponent<BoxCollider>() as BoxCollider;
                bc.size = new Vector3(0.05f, 0.015f, 0.015f);
                bc.tag = "Fingertip";
                bc.isTrigger = true;
                colliderSetup = true;

                break;
            }
        }
    }

    void updateContainer()
    {
        int count = parentRigid.transform.childCount;
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < count; i++)
        {
            Transform child = parentRigid.transform.GetChild(i);
            children.Add(child);
        }
        parentRigid.transform.DetachChildren();
        parentRigid.transform.position = pivot.transform.position;
        parentRigid.transform.rotation = pivot.transform.rotation;
        for (int i = 0; i < children.Count; i++)
        {
            Transform child = children[i];
            child.parent = parentRigid.transform;
        }
        parentRigid.centerOfMass = new Vector3(0f, 0.0f, 0f);
    }

    void OnCollisionExit(Collision c)
    {
        
    }

    public void disableCollision(Collider c, bool flag = true)
    {
        Physics.IgnoreCollision(c, head, flag);
        Physics.IgnoreCollision(c, leftHand, flag);
        Physics.IgnoreCollision(c, rightHand, flag);
    }

    void OnCollisionEnter(Collision c)
    {
        //Debug.Log(Time.time - lastCollision);
        if (Time.time - lastCollision < collisionThres || c.collider.tag == "Pickable")
        {
            return;
        }
        lastCollision = Time.time;
        //Debug.Log(transform.name + parentRigid.name);
        float selfMass =0;
        float targetMass = 0;
        bool ignore = false;
        try {
            selfMass = parentRigid.mass;
            targetMass = c.rigidbody.mass;
        }
        catch (Exception e)
        {
            ignore = true;
            print(c.gameObject.name);
            print("error");
        }

        if (ignore)
        {
            return;
        }
        Vector3 vDir = (transform.position - c.transform.position).normalized;

        Vector3 tv1 = (leftHand.transform.position - lastPosL) / Time.deltaTime;
        Vector3 tv2 = (rightHand.transform.position - lastPosR) / Time.deltaTime;

        //Vector3 tv1 = (leftHand.transform.position - lastPosL).normalized * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude;
        //Vector3 tv2 = (rightHand.transform.position - lastPosR).normalized * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude;

        //Vector3 vel0 = vel * vDir;

        //Vector3
        // 0 + m0v0 = m1v1' + m0v0'
        Vector3 contact = c.contacts[0].point;
        float ltc = (leftHand.ClosestPoint(contact)-contact).magnitude;
        float rtc = (rightHand.ClosestPoint(contact)-contact).magnitude;
        //Debug.Log(c.rigidbody.name + ltc + rtc);
        if (ltc < rtc && ltc < 0.5f)
        {
            vel0 = tv1;
        }
        else if (ltc > rtc && rtc < 0.5f)
        {
            vel0 = tv2;
        }
        else
        {
            return;
        }
        //Debug.Log(c.rigidbody.name + vel0);
        contact = parentRigid.position + (contact - parentRigid.position).normalized;
        if ((targetMass / selfMass * vel0).magnitude > breakForce)
        {
            //parentRigid.GetComponent<MotionController>();
        }
        updateContainer();
        if (selfMass > targetMass)
        {
            parentRigid.AddForceAtPosition((-1.0f) * vel0 * speed * targetMass / selfMass , contact, ForceMode.VelocityChange);
            c.rigidbody.AddForceAtPosition(vel0 * speed * selfMass / targetMass, contact, ForceMode.VelocityChange);
        }
        else
        {
            parentRigid.AddForceAtPosition((-1.0f) * vel0 * speed * targetMass / selfMass, contact, ForceMode.VelocityChange);
        }
        
        

        //c.rigidbody.AddTorque(vel0/=10.0f, ForceMode.Impulse);
        //Debug.Log(c.relativeVelocity);
    }


    public void AddForceAtPosition(Vector3 force, Vector3 pos, ForceMode mode)
    {
        updateContainer();
        parentRigid.AddForceAtPosition(force, pos, mode);
    }

    // Update is called once per frame
    void Update()
    {
        if (!colliderSetup)
        {
            addCollider();
        }
        lastPosL = leftHand.transform.position;
        lastPosR = rightHand.transform.position;
    }
}