using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using LCPrinter;

public class DOSBehaviour : MonoBehaviour
{
    OscIn _oscIn;
    OscOut _oscOut;

    public InputField command;
    public GameObject dosTextPrefab;
    public Transform contentParentContainer;
    public ScrollRect scrollRect;

    public string OPEN = "open ";
    public string ENTER = "enter ";
    public string AUTHORIZE = "authorize ";
    
    private string tempText;
    private bool crate2Access = false;
    private bool crate3Access = false;

    [SerializeField]
    private string CRATE1 = "E904FR289";
    [SerializeField]
    private string CRATE2 = "E22CV1280";
    [SerializeField]
    private string CRATE3 = "E78GH490";
    [SerializeField]
    private string CRATE2ACCESSCODES = "290683";
    [SerializeField]
    private string CRATE3ACCESSCODES = "955881";
    [SerializeField]
    private string USERNAME = "<b>darwinner81@nebula:~$ </b>";
  
    private int commandCounter = 0;
    public MicroscopeBehaviour microscope;
    public CrateOpener crateOpener;

    private int amsIceEarthScanCounter;
    private int amsIceMarsScanCounter;
    private int amsTeethScanCounter;
    private int amsRingScanCounter;
    private int amsNoCarbonScanCounter;
    private int amsNoPbUScanCounter;
    private int bdcScanCounter;

    private bool allowReport = false;

    public bool hypothesis1, hypothesis2, hypothesis3, hypothesis4, hypothesis5 = false;
    private bool generatingReport = false;
    private int protocolCounter = 0;

    private int protocol1Level, protocol2Level, protocol3Level, protocol4Level, protocol5Level = 0;

    public Texture2D[] likert_pro0, likert13_pro1, likert13_pro2, likert13_pro3, likert13_pro4, likert13_pro5;
    public Texture2D[] likert4_pro1, likert4_pro2, likert4_pro3, likert4_pro4, likert4_pro5;
    public Texture2D[] likert57_pro1, likert57_pro2, likert57_pro3, likert57_pro4, likert57_pro5;
    public string printerName = "";
    public int copies = 1;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Displays connected: " + Display.displays.Length);

        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
        if (Display.displays.Length > 2)
            Display.displays[2].Activate();

        _oscIn = gameObject.AddComponent<OscIn>();
        _oscOut = gameObject.AddComponent<OscOut>();

        _oscIn.Open(8000);
        _oscOut.Open(8000, "255.255.255.255");

        command.Select();
        command.ActivateInputField();

        _oscIn.MapInt("/amsie", AMSIceEarthCounterSetter);
        _oscIn.MapInt("/amsim", AMSIceMarsCounterSetter);
        _oscIn.MapInt("/amst", AMSTeethCounterSetter);
        _oscIn.MapInt("/amsr", AMSRingCounterSetter);
        _oscIn.MapInt("/amscn", AMSNoCarbonCounterSetter);
        _oscIn.MapInt("/amslun", AMSNoPbUCounterSetter);
        _oscIn.MapInt("/bdc", BDCCounterSetter);
        _oscIn.MapBool("/dosreport", ReportActivator);
        _oscIn.MapInt("/crate", OpenCrates);
    }

    void OpenCrates(int crateNumber)
    {
        if (crateNumber == 1)
        {
            crateOpener.OpenCrate1();
        }
        else if (crateNumber == 2)
        {
            crateOpener.OpenCrate2();
        }
        else if (crateNumber == 3)
        {
            crateOpener.OpenCrate3();
        }

    }

    void ReportActivator(bool state)
    {
        allowReport = state;
    }

    void AMSIceEarthCounterSetter(int scanCounter)
    {
        amsIceEarthScanCounter = scanCounter;
    }
    
    void AMSIceMarsCounterSetter(int scanCounter)
    {
        amsIceMarsScanCounter = scanCounter;
    }
    void AMSTeethCounterSetter(int scanCounter)
    {
        amsTeethScanCounter = scanCounter;
    }
    void AMSRingCounterSetter(int scanCounter)
    {
        amsRingScanCounter = scanCounter;
    }
    void AMSNoCarbonCounterSetter(int scanCounter)
    {
        amsNoCarbonScanCounter = scanCounter;
    }
    void AMSNoPbUCounterSetter(int scanCounter)
    {
        amsNoPbUScanCounter = scanCounter;
    }

    void BDCCounterSetter(int scanCounter)
    {
        Debug.Log("BCD RECIEVED: " + scanCounter);
        bdcScanCounter = scanCounter;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            command.Select();
            command.text = tempText;
            command.ActivateInputField();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            command.Select();
            command.text = "";
            command.ActivateInputField();
        }
        if (generatingReport && Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                GameObject dosText = CreateDOSText();
                dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
                dosText.GetComponent<Text>().text = "Confidence level entered: 1";
                ScrollToBottom();
                switch (protocolCounter)
                {
                    case 1:
                        protocol1Level = 1;
                        StartCoroutine(StartProtocol2());
                        break;
                    case 2:
                        protocol2Level = 1;
                        StartCoroutine(StartProtocol3());
                        break;
                    case 3:
                        protocol3Level = 1;
                        StartCoroutine(StartProtocol4());
                        break;
                    case 4:
                        protocol4Level = 1;
                        StartCoroutine(StartProtocol5());
                        break;
                    case 5:
                        protocol5Level = 1;
                        StartCoroutine(PrintReport());
                        break;
                    
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                GameObject dosText = CreateDOSText();
                dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
                dosText.GetComponent<Text>().text = "Confidence level entered: 2";
                ScrollToBottom();
                switch (protocolCounter)
                {
                    case 1:
                        protocol1Level = 2;
                        StartCoroutine(StartProtocol2());
                        break;
                    case 2:
                        protocol2Level = 2;
                        StartCoroutine(StartProtocol3());
                        break;
                    case 3:
                        protocol3Level = 2;
                        StartCoroutine(StartProtocol4());
                        break;
                    case 4:
                        protocol4Level = 2;
                        StartCoroutine(StartProtocol5());
                        break;
                    case 5:
                        protocol5Level = 2;
                        StartCoroutine(PrintReport());
                        break;

                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                GameObject dosText = CreateDOSText();
                dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
                dosText.GetComponent<Text>().text = "Confidence level entered: 3";
                ScrollToBottom();
                switch (protocolCounter)
                {
                    case 1:
                        protocol1Level = 3;
                        StartCoroutine(StartProtocol2());
                        break;
                    case 2:
                        protocol2Level = 3;
                        StartCoroutine(StartProtocol3());
                        break;
                    case 3:
                        protocol3Level = 3;
                        StartCoroutine(StartProtocol4());
                        break;
                    case 4:
                        protocol4Level = 3;
                        StartCoroutine(StartProtocol5());
                        break;
                    case 5:
                        protocol5Level = 3;
                        StartCoroutine(PrintReport());
                        break;

                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                GameObject dosText = CreateDOSText();
                dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
                dosText.GetComponent<Text>().text = "Confidence level entered: 4";
                ScrollToBottom();
                switch (protocolCounter)
                {
                    case 1:
                        protocol1Level = 4;
                        StartCoroutine(StartProtocol2());
                        break;
                    case 2:
                        protocol2Level = 4;
                        StartCoroutine(StartProtocol3());
                        break;
                    case 3:
                        protocol3Level = 4;
                        StartCoroutine(StartProtocol4());
                        break;
                    case 4:
                        protocol4Level = 4;
                        StartCoroutine(StartProtocol5());
                        break;
                    case 5:
                        protocol5Level = 4;
                        StartCoroutine(PrintReport());
                        break;

                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                GameObject dosText = CreateDOSText();
                dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
                dosText.GetComponent<Text>().text = "Confidence level entered: 5";
                ScrollToBottom();
                switch (protocolCounter)
                {
                    case 1:
                        protocol1Level = 5;
                        StartCoroutine(StartProtocol2());
                        break;
                    case 2:
                        protocol2Level = 5;
                        StartCoroutine(StartProtocol3());
                        break;
                    case 3:
                        protocol3Level = 5;
                        StartCoroutine(StartProtocol4());
                        break;
                    case 4:
                        protocol4Level = 5;
                        StartCoroutine(StartProtocol5());
                        break;
                    case 5:
                        protocol5Level = 5;
                        StartCoroutine(PrintReport());
                        break;

                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                GameObject dosText = CreateDOSText();
                dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
                dosText.GetComponent<Text>().text = "Confidence level entered: 6";
                ScrollToBottom();
                switch (protocolCounter)
                {
                    case 1:
                        protocol1Level = 6;
                        StartCoroutine(StartProtocol2());
                        break;
                    case 2:
                        protocol2Level = 6;
                        StartCoroutine(StartProtocol3());
                        break;
                    case 3:
                        protocol3Level = 6;
                        StartCoroutine(StartProtocol4());
                        break;
                    case 4:
                        protocol4Level = 6;
                        StartCoroutine(StartProtocol5());
                        break;
                    case 5:
                        protocol5Level = 6;
                        StartCoroutine(PrintReport());
                        break;

                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
            {
                GameObject dosText = CreateDOSText();
                dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
                dosText.GetComponent<Text>().text = "Confidence level entered: 7";
                ScrollToBottom();
                switch (protocolCounter)
                {
                    case 1:
                        protocol1Level = 7;
                        StartCoroutine(StartProtocol2());
                        break;
                    case 2:
                        protocol2Level = 7;
                        StartCoroutine(StartProtocol3());
                        break;
                    case 3:
                        protocol3Level = 7;
                        StartCoroutine(StartProtocol4());
                        break;
                    case 4:
                        protocol4Level = 7;
                        StartCoroutine(StartProtocol5());
                        break;
                    case 5:
                        protocol5Level = 7;
                        StartCoroutine(PrintReport());
                        break;

                }
            }
            else
            {
                GameObject dosText = CreateDOSText();
                dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
                dosText.GetComponent<Text>().text = "Invalid confidence level entered. Please enter a valid confidence level on a scale of 1 to 7";
                ScrollToBottom();
            }
            
        }
    }

    private IEnumerator PrintReport()
    {
        generatingReport = false;
        yield return null;
        command.interactable = true;
        command.Select();
        command.ActivateInputField();
        GameObject dosText = CreateDOSText();
        dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
        dosText.GetComponent<Text>().text = "Printing report. Please stand by.";
        ScrollToBottom();
        StartCoroutine(PrintProtocol(likert_pro0));
        yield return new WaitForSeconds(0.25f);
        while (protocolCounter > 0)
        {
            switch (protocolCounter)
            {
                case 5:
                    if (protocol1Level >= 1 && protocol1Level <= 3)
                    {
                        StartCoroutine(PrintProtocol(likert13_pro1));
                    }
                    else if (protocol1Level == 4)
                    {
                        StartCoroutine(PrintProtocol(likert4_pro1));
                    }
                    else if (protocol1Level >= 5 && protocol1Level <= 7)
                    {
                        StartCoroutine(PrintProtocol(likert57_pro1));
                    }
                    protocolCounter--;
                    break;

                case 4:
                    if (protocol2Level >= 1 && protocol2Level <= 3)
                    {
                        StartCoroutine(PrintProtocol(likert13_pro2));
                    }
                    else if (protocol2Level == 4)
                    {
                        StartCoroutine(PrintProtocol(likert4_pro2));
                    }
                    else if (protocol2Level >= 5 && protocol2Level <= 7)
                    {
                        StartCoroutine(PrintProtocol(likert57_pro2));
                    }
                    protocolCounter--;
                    break;

                case 3:
                    if (protocol3Level >= 1 && protocol3Level <= 3)
                    {
                        StartCoroutine(PrintProtocol(likert13_pro3));
                    }
                    else if (protocol3Level == 4)
                    {
                        StartCoroutine(PrintProtocol(likert4_pro3));
                    }
                    else if (protocol3Level >= 5 && protocol3Level <= 7)
                    {
                        StartCoroutine(PrintProtocol(likert57_pro3));
                    }
                    protocolCounter--;
                    break;

                case 2:
                    if (protocol4Level >= 1 && protocol4Level <= 3)
                    {
                        StartCoroutine(PrintProtocol(likert13_pro4));
                    }
                    else if (protocol4Level == 4)
                    {
                        StartCoroutine(PrintProtocol(likert4_pro4));
                    }
                    else if (protocol4Level >= 5 && protocol4Level <= 7)
                    {
                        StartCoroutine(PrintProtocol(likert57_pro4));
                    }
                    protocolCounter--;
                    break;

                case 1:
                    if (protocol5Level >= 1 && protocol5Level <= 3)
                    {
                        StartCoroutine(PrintProtocol(likert13_pro5));
                    }
                    else if (protocol5Level == 4)
                    {
                        StartCoroutine(PrintProtocol(likert4_pro5));
                    }
                    else if (protocol5Level >= 5 && protocol5Level <= 7)
                    {
                        StartCoroutine(PrintProtocol(likert57_pro5));
                    }
                    protocolCounter--;
                    break;
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator PrintProtocol(Texture2D[] protocolPages)
    {
        foreach (Texture2D texture2D in protocolPages)
        {
            Print.PrintTexture(texture2D.EncodeToPNG(), copies, printerName);
            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator StartProtocol2()
    {
        protocolCounter++;
        yield return new WaitForSeconds(1f);
        GameObject dosText = CreateDOSText();
        dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 180);
        dosText.GetComponent<Text>().text = "<b>Procedure 2</b>" +
                                            "\nDoes the <b>PELVIS</b> structure formed have any predictable functions?" +
                                            "\n<b>Conclusion:</b> The results found summarize that this unknown creature had a PELVIS-like bone structure with functions similar to known mammals on Earth, suggesting it evolved to adapt to its relatively low-shrubbed, grassland terrain." +
                                            "\nEnter confidence level of conclusion on a scale of 1 to 7. 1 being the lowest and 7 being the highest." +
                                            "\nWaiting for confidence level ...";
        ScrollToBottom();
    }

    private IEnumerator StartProtocol3()
    {
        protocolCounter++;
        yield return new WaitForSeconds(1f);
        GameObject dosText = CreateDOSText();
        dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 180);
        dosText.GetComponent<Text>().text = "<b>Procedure 3</b>" +
                                            "\nDoes the <b>HAND</b> structure formed have any predictable functions?" +
                                            "\n<b>Conclusion:</b> The results found summarize that this unknown creature had a HAND-like bone structure with functions similar to known mammals on Earth, suggesting it evolved to adapt to burrow in its underground environment." +
                                            "\nEnter confidence level of conclusion on a scale of 1 to 7. 1 being the lowest and 7 being the highest." +
                                            "\nWaiting for confidence level ...";
        ScrollToBottom();
    }

    private IEnumerator StartProtocol4()
    {
        protocolCounter++;
        yield return new WaitForSeconds(1f);
        GameObject dosText = CreateDOSText();
        dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 210);
        dosText.GetComponent<Text>().text = "<b>Procedure 4</b>" +
                                            "\nAre these fossils from Mars?" +
                                            "\n<b>Conclusion:</b> The results found conclude the fossilized teeth found in the collected samples share no common carbon composition with ingestible water found on earth, but did share a composition similar to ice samples found on Mars, suggesting the specimen originally came from Mars." +
                                            "\nEnter confidence level of conclusion on a scale of 1 to 7. 1 being the lowest and 7 being the highest." +
                                            "\nWaiting for confidence level ...";
        ScrollToBottom();
    }

    private IEnumerator StartProtocol5()
    {
        protocolCounter++;
        yield return new WaitForSeconds(1f);
        GameObject dosText = CreateDOSText();
        dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 180);
        dosText.GetComponent<Text>().text = "<b>Procedure 5</b>" +
                                            "\nDoes the timeline of the origin of this specimen coincide with when it may have realistically existed?" +
                                            "\n<b>Conclusion:</b> The results conclude the samples found on Mars can be dated to roughly 3 BYA (billion years ago), coinciding with the time period where it has been previously theorized that Mars may have supported life." +
                                            "\nEnter confidence level of conclusion on a scale of 1 to 7. 1 being the lowest and 7 being the highest." +
                                            "\nWaiting for confidence level ...";
        ScrollToBottom();
    }

    public void ScrollToBottom()
    {
        StartCoroutine(ForceScrollDown());
    }

    IEnumerator ForceScrollDown()
    {
        // Wait for end of frame AND force update all canvases before setting to bottom.
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }

    private GameObject CreateDOSText()
    {
        GameObject dosText = Instantiate(dosTextPrefab, contentParentContainer);
        commandCounter++;
        dosText.name = "DOSText" + commandCounter;
        return dosText;
    }

    public void EnterPressed()
    {
        if (command.text.Length > 0)
        {
            tempText = command.text;
            //command.text = command.text.ToLower();
            if (command.text == "help" || command.text == "HELP")
            {
                string commandList = "\nList of commands: " +
                                    "\n<b>Commands</b>            <b>Arguments</b>                                 <b>Usage</b>" +
                                    "\nopen                ID# of unlocked container                 Opens container with a valid ID#" +
                                    "\n                                                              <b>open E12CLE345</b> - will open container E12CLE345\n" +
                                    "\nauthorize           Password                                  Enable open command for restricted containers" +
                                    "\n                                                              <b>authorize MTAP012345</b> - will enable open command for the associated container\n" +
                                    "\nenter               Data#                                     Uploads data# for use in report" +
                                    "\n                                                              <b>enter CMNHMTA010101_01</b> - will upload CMNHMTA010101_01 to use in the report\n" +
                                    "\nreport                                                        Creates a report using uploaded data";


                GameObject dosText = CreateDOSText();
                dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 420);
                dosText.GetComponent<Text>().text = USERNAME + command.text + commandList;
            }
            else if (command.text == "open" || command.text == "OPEN")
            {
                string missingArgument = "\nCommand 'open' has missing arguments. 1 argument required <b>ID# of unlocked container</b>.";
                GameObject dosText = CreateDOSText();
                dosText.GetComponent<Text>().text = USERNAME + command.text + missingArgument;
            }
            else if (command.text == "authorize" || command.text == "AUTHORIZE")
            {
                string missingArgument = "\nCommand 'authorize' has missing arguments. 1 argument required <b>Password</b>.";
                GameObject dosText = CreateDOSText();
                dosText.GetComponent<Text>().text = USERNAME + command.text + missingArgument;
            }
            else if (command.text == "enter" || command.text == "ENTER")
            {
                string missingArgument = "\nCommand 'enter' has missing arguments. 1 argument required <b>Data# of evaluation conducted</b>.";
                GameObject dosText = CreateDOSText();
                dosText.GetComponent<Text>().text = USERNAME + command.text + missingArgument;
            }
            else if (command.text.StartsWith(OPEN) || command.text.StartsWith("OPEN "))
            {
                string id = command.text.Replace(OPEN, "");
                string unlocked = "\nContainer " + id + " unlocked.";
                if (id.Length > 0)
                {
                    if (string.Compare(id, CRATE1, true) == 0)
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = USERNAME + command.text + unlocked;
                        crateOpener.OpenCrate1();
                    }
                    else if ((string.Compare(id, CRATE2, true) == 0) && !crate2Access)
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = USERNAME + command.text + "\nAccess to container E78CLE490 denied without proper codes";
                    }
                    else if ((string.Compare(id, CRATE2, true) == 0) && crate2Access)
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = USERNAME + command.text + unlocked;
                        crateOpener.OpenCrate2();
                    }
                    else if ((string.Compare(id, CRATE3, true) == 0) && (!crate3Access))
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = USERNAME + command.text + "\nAccess to container E22CLE280 denied without proper codes.";
                    }
                    else if ((string.Compare(id, CRATE3, true) == 0) && (crate3Access))
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = USERNAME + command.text + unlocked;
                        crateOpener.OpenCrate3();
                    }
                    else
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = USERNAME + command.text + "\n'" + id + "' not recognized. Please enter a valid container ID#.";
                    }
                }
            }
            else if (command.text.StartsWith(AUTHORIZE) || command.text.StartsWith("UNLOCK "))
            {
                string id = command.text.Replace(AUTHORIZE, "");
                string unlocked = "\nPassword accepted.";
                if (id.Length > 0)
                {
                    if (string.Compare(id, CRATE2ACCESSCODES, true) == 0)
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = USERNAME + command.text + unlocked + " Container E78CLE490 clearance granted.";

                        crate2Access = true;
                    }
                    else if (string.Compare(id, CRATE3ACCESSCODES, true) == 0)
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = USERNAME + command.text + unlocked + " Container E22CLE280 clearance granted.";

                        crate3Access = true;
                    }
                    else
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = USERNAME + command.text + "\n'" + id + "' not recognized. Please enter a valid password";
                    }
                }

            }

            else if (command.text.StartsWith(ENTER) || command.text.StartsWith("ENTER "))
            {
                string id = command.text.Replace(ENTER, "");
                string unlocked = "\nUploaded " + id + " to this terminal.";
                if (id.Length > 0)
                {
                    id = id.ToUpper();
                    if (id.StartsWith("CMNHSEM0" + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "19_"))
                    {
                        int scanNumber = int.Parse(id.Replace("CMNHSEM0" + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "19_", ""));
                        if (scanNumber <= microscope.scanCounter)
                        {
                            GameObject dosText = CreateDOSText();
                            dosText.GetComponent<Text>().text = USERNAME + command.text + unlocked;

                            hypothesis1 = true;
                        }
                    }
                    else if (id.StartsWith("CMNHBCD0" + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "19_"))
                    {
                        int scanNumber = int.Parse(id.Replace("CMNHBCD0" + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "19_", ""));
                        if (scanNumber == bdcScanCounter || scanNumber <= 6)
                        {
                            GameObject dosText = CreateDOSText();
                            dosText.GetComponent<Text>().text = USERNAME + command.text + unlocked;

                            hypothesis2 = true;
                            if (scanNumber == 2)
                                hypothesis3 = true;
                        }
                    }
                    else if (id.StartsWith("CMNHAMS0" + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "19_"))
                    {
                        int scanNumber = int.Parse(id.Replace("CMNHAMS0" + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "19_", ""));
                        if (scanNumber == amsRingScanCounter)
                        {
                            GameObject dosText = CreateDOSText();
                            dosText.GetComponent<Text>().text = USERNAME + command.text + unlocked;

                            hypothesis4 = true;
                        }
                        else if (scanNumber == amsIceEarthScanCounter || scanNumber == amsIceMarsScanCounter || scanNumber == amsTeethScanCounter)
                        {
                            GameObject dosText = CreateDOSText();
                            dosText.GetComponent<Text>().text = USERNAME + command.text + unlocked;

                            hypothesis5 = true;
                        }
                        else if (scanNumber == amsNoCarbonScanCounter || scanNumber == amsNoPbUScanCounter )
                        {
                            GameObject dosText = CreateDOSText();
                            dosText.GetComponent<Text>().text = USERNAME + command.text + unlocked;
                        }
                    }
                    else
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = USERNAME + command.text + "\n'" + id + "' not recognized. Please enter a valid data#";
                    }
                }

            }
            else if (command.text == "report")
            {
                if ((hypothesis1 && hypothesis2 && hypothesis3 && hypothesis4 && hypothesis5) || allowReport)
                {
                    command.interactable = false;
                    GameObject dosText = CreateDOSText();
                    dosText.GetComponent<Text>().text = USERNAME + command.text + "\nGenerating report ...";
                    protocolCounter++;
                    StartCoroutine(StartReportGeneration());
                }
                else
                {
                    GameObject dosText = CreateDOSText();
                    dosText.GetComponent<Text>().text = USERNAME + command.text + "\nInsufficient data entered to generate report.";
                }
            }
            else
            {
                GameObject dosText = CreateDOSText();
                dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 90);
                dosText.GetComponent<Text>().text = USERNAME + command.text + "\nInvalid Command. '" + command.text + "' is not recognized as an internal or external command." +
                                                                              "\nPlease use <b>help</b> to get a list of valid commands";
            }
            command.Select();
            command.text = "";
            command.ActivateInputField();
        }
        else
        {
            string commandList = "\nList of commands: " +
                                    "\n<b>Commands</b>            <b>Arguments</b>                                 <b>Usage</b>" +
                                    "\nopen                ID# of unlocked container                 Opens container with a valid ID#" +
                                    "\n                                                              <b>open E12CLE345</b> - will open container E12CLE345\n" +
                                    "\nauthorize           Password                                  Enable open command for restricted containers" +
                                    "\n                                                              <b>authorize MTAP012345</b> - will enable open command for the associated container\n" +
                                    "\nenter               Data#                                     Uploads data# for use in report" +
                                    "\n                                                              <b>enter CMNHMTA010101_01</b> - will upload CMNHMTA010101_01 to use in the report\n" +
                                    "\nreport                                                        Creates a report using uploaded data";


            GameObject dosText = CreateDOSText();
            dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 420);
            dosText.GetComponent<Text>().text = USERNAME + command.text + commandList;
        }
    }

    private IEnumerator StartReportGeneration()
    {
        yield return new WaitForSeconds(1f);
        GameObject dosText = CreateDOSText();
        dosText.GetComponent<Text>().text = "You will be presented with a conclusion for each procudure in your protocol.\nReview each conclusion with team and enter confidence level.";
        //dosText.GetComponent<Text>().text = "Please read all protocol conclusions clearly at each protocol step.\nReview amongst authors and select general group confidence level.";
        ScrollToBottom();
        generatingReport = true;
        yield return new WaitForSeconds(1.5f);
        dosText = CreateDOSText();
        dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 150);
        dosText.GetComponent<Text>().text = "<b>Procedure 1</b>" + 
                                            "\nAre these samples conclusively bones?" +
                                            "\n<b>Conclusion:</b> The results conclusively determine that the samples discovered on the surface of Mars are indeed fossilized bones." +
                                            "\nEnter confidence level of conclusion on a scale of 1 to 7. 1 being the lowest and 7 being the highest." +
                                            "\nWaiting for confidence level ...";
        ScrollToBottom();

    }
}
