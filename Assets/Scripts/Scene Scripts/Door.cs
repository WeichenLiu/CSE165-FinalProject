using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public float dist;
    public bool opened = false;
    public float time = 0.3f;
    public Vector3 startPos;

	// Use this for initialization
	void Start ()
	{
	    startPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Open(bool flag = true)
    {
        opened = flag;
        StartCoroutine("OpenOverTime", Time.time);
    }

    

    IEnumerator OpenOverTime(float start)
    {
        while (Time.time - start < time)
        {
            float ratio = (Time.time - start) / time;
            if (opened)
            {
                this.transform.position = Vector3.Lerp(startPos, startPos + this.transform.forward * dist, ratio);
            }
            else
            {
                this.transform.position = Vector3.Lerp(startPos + this.transform.forward * dist, startPos, ratio);
            }

            yield return null;
        }
        if (opened)
        {
            this.transform.position = startPos + this.transform.forward * dist;
        }
        else
        {
            this.transform.position = startPos;
        }
    }
}
