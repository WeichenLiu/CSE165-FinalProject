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
    public GameObject body;

    public Vector3 velocity;
    public float maxSpeed = 0.7f;
    public Vector3 lastVel;
    public Quaternion angleVelocity;

    public Vector3 pivotCoord;

    public float vFalloff = 0.99f;
    public float wFalloff = 0.85f;

    public bool triggered = false;
    private bool fired = true;
    public bool breakGrab = false;
    public Collider lastGrab;

    float lastTime;

    public int handIndex = -1;

    Vector3 lastCoord;
    Quaternion lastRot;
    private Rigidbody r;

    // Use this for initialization
    void Start()
    {
        Physics.IgnoreCollision(leftHand.GetComponent<Collider>(), rightHand.GetComponent<Collider>());
        Physics.IgnoreCollision(leftHand.GetComponent<Collider>(), body.GetComponent<Collider>());
        Physics.IgnoreCollision(body.GetComponent<Collider>(), rightHand.GetComponent<Collider>());
        velocity = new Vector3();
        angleVelocity = Quaternion.identity;
        r = player.GetComponent<Rigidbody>();
        r.centerOfMass = new Vector3(0f, 0.0f, 0f);
        r.maxAngularVelocity = 0.7f;
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
                    r.velocity = new Vector3(0f, 0f, 0f);
                    r.angularVelocity = new Vector3(0f, 0f, 0f);
                    r.ResetInertiaTensor();
                    r.isKinematic = true;
                    //pivotRot = rightHand.transform.localPosition;
                    triggered = true;
                    fired = false;
                    lastGrab = c;
                }
            }
            else if (handIndex == 1 &&
                     OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.11f)
            {
                if (handIndex != 1 || !triggered)
                {
                    pivotCoord = rightHand.transform.position;
                    r.velocity = new Vector3(0f, 0f, 0f);
                    r.angularVelocity = new Vector3(0f, 0f, 0f);
                    r.ResetInertiaTensor();
                    r.isKinematic = true;
                    //pivotRot = rightHand.transform.localPosition;
                    triggered = true;
                    fired = false;
                    lastGrab = c;
                }
            }
            else
            {
                handIndex = -1;
            }
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c == lastGrab)
        {
            breakGrab = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //updateContainer();
        
        r.centerOfMass = new Vector3(0f, 0f, 0f);
        OVRInput.Update();
        if (r.velocity.magnitude > maxSpeed)
        {
            r.velocity = r.velocity.normalized * maxSpeed;
        }
        if (!breakGrab && ((handIndex == 0 && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.11f)
            || (handIndex == 1 &&
                OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.11f)))
        {
            if (triggered)
            {
                r.isKinematic = false;
                if (handIndex == 0)
                {
                    velocity = pivotCoord - leftHand.transform.position;
                }
                else
                {
                    velocity = pivotCoord - rightHand.transform.position;
                }
                
                r.velocity = new Vector3(0f, 0f, 0f);
                //player.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
                //player.GetComponent<Rigidbody>().ResetInertiaTensor();
                r.MovePosition(player.transform.position + velocity);
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
                    r.AddForce(velocity / Time.deltaTime, ForceMode.VelocityChange);
                    //Vector3 dir = (pivotCoord - player.transform.position).normalized;
                    //torque = Vector3.Cross(dir, player.transform.forward);
                    //player.GetComponent<Rigidbody>().AddTorque(torque, ForceMode.VelocityChange);
                    //StartCoroutine("RotateOverTime", pivotCoord);
                    //player.GetComponent<Rigidbody>().angularVelocity = (angleVelocity.eulerAngles);
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
