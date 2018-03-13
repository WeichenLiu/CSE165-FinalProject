using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTransfer : MonoBehaviour {

    public Rigidbody root;
    public bool grabbed = false;
    public Vector3 lastPos;

	// Use this for initialization
	void Start () {
		
	}

    void OnCollisionEnter(Collision c)
    {
        //Debug.Log("Collided");
        if (!grabbed || c.rigidbody.name == root.name)
        {
            
            return;
        }
        Vector3 vel0 = (this.transform.position - lastPos) / Time.deltaTime;
        float rootMass = root.mass;
        float targetMass = c.rigidbody.mass;
        Vector3 contact = c.contacts[0].point;
        //Debug.Log(name + vel0);
        contact = root.position + (contact - root.position).normalized;
        if (rootMass > targetMass)
        {
            root.AddForceAtPosition((-1.0f) * vel0 * targetMass / rootMass, contact, ForceMode.VelocityChange);
            c.rigidbody.AddForceAtPosition(vel0 * rootMass / targetMass, contact, ForceMode.VelocityChange);
        }
        else
        {
            root.AddForceAtPosition((-1.0f) * vel0 * targetMass / rootMass, contact, ForceMode.VelocityChange);
        }
    }

	// Update is called once per frame
	void Update ()
	{
	    lastPos = this.transform.position;
	}
}
