using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{


    public HandCollisionController parent;

    public OVRGrabbable self;
    public Vector3 lastPos;
    public Vector3 initialVel = new Vector3();

    private Collider[] cachedCollider;

    private float rootMass;


    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().velocity = initialVel;
        cachedCollider = GetComponents<Collider>();

        GetComponent<Rigidbody>().maxAngularVelocity = 0.7f;
        foreach (Collider c in cachedCollider)
        {
            Physics.IgnoreCollision(c, parent.head);
            Physics.IgnoreCollision(c, parent.leftHand);
            Physics.IgnoreCollision(c, parent.rightHand);
        }

        self = this.GetComponent<OVRGrabbable>();
        rootMass = parent.parentRigid.mass;
        
    }
}