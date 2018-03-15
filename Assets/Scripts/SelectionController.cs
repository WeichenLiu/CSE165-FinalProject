using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public Rigidbody self;
    public GameObject rightHand;
    public GameObject light;
    public GameObject attachedObj;
    public Vector3 velocity;
    public Vector3 lastPos;
    public float objSize = 0;

    public bool attached = false;

    private bool fired = true;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.GetAxis("SteamVRRightGrip"));
        OVRInput.Update();
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch) > 0.11f)
        {
            if (!attached)
            {
                light.SetActive(true);
                RaycastHit hitInfo;
                bool hit = Physics.Raycast(rightHand.transform.position, rightHand.transform.forward, out hitInfo,
                    10.1f);
                Debug.DrawLine(rightHand.transform.position,
                    rightHand.transform.position + rightHand.transform.forward * 0.1f);
                if (hit)
                {
                    if (hitInfo.collider.tag == "Pick")
                    {
                        if (hitInfo.rigidbody.mass < self.mass / 15.0f)
                        {
                            attached = true;
                            attachedObj = hitInfo.rigidbody.gameObject;
                            attachedObj.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                            attachedObj.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
                            attachedObj.GetComponent<Rigidbody>().ResetInertiaTensor();
                            //attachedObj.GetComponent<Rigidbody>().isKinematic = true;
                            attachedObj.GetComponent<Grabbable>().grabbed = true;
                            attachedObj.GetComponent<Grabbable>().root = self;
                            Vector3 boundSize = attachedObj.GetComponent<Collider>().bounds.size;
                            objSize = Mathf.Max(Mathf.Max(boundSize.x, boundSize.y), boundSize.z);
                            //Debug.Log("attached: " + hitInfo.collider.name);
                            fired = false;
                        }
                    }
                }
            }
            else
            {
                lastPos = attachedObj.transform.position;

                //

                //player.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
                attachedObj.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                attachedObj.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
                attachedObj.GetComponent<Rigidbody>().ResetInertiaTensor();
                attachedObj.GetComponent<Rigidbody>()
                    .MovePosition(rightHand.transform.position + rightHand.transform.forward * (objSize / 2.0f + 0.3f));
                attachedObj.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(rightHand.transform.forward));

                velocity = rightHand.transform.position + rightHand.transform.forward * (objSize / 2.0f + 0.3f) -
                           lastPos;
                //Debug.Log(velocity);
            }
        }
        else
        {
            light.SetActive(false);
            if (attached)
            {
                attached = false;
                attachedObj.GetComponent<Grabbable>().grabbed = false;
                attachedObj.GetComponent<Collider>().isTrigger = false;
                //attachedObj.GetComponent<Rigidbody>().isKinematic = false;
            }

            if (!fired)
            {
                //velocity = transform.position - lastCoord;
                if (velocity.magnitude >= 0.00001f)
                {
                    //player.transform.position += velocity;
                    RaycastHit hitInfo;
                    Physics.Raycast(rightHand.transform.position, rightHand.transform.forward, out hitInfo,
                        0.2f);
                    attachedObj.GetComponent<Rigidbody>().AddForceAtPosition(velocity / Time.deltaTime, hitInfo.point,
                        ForceMode.VelocityChange);
                    //velocity *= vFalloff;
                }

                fired = true;
            }
        }
    }
}