using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionController : MonoBehaviour {

	public GameObject leftHand;
	public GameObject rightHand;
	public GameObject player;

	public Vector3 velocity;
	public Quaternion angleVelocity;

	public Vector3 pivotCoord;
	public Vector3 pivotDir;
	public Vector3 pivotRot;

	public float vFalloff = 0.99f;
	public float wFalloff = 0.85f;

	public bool triggered = false;

	float lastTime;

	Vector3 lastCoord;
	Quaternion lastRot;

	// Use this for initialization
	void Start () {
		velocity = new Vector3 ();
		angleVelocity = Quaternion.identity;
		pivotRot = rightHand.transform.localPosition.normalized;
	}
	
	// Update is called once per frame
	void Update () {
		
		OVRInput.Update ();
		if (OVRInput.Get (OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch)>0.01f) {
			Debug.Log ("triggered");
			if (!triggered) {
				pivotCoord = rightHand.transform.position;
			    GetComponentInChildren<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
			    GetComponentInChildren<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
                //pivotRot = rightHand.transform.localPosition;
                triggered = true;
			} else {
				velocity = pivotCoord - rightHand.transform.position;
				angleVelocity = Quaternion.Inverse(Quaternion.Euler(pivotRot)) * Quaternion.Euler(rightHand.transform.localPosition.normalized);
				//angleVelocity = Quaternion.Slerp (Quaternion.identity, angleVelocity, 0.2f);

				//angleVelocity *= angleVelocity;
				Debug.Log(angleVelocity.eulerAngles);
				//player.transform.position += velocity;
				//player.transform.rotation *= angleVelocity;

				Vector3 dir = player.transform.position - pivotCoord; // get point direction relative to pivot
				dir = angleVelocity * dir; // rotate it
				player.transform.rotation *= angleVelocity;
				//player.transform.position = dir + pivotCoord; // calculate rotated point

			}
		} else {
			triggered = false;
			if (velocity.magnitude >= 0.00001f) {
				//player.transform.position += velocity;
				velocity *= vFalloff;
			}
			if(Quaternion.Angle(angleVelocity, Quaternion.identity) >= 0.005f){
				//player.transform.rotation *= angleVelocity;

				Vector3 dir = player.transform.position - pivotCoord; // get point direction relative to pivot
				dir = angleVelocity * dir; // rotate it
				//angleVelocity = Quaternion.Slerp (Quaternion.identity, angleVelocity, 0.1f);
				player.transform.rotation *= angleVelocity;
				//player.transform.position = dir + pivotCoord; // calculate rotated point

				angleVelocity = Quaternion.Slerp (Quaternion.identity, angleVelocity, wFalloff);
			}
		}

		pivotRot = rightHand.transform.localPosition.normalized;
	}
}
