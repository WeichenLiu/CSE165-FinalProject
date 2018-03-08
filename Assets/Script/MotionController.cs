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
	public Quaternion pivotRot;

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
	}
	
	// Update is called once per frame
	void Update () {
		OVRInput.Update ();
		if (OVRInput.Get (OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTrackedRemote)>0.0001f) {
			if (!triggered) {
				pivotCoord = rightHand.transform.position;
				pivotRot = rightHand.transform.rotation;
				triggered = true;
			} else {
				velocity = pivotCoord - rightHand.transform.position;
				angleVelocity = Quaternion.Inverse(pivotRot) * rightHand.transform.rotation;
				player.transform.position += velocity;
				player.transform.rotation *= angleVelocity;
			}
		} else {
			triggered = false;
			if (velocity.magnitude >= 0.01f) {
				player.transform.position += velocity;
				velocity *= vFalloff;
			}
			if(Quaternion.Angle(angleVelocity, Quaternion.identity) >= 0.005f){
				player.transform.rotation *= angleVelocity;
				angleVelocity = Quaternion.Slerp (Quaternion.identity, angleVelocity, wFalloff);
			}
		}
	}
}
