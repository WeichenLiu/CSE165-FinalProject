using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    public AudioSource terminalAudio;
    public AudioSource keyboardAudio;

    public AudioClip keyboardClick;
    public AudioClip error;
    public AudioClip logon;

    public GameObject lockScreen;
    public GameController gc;
    public Text passwordText;
    public Text passwordPrompt;
    public Text text;
    public GameObject keyboard;
    public GameObject centerCamera;
    public bool locked = true;

    public int count = 5;
    public float switchTime = 1.0f;

    public int maxLine = 14;
    public List<string> display;
    public List<string> current;

    public string currentInput = "";

    private string passwordInput = "";

    // Prefix of currentInput
    private const string prefix = "tera:~ shin$ ";
    private bool sflag = false;
    private float lastTime = 0;

    // password
    private const string password = "VIRTUA1";
    // Command
    private const string ls = "LS";
    private const string print = "PRINT";
    private const string help = "HELP";
    //
    private const string door = "DOOR";
    private const string open = "DOOR OPEN";
    private const string close = "DOOR CLOSE";
    private const string overridePad = "DOOR OVERRIDE";

    private bool doorUnlocked = false;

    // response
    private const string doorOpenBad = "Not Authorized. Please use OVERRIDE to enable the door lock, and the swipe your Staff ID card.";
    private const string doorOpenGood = "Door is opened. Have a nice day.";
    private const string doorClosed = "Door is closed.";
    private const string overridePanel = "Override panel enable. Please swipe your card to manual open the door.";
    private const string unknownCommand = "Unknown command. Please use \"HELP\" to check for available command.";

    private string[] lsResult =
    {
        "-rw-@ shintaro staff Jan  1  6:00 INFO.TXT",
        "-rw-@ root     staff Jan  6 14:42 JOURNAL1.TXT",
        "-rw-@ root     staff Jan 12 21:20 JOURNAL2.TXT",
        "-rw-@ root     staff Jan 13 19:15 JOURNAL3.TXT"
    };

    private string[] login = { "Login successfully. Welcome Home.", "Type HELP for available commands." };
    private string[] doorHelp = { "Door Utilities: ", " - door open - Open the door", " - door close - Close the door" };
    private string[] helpText = { "Available Commands: ", " - DOOR: Door Utilities", " - LS: List the files", " - PRINT [FILE]: Show the content of a file" };

    // Use this for initialization
    void Start () {
        for (int i = 0; i < maxLine; i++)
        {
            display.Add("");
        }
        keyboard.SetActive(false);
        lockScreen.SetActive(false);
        text.enabled = false;
        keyboardAudio.clip = keyboardClick;
        terminalAudio.clip = error;
    }

    public void enableDisplay(bool flag = true)
    {

        keyboard.SetActive(flag);
        lockScreen.SetActive(flag);
        text.enabled = flag;
    }

    public void input(string s)
    {
        playKeyboard();
        if (!locked)
        {
            if (s[0] == '<') // <>
            {
                if (s.Contains("Backspace"))
                {
                    if (currentInput.Length >= 1)
                    {
                        currentInput = currentInput.Remove(currentInput.Length - 1);
                    }
                    else
                    {
                        playError();
                    }
                }
                else if (s.Contains("Space"))
                {
                    if (currentInput.Length < 33)
                    {
                        currentInput += " ";
                    }
                    else
                    {
                        playError();
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
                else
                {
                    playError();
                }
            }
        }
        else
        {
            if (s[0] == '<') // <>
            {
                if (s.Contains("Backspace"))
                {
                    if (passwordInput.Length >= 1)
                    {
                        passwordInput = passwordInput.Remove(passwordInput.Length - 1);
                    }
                    else
                    {
                        playError();
                    }
                }
                else if (s.Contains("Space"))
                {
                    if (passwordInput.Length < 15)
                    {
                        passwordInput += " ";
                    }
                    else
                    {
                        playError();
                    }
                }
                else if (s.Contains("Enter"))
                {
                    processCurrentInput();
                }


            }
            else
            {
                if (passwordInput.Length < 15)
                {
                    passwordInput += s;
                }
                else
                {
                    playError();
                }
            }

            passwordText.text = "";
            for (int i = 0; i < passwordInput.Length-1; i++)
            {
                passwordText.text += '*';
            }

            if (passwordInput.Length > 0)
            {
                passwordText.text += passwordInput[passwordInput.Length - 1];
            }

        }
    }

    void loginSuccess()
    {
        locked = false;
        lockScreen.SetActive(false);
        playLogon();
        insertInput(login);
    }

    void playError()
    {
        terminalAudio.clip = error;
        terminalAudio.Play();
    }

    void playLogon()
    {
        terminalAudio.PlayOneShot(logon);
    }

    void playKeyboard()
    {
        keyboardAudio.Play();
    }

    void wrongPassword()
    {
        playError();
        passwordPrompt.text = "Wrong password. " + count + " more chances left.";
        passwordInput = "";
        passwordText.text = "";
        count--;
    }

    public void setDoorUnlocked()
    {
        doorUnlocked = true;
    }

    void processUnknownCommand()
    {
        playError();
        insertInput(unknownCommand);
    }
    void processCurrentInput()
    {
        if (locked)
        {

            if (passwordInput == (password))
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
            string[] tokens = currentInput.Split(' ');
            switch (tokens[0])
            {
                case door:
                    insertInput(doorHelp);
                    break;
                case open:
                    if (doorUnlocked)
                    {
                        gc.openDoor();
                        insertInput(doorOpenGood);
                    }
                    else
                    {
                        insertInput(doorOpenBad);
                    }
                    break;
                case close:
                    gc.openDoor(false);
                    insertInput(doorClosed);
                    break;
                case overridePad:
                    gc.activateDoorLock();
                    insertInput(overridePanel);
                    break;
                case help:
                    insertInput(helpText);
                    break;
                case print:
                    switch (tokens[1])
                    {
                        case "INFO.TXT":
                            
                            break;
                        case "JOURNAL1.TXT":

                            break;
                        case "JOURNAL2.TXT":

                            break;
                        case "JOURNAL3.TXT":

                            break;
                    }
                    break;
                case ls:
                    insertInput(lsResult);
                    break;
                default:
                    processUnknownCommand();
                    break;
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
            if (s[49]!=' ')
            {
                string temp = s.Substring(0, 50);
                index = temp.LastIndexOf(' ');
            }
            curr = s.Substring(0, index);
            next = s.Substring(index);
            display.Add(curr);
            while (next.Length > 50)
            {
                index = 50;
                if (next[49] != ' ')
                {
                    string temp = next.Substring(0, 50);
                    index = temp.LastIndexOf(' ');
                }
                curr = next.Substring(0, index);
                next = next.Substring(index);
                display.Add(curr);
            }
            display.Add(next);
        }
        else
        {
            curr += s;
            display.Add(curr);
        }
    }

    void insertInput(string[] ss)
    {
        for (int i = 0; i < ss.Length; i++)
        {
            insertInput(ss[i]);
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
