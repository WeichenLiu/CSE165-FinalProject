using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * 
 * OVRInput.Update ();
		if (OVRInput.Get (OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch)>0.11f) {
 * */
public class MotionController : MonoBehaviour {



    //// Update is called once per frame
    //void Update () {
    //    Debug.Log(Input.GetAxis("SteamVRLeftTrigger"));

    //   }


    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject player;

    public Vector3 velocity;
    public Vector3 lastVel;
    public Quaternion angleVelocity;

    public Vector3 pivotCoord;
    public Vector3 pivotDir;
    public Vector3 pivotRot;

    public float vFalloff = 0.99f;
    public float wFalloff = 0.85f;

    public bool triggered = false;
    private bool fired = true;

    float lastTime;

    Vector3 lastCoord;
    Quaternion lastRot;

    // Use this for initialization
    void Start()
    {
        velocity = new Vector3();
        angleVelocity = Quaternion.identity;
        pivotRot = rightHand.transform.localPosition.normalized;
        player.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, 0.0f, 0f);
        player.GetComponent<Rigidbody>().maxAngularVelocity = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {

        player.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, 0f, 0f);
        //Debug.Log("center mass:" + player.GetComponent<Rigidbody>().centerOfMass);
        // Debug.Log("vel:" + player.GetComponent<Rigidbody>().velocity);
        OVRInput.Update();
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.11f)
        {
            //Debug.Log("triggered");
            if (!triggered)
            {

                pivotCoord = rightHand.transform.position;
                player.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                player.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
                player.GetComponent<Rigidbody>().ResetInertiaTensor();
                player.GetComponent<Rigidbody>().isKinematic = true;
                //pivotRot = rightHand.transform.localPosition;
                triggered = true;
                fired = false;
            }
            else
            {
                player.GetComponentInChildren<Rigidbody>().isKinematic = false;
                velocity = pivotCoord - rightHand.transform.position;

                //Debug.Log(angleVelocity.eulerAngles);
                //player.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
                player.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                player.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
                player.GetComponent<Rigidbody>().ResetInertiaTensor();
                player.GetComponent<Rigidbody>().MovePosition(player.transform.position + velocity);
            }
        }
        else
        {
            triggered = false;
            if (!fired)
            {
                //velocity = transform.position - lastCoord;
                if ((velocity / Time.deltaTime).magnitude >= 0.001f)
                {
                    //player.transform.position += velocity;
                    player.GetComponent<Rigidbody>().AddForce(velocity / Time.deltaTime, ForceMode.VelocityChange);
                    //velocity *= vFalloff;
                }
                fired = true;
            }
            
        }
        lastCoord = player.transform.position;
        lastVel = player.GetComponent<Rigidbody>().velocity;
        pivotRot = rightHand.transform.localPosition.normalized;
    }
}
