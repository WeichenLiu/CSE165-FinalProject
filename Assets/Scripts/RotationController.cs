using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class RotationController : MonoBehaviour
{

    public Rigidbody player;
    public float sens = 1.0f;
    public GameObject body;

	// Use this for initialization
	void Start () {
		
	}

    bool triggerPressed()
    {
        return (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.11f) ||
               (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.11f);
    }

    void updateContainer()
    {
        int count = player.transform.childCount;
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < count; i++)
        {
            Transform child = player.transform.GetChild(i);
            children.Add(child);
        }
        player.transform.DetachChildren();
        player.transform.position = body.transform.position;
        player.transform.rotation = body.transform.rotation;
        for (int i = 0; i < children.Count; i++)
        {
            Transform child = children[i];
            child.parent = player.transform;
        }
    }

    // Update is called once per frame
    void Update () {
	    if (!triggerPressed())
	    {
	        Vector2 t = (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch));
	        if (t.magnitude > 0.001f)
	        {
	            player.angularVelocity = Vector3.zero;
                //player.inertiaTensor = Vector3.zero;
                //player.inertiaTensorRotation = Quaternion.identity;
                //Quaternion currentRot = player.transform.rotation;
                //Quaternion up = Quaternion.Euler(player.transform.up);
                //Quaternion right = Quaternion.Euler(player.transform.right);
                //Quaternion result = Quaternion.SlerpUnclamped(currentRot, up, 0.1f * sens);
                //result = Quaternion.SlerpUnclamped(result, right, 0.1f * sens);
	            updateContainer();

                player.transform.Rotate(new Vector3(0, 1, 0) * t.x * sens, Space.World);
	            player.transform.Rotate(new Vector3(1, 0, 0) * t.y * sens, Space.World);
                }
	    }
    }
}
