using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerKey : MonoBehaviour {

    public static Color normal = new Color(1, 1, 0, 0);
    public static Color hover = new Color(1, 1, 0, 0.2f);
    public static Color active = new Color(1, 1, 0, 1f);

    public char character;
    public bool isEnter;
    public bool isSpecial;

	// Use this for initialization
	void Start ()
    {
        if (!isSpecial)
        {
            GetComponentInChildren<Text>().text = character.ToString();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
