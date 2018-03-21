using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Terminal terminal;
    public Cable terminalCable;
    public Cable doorCable;
    public Door doorUp;
    public Door doorDown;
    public GameObject card;
    public GameObject password;
    public bool redCoreActivated = false;
    public bool greenCoreActivated = false;
    public bool blueCoreActivated = false;

    private Vector3 cardIniPos;
    private Quaternion cardIniRot;
    private Vector3 passwordIniPos;
    private Quaternion passwordIniRot;
    private Vector3 doorUpIniPos;
    private Vector3 doorDownIniPos;

    
    // Use this for initialization
    void Start ()
    {
        cardIniPos = card.transform.position;
        cardIniRot = card.transform.rotation;
        passwordIniPos = password.transform.position;
        passwordIniRot = password.transform.rotation;
        doorUpIniPos = doorUp.transform.position;
        doorDownIniPos = doorDown.transform.position;
    }
	


	// Update is called once per frame
	void Update () {
		
	}



    void checkCoreActivation()
    {
        if (redCoreActivated && blueCoreActivated && greenCoreActivated)
        {
            terminal.enableDisplay();
            terminalCable.activate();
        }
    }

    public void activateRedCore(bool flag = true)
    {
        redCoreActivated = true;
        checkCoreActivation();
    }
    public void activateGreenCore(bool flag = true)
    {
        greenCoreActivated = true;
        checkCoreActivation();
    }
    public void activateBlueCore(bool flag = true)
    {
        blueCoreActivated = true;
        checkCoreActivation();
    }

    public void activateDoorCable(bool flag = true)
    {
        doorCable.activate(flag);
    }

    public void openDoor(bool flag = true)
    {
        doorUp.Open(flag);
        doorDown.Open(flag);
    }
}
