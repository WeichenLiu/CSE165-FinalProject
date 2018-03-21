using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    public Text text;
    public GameObject centerCamera;
    public bool locked = true;

    public int count = 5;
    public float switchTime = 1.0f;

    public int maxLine = 8;
    public List<string> display;
    public List<string> current;

    public string currentInput;

    // Prefix of currentInput
    private const string prefix = "liby99@ttl1 $ ";
    private bool sflag = false;
    private float lastTime = 0;

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
        for (int i = 0; i < maxLine; i++)
        {
            display.Add("");
        }
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
        display.Add("Login successfully. Welcome Home.");
    }

    void wrongPassword()
    {
        display.Add("Wrong password. " + count + " more chances left.");
        count--;
        
    }

    void processCurrentInput()
    {
        if (locked)
        {
            if (currentInput == (password))
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

        currentInput = "";
    }

    void updateDisplay()
    {
        while (display.Count > maxLine)
        {
            display.RemoveAt(0);
        }




        current.Clear();
        current.InsertRange(0, display);
    }

    void updateOutput()
    {
        string s = "";
        for (int i = 0; i < current.Count; i++)
        {
            s += current[i] + "\n";
        }

        text.text = s;
    }

    void updateCurrentInput()
    {
        string s = prefix;
        s += currentInput;
        if (sflag)
        {
            s += "|";
            if (Time.time - lastTime > switchTime)
            {
                sflag = false;
                lastTime = Time.time;
            }
        }
        else
        {
            if (Time.time - lastTime > switchTime)
            {
                sflag = true;
                lastTime = Time.time;
            }
        }

        text.text += s;
    }


    void updateVisual(bool flag = true)
    {

        GlitchEffect ge = centerCamera.GetComponent<GlitchEffect>();
        VHSPostProcessEffect ve = centerCamera.GetComponent<VHSPostProcessEffect>();
        if (!flag || count > 0)
        {
            ge.enabled = false;
            ve.enabled = false;
            return;
        }
        if (count <= 0)
        {
            ge.enabled = true;
            float factor = (0 - count) * 0.2f;
            factor = factor > 1.0f ? 1.0f : factor;
            ge.intensity = factor;
            ge.colorIntensity = factor;
            ge.flipIntensity = factor;

        }

        if (count < -5)
        {
            ve.enabled = true;
            float factor = (-5 - count) * 0.2f;
            factor = factor > 1.0f ? 1.0f : factor;
            ve.vratio = factor;
        }
    }


    // Update is called once per frame
    void Update () {
		updateDisplay();
        updateOutput();
        updateCurrentInput();
	    updateVisual(locked);
        
	}
}
