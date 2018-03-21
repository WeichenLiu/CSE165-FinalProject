using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public bool locked = true;
    
    public int maxLine = 8;
    public List<string> display;
    public string currentInput;

    // password
    private const string password = "VIRTUA1";
    // Command
    private const string ls = "LS";
    private const string cat = "CAT";
    private const string help = "HELP";
    private const string open = "OPEN";

    // response
    private const string login = "";

    // Use this for initialization
    void Start () {
		
	}

    public void input(string s)
    {
        if (s[0] == '<') // <>
        {
            if (s.Contains("Space"))
            {
                currentInput += " ";
            }

            if (s.Contains("Enter"))
            {
                processCurrentInput();
            }
        }
        else
        {
            currentInput += s;
        }
    }

    void loginSuccess()
    {
        locked = false;

        updateDisplay();
    }

    void wrongPassword()
    {

    }

    void processCurrentInput()
    {
        if (locked)
        {
            if (currentInput == (">> " + password))
            {
                loginSuccess();
            }
            else
            {
                wrongPassword();
            }
        }
        else
        {

        }

        currentInput = ">> ";
    }

    void updateDisplay()
    {
        while (display.Count > maxLine)
        {
            display.RemoveAt(0);
        }

    }

	// Update is called once per frame
	void Update () {
		
	}
}
