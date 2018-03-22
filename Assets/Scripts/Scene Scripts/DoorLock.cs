using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorLock : MonoBehaviour
{

    public Terminal terminal;
    public GameObject Card;
    public float ActivateDistance;
    public GameController Controller;
    public float ActivateTime;

    public Image Background;
    public Image LockImage;
    public Image CardImage;

    public Color DisableColor;
    public Color NormalColor;
    public Color ActivatedColor;

    private bool activated;
    private bool on;
    private bool hovering;
    private AudioSource doorLockAudio;

    // Use this for initialization
    void Start ()
	{
	    doorLockAudio = GetComponent<AudioSource>();
        SwitchOff();
	}

    float GetDistance()
    {
        return (transform.position - Card.transform.position).magnitude;
    }
	
	// Update is called once per frame
	void Update () {
        if (!hovering && GetDistance() < ActivateDistance)
        {
            hovering = true;
            if (!activated)
            {
                Activate();
            }
        }
        else if (hovering && GetDistance() >= ActivateDistance)
        {
            hovering = false;
        }
	}

    public void SwitchOff()
    {
        on = false;
        Background.color = DisableColor;
        LockImage.gameObject.SetActive(true);
        CardImage.gameObject.SetActive(false);
    }

    public void SwitchOn()
    {
        on = true;
        Background.color = NormalColor;
        LockImage.gameObject.SetActive(false);
        CardImage.gameObject.SetActive(true);
    }

    public void Activate()
    {
        doorLockAudio.Play();
        activated = true;
        Controller.openDoor();
        terminal.setDoorUnlocked();
        StartCoroutine("ActivateUI", Time.time);
    }

    IEnumerator ActivateUI(float startTime)
    {
        while (Time.time - startTime < ActivateTime)
        {
            Background.color = ActivatedColor;
            yield return null;
        }
        Background.color = NormalColor;
    }
}
