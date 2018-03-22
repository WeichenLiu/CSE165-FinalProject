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

    public float shakeTime = 2f;
    public float darkenTime = 4f;
    public float subtitleTime = 4f;
    public bool tempshake = false;

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
    public int maxPrint = 200;
    public int displayIndex = 0;
    public bool printMode = false;
    public List<string> display;
    public List<string> printList;
    public List<string> current;

    public string currentInput = "";

    private string passwordInput = "";

    // Prefix of currentInput
    private const string prefix = "tera:~ shin$ ";
    private bool darken = false;
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
    private const string doorOpen = "OPEN";
    private const string doorClose = "CLOSE";
    private const string overridePad = "OVERRIDE";

    public bool doorUnlocked = false;

    // response
    private const string doorOpenBad = "Not Authorized. Please use OVERRIDE to enable the door lock, and the swipe your Staff ID card.";
    private const string doorOpenGood = "Door is opened. Have a nice day.";
    private const string doorClosed = "Door is closed.";
    private const string overridePanel = "Override panel enable. Please swipe your card to manual open the door.";
    private const string unknownCommand = "Unknown command. Please use \"HELP\" to check for available command.";

    private string[] lsResult =
    {
        "-rw-@ shintaro staff Jan  1  6:00 INFO",
        "-rw-@ root     staff Jan  6 14:42 EMAIL",
        "-rw-@ root     staff Jan 12 21:20 JOURNAL1",
        "-rw-@ root     staff Jan 13 19:15 JOURNAL2"
    };

    private const string printModeHelp = "<ENTER> exit; <8> scroll up; <2> scroll down";

    private string[] login = { "Login successfully. Welcome Home.", "Type HELP for available commands." };
    private string[] doorHelp = { "Door Utilities. Usage: ", " - DOOR OPEN - Open the door", " - DOOR CLOSE - Close the door" };
    private string[] helpText = { "Available Commands: ", " - DOOR: Door Utilities", " - LS: List the files", " - PRINT [FILE]: Show the content of a file" };
    private string[] infoText =
    {
        "Name: Kisaragi Shintaro",
        "Role: Programmer & Designer",
        "Gender: Male",
        "Age: 21",
        "Description: Genius Undergraduate Who Might Be Capable to Operate Human Reborn Project [Virtua1]"
    };
    private string[] emailText = {"From: Alexandra Drennan",
        "To: Noematics Mailing List",
        "Subject: [NML]",
        "Talos Principle",
        "Have you heard of the Talos Principle? It's this old philosophical concept about the impossibility of avoiding reality - no matter what you believe, if you lose your blood, you will die. I think that applies to our situation more than we'd like to admit.We could close our eyes and pretend that everything's going to be all right... but it won't change the physical reality of what's going to happen to our 4E 6F 20 6D 61 6E 20 69",
        "I think that, as scientists, it is our duty to face the truth, and to ask ourselves the most important question: how can we help?",
        "73 20 6C 69 62 65 72 61 74 65 64 20 66 72 6F 6D 20 66 65 61 72 20 77 68 6F 20 64 61 72 65 20 6E 6F 74 20 73 65 65 20 68 69 73",
        "20 70 6C 61 63 65 20 69 6E 20 74 68 65 20 77 6F 72 6C 64 20 61 73 20 69 74 20 69 73 3B 20 6E 6F 20 6D 61 6E 20 63 61 6E 20 61 63 68 69 65 76 65 20 74 I think I have an idea 68 65 20 67 72 65 61 74 6E 65 73 73 20 6F 66 20 77 68 69 63 68 20 68 65 20 69 73 20 63 61 70 61 62 6C 65 20 75 6E 74 69 6C 20",
        "68 65 20 68 61 73 20 61 6C 6C 6F 77 65 64 20 68 69 6D 73 65 6C 66 20 74 6F 20 73 65 65 20 68 69 73 20 6F 77 6E 20 6C 69 74 74 6C 65 6E 65 73 73 2E 20",
        "Regards,",
        "Alexandra"
    };


    private GlitchEffect ge;
    private VHSPostProcessEffect ve;
    private DarkenEffect de;

    // Use this for initialization
    void Start () {
        ge = centerCamera.GetComponent<GlitchEffect>();
        ve = centerCamera.GetComponent<VHSPostProcessEffect>();
        de = centerCamera.GetComponent<DarkenEffect>();
        for (int i = 0; i < maxLine; i++)
        {
            display.Add("");
        }
        //keyboard.SetActive(false);
        //lockScreen.SetActive(false);
        //text.enabled = false;
        keyboardAudio.clip = keyboardClick;
        terminalAudio.clip = error;
    }

    public void enableDisplay(bool flag = true)
    {

        keyboard.SetActive(flag);
        lockScreen.SetActive(flag);
        text.enabled = flag;
    }

    void scrollUp(bool up = true)
    {
        if (up)
        {
            displayIndex++;
            if (displayIndex >= printList.Count)
            {
                displayIndex = printList.Count - 1;
            }
        }
        else
        {
            displayIndex--;
            if (displayIndex < 0)
            {
                displayIndex = 0;
            }
        }
    }

    public void input(string s)
    {
        playKeyboard();
        if (!locked)
        {
            if (printMode)
            {
                currentInput = printModeHelp;
                if (s[0] == '<')
                {
                    if (s.Contains("Enter"))
                    {
                        printMode = false;
                        currentInput = "";
                    }
                }
                else
                {
                    switch (s)
                    {
                        case "2":
                            scrollUp(false);
                            break;
                        case "8":
                            scrollUp();
                            break;
                    }
                }
            }
            else if (s[0] == '<') // <>
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

        bool valid = true;
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
                    if (tokens.Length == 1)
                    {
                        insertInput(doorHelp);
                    }
                    else
                    {
                        switch (tokens[1])
                        {
                            case doorOpen:
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
                            case doorClose:
                                gc.openDoor(false);
                                insertInput(doorClosed);
                                break;
                        }
                    }
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
                        case "INFO":
                            
                            break;
                        case "EMAIL":
                            printMode = true;
                            printList.Clear();
                            insertInput(emailText, true);
                            displayIndex = printList.Count - 1;
                            StartCoroutine("shakeScreen", Time.time);
                            break;
                        case "JOURNAL1":

                            break;
                        case "JOURNAL2":

                            break;
                    }
                    break;
                case ls:
                    insertInput(lsResult);
                    break;
                default:
                    valid = false;
                    processUnknownCommand();
                    break;

            }
            if (!valid)
            {
                count--;
            }
            else
            {
                count = 5;
            }
        }

        
        currentInput = "";
    }

    void insertInput(string s, bool print = false)
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
            if (print)
            {
                printList.Add(curr);
            }
            else
            {
                display.Add(curr); 

            }
            
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
                if (print)
                {
                    printList.Add(curr);
                }
                else
                {
                    display.Add(curr);

                }
            }
            if (print)
            {
                printList.Add(next);
            }
            else
            {
                display.Add(next);

            }
        }
        else
        {
            curr += s;
            if (print)
            {
                printList.Add(curr);
            }
            else
            {
                display.Add(curr);

            }
        }
    }

    void insertInput(string[] ss, bool print = false)
    {
        for (int i = 0; i < ss.Length; i++)
        {
            insertInput(ss[i], print);
        }
    }

    void updateDisplay()
    {
        if (printMode)
        {
            while (printList.Count > maxPrint)
            {
                printList.RemoveAt(0);
            }
            current.Clear();
            int j = printList.Count - displayIndex;
            for (int i = 0; i < maxLine; i++)
            {
                if (j < printList.Count)
                {
                    current.Add(printList[j]);
                }
                else
                {
                    current.Add("");
                }

                j++;
            }
        }
        else
        {
            while (display.Count > maxLine)
            {
                display.RemoveAt(0);
            }

            current.Clear();
            current.InsertRange(0, display);
        }
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
        if (printMode)
        {
            text.text += printModeHelp;
        }
        else
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
    }


    void updateVisual(bool flag = true)
    {

        if (!tempshake && !doorUnlocked)
        {
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

            if (count < -10 && !darken)
            {
                de.enabled = true;
                StartCoroutine("darkenScreen", Time.time);
            }
        }
    }


    IEnumerator darkenScreen(float start)
    {
        while (Time.time - start < darkenTime)
        {
            de.ratio = (darkenTime - (Time.time - start)) / darkenTime;
            yield return null;
        }

        de.ratio = 0;
        while (Time.time - start < darkenTime + subtitleTime)
        {
            de.tratio = ((Time.time - start - darkenTime)) / subtitleTime;
            yield return null;
        }

        de.tratio = 1;
    }

    IEnumerator shakeScreen(float start)
    {
        tempshake = true;
        ge.enabled = true;
        float factor = 1f;
        ge.intensity = factor;
        ge.colorIntensity = factor;
        ge.flipIntensity = factor;

        while (Time.time - start < shakeTime)
        {
            yield return null;
        }

        ge.enabled = false;
        tempshake = false;
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
