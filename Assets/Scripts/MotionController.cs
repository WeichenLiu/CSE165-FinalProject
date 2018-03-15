using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionController : MonoBehaviour
{
    public float time = 2000.0f;
    public Vector3 torque;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject player;

    public Vector3 velocity;
    public Vector3 lastVel;
    public Quaternion angleVelocity;

    public Vector3 pivotCoord;

    public float vFalloff = 0.99f;
    public float wFalloff = 0.85f;

    public bool triggered = false;
    private bool fired = true;

    float lastTime;

    public int handIndex = -1;

    Vector3 lastCoord;
    Quaternion lastRot;

    // Use this for initialization
    void Start()
    {
        velocity = new Vector3();
        angleVelocity = Quaternion.identity;
        player.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, 0.0f, 0f);
        player.GetComponent<Rigidbody>().maxAngularVelocity = 0.7f;
    }


    void OnTriggerStay(Collider c)
    {
        if (handIndex == -1 && c.tag == "Drag")
        {
            if (c.gameObject.name == "ChairTrigger")
            {
            }
            else
            {
                Collider[] cols = Physics.OverlapBox(c.bounds.center, c.bounds.extents);
                for (int i = 0; i < cols.Length; i++)
                {
                    Collider curr = cols[i];
                    //if (curr.name == "base")
                    //{
                    //    break;
                    //}else 
                    if (curr.gameObject.name == "controller_left")
                    {
                        handIndex = 0;
                        break;
                    }
                    else if (curr.gameObject.name == "controller_right")
                    {
                        handIndex = 1;
                        break;
                    }
                }
            }

            if (handIndex == 0 && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.11f)
            {
                if (handIndex != 0 || !triggered)
                {
                    pivotCoord = leftHand.transform.position;
                    player.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    player.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
                    player.GetComponent<Rigidbody>().ResetInertiaTensor();
                    player.GetComponent<Rigidbody>().isKinematic = true;
                    //pivotRot = rightHand.transform.localPosition;
                    triggered = true;
                    fired = false;
                }
            }
            else if (handIndex == 1 &&
                     OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.11f)
            {
                if (handIndex != 1 || !triggered)
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
            }
            else
            {
                handIndex = -1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        player.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, 0f, 0f);
        //Debug.Log("center mass:" + player.GetComponent<Rigidbody>().centerOfMass);
        // Debug.Log("vel:" + player.GetComponent<Rigidbody>().velocity);
        OVRInput.Update();
        if ((handIndex == 0 && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.11f)
            || (handIndex == 1 &&
                OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.11f))
        {
            if (triggered)
            {
                player.GetComponentInChildren<Rigidbody>().isKinematic = false;
                if (handIndex == 0)
                {
                    velocity = pivotCoord - leftHand.transform.position;
                }
                else
                {
                    velocity = pivotCoord - rightHand.transform.position;
                }

                //Debug.Log(angleVelocity.eulerAngles);
                //player.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
                player.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                player.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
                player.GetComponent<Rigidbody>().ResetInertiaTensor();
                player.GetComponent<Rigidbody>().MovePosition(player.transform.position + velocity);
                float dist = (pivotCoord - player.transform.position + velocity).magnitude;
                Quaternion rot = Quaternion.Slerp(player.transform.rotation,
                    Quaternion.Euler(pivotCoord - player.transform.position + velocity), 0.01f);
                //player.GetComponent<Rigidbody>().MoveRotation(rot);
                //player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.Euler(pivotCoord - player.transform.position), 0.01f);
                //player.GetComponent<Rigidbody>().MovePosition(pivotCoord - dist * (rot.eulerAngles).normalized);

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
                    angleVelocity = (Quaternion.Inverse(lastRot) * player.transform.rotation);
                    player.GetComponent<Rigidbody>().AddForce(velocity / Time.deltaTime, ForceMode.VelocityChange);
                    //Vector3 dir = (pivotCoord - player.transform.position).normalized;
                    //torque = Vector3.Cross(dir, player.transform.forward);
                    //player.GetComponent<Rigidbody>().AddTorque(torque, ForceMode.VelocityChange);
                    //StartCoroutine("RotateOverTime", pivotCoord);
                    //player.GetComponent<Rigidbody>().angularVelocity = (angleVelocity.eulerAngles);
                    //velocity *= vFalloff;
                }

                handIndex = -1;
                fired = true;
            }
        }

        lastCoord = player.transform.position;
        lastRot = player.transform.rotation;
    }

    IEnumerator RotateOverTime(Vector3 target)
    {
        float t = 0;
        while (t < time) 
        {
            t += Time.deltaTime;
            Quaternion rot = Quaternion.Slerp(player.transform.rotation,
                Quaternion.Euler(target - player.transform.position), 0.3f);
            player.transform.rotation = rot;
            yield return null;
        }
    }
}
