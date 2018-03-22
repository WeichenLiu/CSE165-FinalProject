using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{

    public Rigidbody player;
    public float sens = 1.0f;

	// Use this for initialization
	void Start () {
		
	}

    bool triggerPressed()
    {
        return (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.11f) ||
               (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.11f);
    }

	// Update is called once per frame
	void Update () {
	    if (!triggerPressed())
	    {
	        Vector2 t = (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
	        if (t.magnitude > 0.001f)
	        {
                player.angularVelocity = Vector3.zero;
	            player.inertiaTensor = Vector3.zero;
                player.inertiaTensorRotation = Quaternion.identity;
	            Vector3 currentRot = player.transform.localRotation.eulerAngles;
                current
                player.MoveRotation();
	        }
	    }
	}
}
