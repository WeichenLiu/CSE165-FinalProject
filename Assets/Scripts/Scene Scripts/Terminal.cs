using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    public GameController gc;
    public Text text;
    public GameObject keyboard;
    public GameObject centerCamera;
    public bool locked = true;

    public int count = 5;
    public float switchTime = 1.0f;

    public int maxLine = 14;
    public List<string> display;
    public List<string> current;

    public string currentInput;

    // Prefix of currentInput
    private const string prefix = "shintaro@ttl1 $ ";
    private bool sflag = false;
    private float lastTime = 0;

    // password
    private const string password = "VIRTUA1";
    // Command
    private const string ls = "LS";
    private const string cat = "CAT";
    private const string help = "HELP";
    //
    private const string open = "OPEN";
    private const string door = "DOOR";
    private const string close = "CLOSE";
    private const string overridePad = "OVERRIDE";

    

    private bool doorFunction = false;

    // response
    private const string login = "";
    private const string doorOpened = "Door is opened. Have a nice day.";
    private const string doorClosed = "Door is closed.";
    private const string overridePanel = "Override panel enable. Please swipe your card to manual open the door.";
    private const string unknownCommand = "Unknown command. Please use \"HELP\" to check for available command.";

    // Use this for initialization
    void Start () {
        for (int i = 0; i < maxLine; i++)
        {
            display.Add("");
        }
        keyboard.SetActive(false);
        text.enabled = false;
    }

    public void enableDisplay(bool flag = true)
    {
        keyboard.SetActive(flag);
        text.enabled = flag;
    }

    public void input(string s)
    {
        if (s[0] == '<') // <>
        {
            if (s.Contains("Backspace"))
            {
                if (currentInput.Length >= 1)
                {
                    currentInput = currentInput.Remove(currentInput.Length - 1);
                }
            }
            else if (s.Contains("Space"))
            {
                if (currentInput.Length < 33)
                {
                    currentInput += " ";
                }
            }
            else if (s.Contains("Enter"))
            {
                processCurrentInput();
            }

            
        }
        else
        {
            if (currentInput.Length < 33)
            {
                currentInput += s;
            }
        }
    }

    void loginSuccess()
    {
        locked = false;
        insertInput("Login successfully. Welcome Home.");
    }

    void wrongPassword()
    {
        insertInput("Wrong password. " + count + " more chances left.");
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
            
            display.Add(prefix + currentInput);
            if (currentInput == door)
            {
                doorFunction = !doorFunction;
            }
            else if(doorFunction)
            {
                switch (currentInput)
                {
                    case open:
                        // or not
                        gc.openDoor();
                        insertInput(doorOpened);
                        break;
                    case close:
                        gc.openDoor(false);
                        insertInput(doorClosed);
                        break;
                    case overridePad:
                        gc.activateDoorCable();
                        insertInput(overridePanel);
                        break;
                    default:
                        doorFunction = false;
                        insertInput(unknownCommand);
                        break;
                }
            }
            
        }

        currentInput = "";
    }

    void insertInput(string s)
    {
        string curr = "";
        string next = "";
        if (s.Length > 50)
        {
            int index = 50;
            if (s[49]!=' ') {
                index = s.LastIndexOf(' ');
            }
            curr += s.Substring(0, index);
            next = s.Substring(index);
            display.Add(curr);
            while (next.Length > 50)
            {
                index = 50;
                if (next[49] != ' ')
                {
                    index = next.LastIndexOf(' ');
                }
                curr += next.Substring(0, index);
                next = next.Substring(index);
            }
            display.Add(curr);
        }
        else
        {
            curr += s;
            display.Add(curr);
        }
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
            float factor = (0 - count) * 0.1f;
            factor = factor > 0.5f ? 0.5f : factor;
            ge.intensity = factor;
            ge.colorIntensity = factor;
            ge.flipIntensity = factor;

        }

        if (count < -5)
        {
            ve.enabled = true;
            float factor = (-5 - count) * 0.1f;
            factor = factor > 0.5f ? 0.5f : factor;
            ve.vratio = factor;
        }
    }


    // Update is called once per frame
    void Update () {
        if (text.enabled)
        {
            updateDisplay();
            updateOutput();
            updateCurrentInput();
            updateVisual(locked);
        }

    }
}
