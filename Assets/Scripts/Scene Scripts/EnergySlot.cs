using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySlot : MonoBehaviour {

    public enum CoreColor
    {
        Red,
        Green,
        Blue
    }

    public CoreColor Color;
    public GameObject EnergyCore;
    public float ActivateDistance;
    public Vector3 Offset;
    public GameController Controller;

    private OVRGrabbable coreGrabbable;
    private bool activated = false;
    private Vector3 initialPosition;

    public void Reset()
    {
        activated = false;
        EnergyCore.transform.position = initialPosition;
    }

    // Use this for initialization
    void Start ()
    {
        initialPosition = EnergyCore.transform.position;
        coreGrabbable = EnergyCore.GetComponent<OVRGrabbable>();

    }

    float GetDistance()
    {
        return (EnergyCore.transform.position - transform.position).magnitude;
    }

    void Activate()
    {
        activated = true;
        coreGrabbable.grabbedBy.ForceRelease(coreGrabbable);
        Destroy(coreGrabbable);
        EnergyCore.transform.position = transform.position + Offset;
        EnergyCore.transform.rotation = Quaternion.identity;
        EnergyCore.GetComponent<Rigidbody>().isKinematic = true;

        switch (Color)
        {
            case CoreColor.Red:
                Controller.activateRedCore();
                break;
            case CoreColor.Blue:
                Controller.activateBlueCore();
                break;
            case CoreColor.Green:
                Controller.activateGreenCore();
                break;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!activated && GetDistance() < ActivateDistance)
        {
            Debug.Log("hahaha");
            Activate();
        }
	}
}
