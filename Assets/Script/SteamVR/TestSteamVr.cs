using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSteamVr : MonoBehaviour {

	
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
    void FixedUpdate()
    {

        player.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, 0f, 0f);
        //Debug.Log("center mass:" + player.GetComponent<Rigidbody>().centerOfMass);
        // Debug.Log("vel:" + player.GetComponent<Rigidbody>().velocity);
        if (Input.GetAxis("SteamVRRightTrigger") > 0.31f)
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
                
                angleVelocity = Quaternion.Inverse(Quaternion.Euler(pivotRot)) * Quaternion.Euler(rightHand.transform.localPosition.normalized);
                //angleVelocity = Quaternion.Slerp (Quaternion.identity, angleVelocity, 0.2f);

                //angleVelocity *= angleVelocity;
                Vector3 a = (velocity - lastVel) / Time.deltaTime;
                Vector3 force = player.GetComponent<Rigidbody>().mass * a;
                //Debug.Log(angleVelocity.eulerAngles);
                //player.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
                player.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                player.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
                player.GetComponent<Rigidbody>().ResetInertiaTensor();
                player.GetComponent<Rigidbody>().MovePosition(player.transform.position + velocity);
                //player.transform.position += velocity;
                //player.transform.rotation *= angleVelocity;

                Vector3 dir = player.transform.position - pivotCoord; // get point direction relative to pivot
                dir = angleVelocity * dir; // rotate it
                //player.transform.rotation *= angleVelocity;
                //player.transform.position = dir + pivotCoord; // calculate rotated point

            }
        }
        else
        {
            triggered = false;
            if (!fired)
            {
                //velocity = transform.position - lastCoord;
                if (velocity.magnitude >= 0.00001f)
                {
                    //player.transform.position += velocity;
                    player.GetComponent<Rigidbody>().AddForce(velocity / Time.fixedDeltaTime, ForceMode.VelocityChange);
                    //velocity *= vFalloff;
                }
                fired = true;
            }
            if (Quaternion.Angle(angleVelocity, Quaternion.identity) >= 0.005f)
            {
                //player.transform.rotation *= angleVelocity;

                Vector3 dir = player.transform.position - pivotCoord; // get point direction relative to pivot
                dir = angleVelocity * dir; // rotate it
                                           //angleVelocity = Quaternion.Slerp (Quaternion.identity, angleVelocity, 0.1f);
                //player.transform.rotation *= angleVelocity;
                //player.transform.position = dir + pivotCoord; // calculate rotated point

                angleVelocity = Quaternion.Slerp(Quaternion.identity, angleVelocity, wFalloff);
            }
        }
        lastCoord = transform.position;
        lastVel = GetComponentInChildren<Rigidbody>().velocity;
        pivotRot = rightHand.transform.localPosition.normalized;
    }
}
