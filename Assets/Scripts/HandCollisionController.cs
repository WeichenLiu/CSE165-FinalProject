using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollisionController : MonoBehaviour
{
    public Rigidbody parentRigid;
    public GameObject leftHand;
    public GameObject rightHand;
    public Vector3 lastPosL;
    public Vector3 lastPosR;
    public Vector3 vel0;
    public Collider last;
    public Material b;
    public Material w;

    // Use this for initialization
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision c)
    {
        

        float selfMass = parentRigid.mass;
        float targetMass = c.rigidbody.mass;
        Vector3 vDir = (transform.position - c.transform.position).normalized;

        Vector3 tv1 = (leftHand.transform.position - lastPosL) / Time.deltaTime;
        Vector3 tv2 = (rightHand.transform.position - lastPosR) / Time.deltaTime;

        Debug.Log(name + vel0);
        //Vector3 vel0 = vel * vDir;

        //Vector3
        // 0 + m0v0 = m1v1' + m0v0'
        Vector3 contact = c.contacts[0].point;
        if (leftHand.GetComponent<Collider>().bounds.Contains(contact))
        {
            vel0 = tv1;
        }
        else if (rightHand.GetComponent<Collider>().bounds.Contains(contact))
        {
            vel0 = tv2;
        }
        else
        {
            return;
        }

        contact = parentRigid.position + (contact - parentRigid.position).normalized;
        if (selfMass > targetMass)
        {
            parentRigid.AddForceAtPosition((-1.0f) * vel0 * targetMass / selfMass, contact, ForceMode.VelocityChange);
            c.rigidbody.AddForceAtPosition(vel0 * selfMass / targetMass, contact, ForceMode.VelocityChange);
        }
        else
        {
            parentRigid.AddForceAtPosition((-1.0f) * vel0 * targetMass / selfMass, contact, ForceMode.VelocityChange);
        }

        //c.rigidbody.AddTorque(vel0/=10.0f, ForceMode.Impulse);
        //Debug.Log(c.relativeVelocity);
    }


    // Update is called once per frame
    void Update()
    {
        lastPosL = leftHand.transform.position;
        lastPosR = rightHand.transform.position;
    }
}