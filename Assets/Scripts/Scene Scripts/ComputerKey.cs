using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerKey : MonoBehaviour {

    public static Color normal = new Color(1, 1, 0, 0);
    public static Color hover = new Color(1, 1, 0, 0.2f);
    public static Color active = new Color(1, 1, 0, 1f);

    public Terminal terminal;
    public HandCollisionController hc;

    public string character;
    public bool isEnter;
    public bool isSpecial;
    public bool isBackSpace;
    private Image i;

	// Use this for initialization
	void Start ()
	{
        hc.disableCollision(GetComponent<BoxCollider>());
	    i = GetComponentInChildren<Image>();
	    i.color = normal;
        if (!isSpecial)
        {
            character = GetComponentInChildren<Text>().text;
        }
        else if (isEnter)
        {
            character = "<Enter>";
        }
        else if(isBackSpace)
        {
            character = "<Backspace>";
        }
        else
        {
            character = "<Space>";
        }
	}

    void OnTriggerEnter(Collider c)
    {
        //Debug.Log(c.name);
        if (c.tag == "Fingertip")
        {
            terminal.input(character);
            i.color = active;
        }
    }

    void OnTriggerExit(Collider c)
    {
        i.color = normal;
    }

	// Update is called once per frame
	void Update ()
    {
		
	}
}
