using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

    public Quaternion startRotation;
    public Quaternion endRotation;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float duration;

    // Use this for initialization
    void Start () {
        StartCoroutine("Animate", Time.time);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Animate(float start)
    {
        transform.rotation = startRotation;
        transform.position = startPosition;
        yield return null;
        while (Time.time - start < duration)
        {
            float t = (Time.time - start) / duration;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }
        transform.rotation = endRotation;
        transform.position = endPosition;
        yield return null;
    }
}
