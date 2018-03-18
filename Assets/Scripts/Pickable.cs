using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{


    public HandCollisionController parent;

    public OVRGrabbable self;
    public Vector3 lastPos;

    private Collider cachedCollider;

    private float rootMass;

    // Use this for initialization
    void Start()
    {
        cachedCollider = GetComponent<Collider>();
        GetComponent<Rigidbody>().maxAngularVelocity = 0.7f;
        Physics.IgnoreCollision(cachedCollider, parent.head);
        Physics.IgnoreCollision(cachedCollider, parent.leftHand);
        Physics.IgnoreCollision(cachedCollider, parent.rightHand);
        self = this.GetComponent<OVRGrabbable>();
        rootMass = parent.parentRigid.mass;
    }

    void OnCollisionEnter(Collision c)
    {


        ///Debug.Log(gameObject.name + " Collision ");
        if (!self.isGrabbed)
        {
            return;
        }
        //Debug.Log("Collision" + c.collider.name);
        Vector3 vel0 = (this.transform.position - lastPos) / Time.deltaTime;
        float targetMass = c.rigidbody.mass;
        Vector3 contact = c.contacts[0].point;
        //Debug.Log(name + vel0);
        contact = parent.parentRigid.position + (contact - parent.parentRigid.position).normalized;
        if (rootMass > targetMass)
        {
            parent.AddForceAtPosition((-1.0f) * vel0 * targetMass / rootMass, contact, ForceMode.VelocityChange);
            c.rigidbody.AddForceAtPosition(vel0 * rootMass / targetMass, contact, ForceMode.VelocityChange);
        }
        else
        {
            parent.AddForceAtPosition((-1.0f) * vel0 * targetMass / rootMass, contact, ForceMode.VelocityChange);

        }
    }

    void OnTriggerStay(Collider c)
    {
        //Debug.Log(c.tag);
    }

   

    // Update is called once per frame
    void Update()
    {
        lastPos = this.transform.position;
    }
}