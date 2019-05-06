using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Phidget22;
using Phidget22.Events;

public class MicroscopeBehaviour : MonoBehaviour
{
    private DigitalInput ScanButton = new DigitalInput();        //used to register when the player presses the scan button
    
    private RFID RFIDMicroscope = new RFID();           //used for the RFID Scanner

    OscIn _oscIn;
    OscOut _oscOut;

    public string[] boneRFIDTagStings;
    public string[] rockRFIDTagStings;

    public Image microscopeScanImage;
    public GameObject imageHolder;
    public Text dataNumber;

    public GameObject introduction;
    public GameObject scanning;
    public GameObject noSampleScanned;
    public Sprite rockImage;
    public Sprite boneImage;
    private string MICROSCOPEDATAID = "Data #: CMNHSEM0"+ System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + "19_";
    private bool tagPresent = false;
    

    public AudioSource semSFX;
    public AudioClip microScope;

   
    public int scanCounter = 0;
    private bool audioPlaying = false;
    // Start is called before the first frame update
    void Start()
    {

        _oscIn = gameObject.AddComponent<OscIn>();
        _oscOut = gameObject.AddComponent<OscOut>();

        _oscIn.Open(8000);
        _oscOut.Open(8000, "255.255.255.255");


        _oscIn.MapInt("/resetsem", ResetMachine);

        imageHolder.SetActive(false);
        dataNumber.gameObject.SetActive(false);
        scanning.SetActive(false);
        noSampleScanned.SetActive(false);
        introduction.SetActive(true);
        


        //MicroscopeBackgroundSource.Play();
 
        ScanButton.DeviceSerialNumber = 523574;
        ScanButton.Channel = 0;
        ScanButton.IsLocal = true;
        ScanButton.Attach += digitalInput_Attach;
        ScanButton.StateChange += digitalInput_StateChange;

        RFIDMicroscope.DeviceSerialNumber = 453586;
        RFIDMicroscope.Channel = 0;
        RFIDMicroscope.IsLocal = true;
        RFIDMicroscope.Attach += rfid_Attach;
        RFIDMicroscope.Tag += rfid_Tag;
        RFIDMicroscope.TagLost += rfid_TagLost;
        

        try
        {
            ScanButton.Open(5000);
            RFIDMicroscope.Open(5000);
        }
        catch (PhidgetException e)
        {
            Debug.Log("Failed: " + e.Message);
        }

        // StartCoroutine(TurnRFIDAntennaOff());
    }

    private void ResetMachine(int args)
    {
        imageHolder.SetActive(false);
        dataNumber.gameObject.SetActive(false);
        scanning.SetActive(false);
        noSampleScanned.SetActive(false);
        introduction.SetActive(true);
    }

    void OnDestroy()
    {
        ScanButton.Close();
        RFIDMicroscope.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScanButton.Attach -= digitalInput_Attach;
            ScanButton.StateChange -= digitalInput_StateChange;
            ScanButton.Close();
            ScanButton = null;

            RFIDMicroscope.Attach -= rfid_Attach;
            RFIDMicroscope.Tag -= rfid_Tag;
            RFIDMicroscope.TagLost -= rfid_TagLost;
            RFIDMicroscope.Close();
            RFIDMicroscope = null;

            Application.Quit();
        }

    }

    void OnApplicationQuit()
    {
        if (Application.isEditor)
            Phidget.FinalizeLibrary(0);
        else
            Phidget.FinalizeLibrary(0);
    }

    void rfid_Tag(object sender, RFIDTagEventArgs e)
    {
        tagPresent = true;
        Debug.Log("Tag Present: " + tagPresent);
    }

    void rfid_TagLost(object sender, RFIDTagLostEventArgs e)
    {
        tagPresent = false;
        Debug.Log("Tag Present: " + tagPresent);
    }

    private IEnumerator TurnRFIDAntennaOff()
    {
        yield return new WaitForSeconds(0.5f);
        RFIDMicroscope.AntennaEnabled = false;
    }

    void digitalInput_Attach(object sender, Phidget22.Events.AttachEventArgs e)
    {
        DigitalInput attachedDevice = ((DigitalInput)sender);
        int deviceSerial = attachedDevice.DeviceSerialNumber;
        Debug.Log("Attached device " + attachedDevice.DeviceSerialNumber);
    }

    void rfid_Attach(object sender, Phidget22.Events.AttachEventArgs e)
    {
        RFID attachedDevice = ((RFID)sender);
        int deviceSerial = attachedDevice.DeviceSerialNumber;
        Debug.Log("Attached device " + attachedDevice.DeviceSerialNumber);
    }

    void digitalInput_StateChange(object sender, Phidget22.Events.DigitalInputStateChangeEventArgs e)
    {
       
            UnityMainThreadDispatcher.Instance().Enqueue(ThisWillBeExecutedOnTheMainThread(ScanButton.State));
        
    }

    private IEnumerator WaitTillEndAudio(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioPlaying = false;
    }

    public IEnumerator ThisWillBeExecutedOnTheMainThread(bool state)
    {
        Debug.Log("This is executed from the main thread");

        switch (state)
        {

            case true:                    //MicroscopeClickSource.PlayOneShot(MicroscopeClick);

                if (!audioPlaying)
                {
                    audioPlaying = true;
                    semSFX.clip = microScope;
                    semSFX.Play();
                    StartCoroutine(WaitTillEndAudio(microScope.length));
                    imageHolder.SetActive(false);
                    dataNumber.gameObject.SetActive(false);
                    scanning.SetActive(true);
                    noSampleScanned.SetActive(false);
                    introduction.SetActive(false);
                    yield return new WaitForSeconds(3.14f);
                    RFIDMicroscope.AntennaEnabled = true;
                    if (tagPresent)
                    {
                        Debug.Log("Tag scanned: " + RFIDMicroscope.GetLastTag().TagString);
                        foreach (string x in boneRFIDTagStings)
                        {
                            if (x.Equals(RFIDMicroscope.GetLastTag().TagString))
                            //microscopeSamples.Contains(new MicroscopeSamples(RFIDMicroscope.GetLastTag(), "bone")))
                            {
                                imageHolder.SetActive(true);
                                dataNumber.gameObject.SetActive(true);
                                scanCounter++;
                                dataNumber.text = MICROSCOPEDATAID + scanCounter.ToString("00");
                                scanning.SetActive(false);
                                microscopeScanImage.sprite = boneImage;
                                microscopeScanImage.SetNativeSize();
                                microscopeScanImage.rectTransform.localScale = new Vector3(1, 1, 1);
                                microscopeScanImage.rectTransform.localPosition = new Vector2(UnityEngine.Random.Range(-398, 398), UnityEngine.Random.Range(-522, 522));
                                yield return null;
                            }
                        }
                        foreach (string x in rockRFIDTagStings)
                        {
                            if (x.Equals(RFIDMicroscope.GetLastTag().TagString))
                            //microscopeSamples.Contains(new MicroscopeSamples(RFIDMicroscope.GetLastTag(), "rock")))
                            {
                                imageHolder.SetActive(true);
                                dataNumber.gameObject.SetActive(true);
                                scanCounter++;
                                dataNumber.text = MICROSCOPEDATAID + scanCounter.ToString("00");
                                scanning.SetActive(false);
                                microscopeScanImage.sprite = rockImage;
                                microscopeScanImage.SetNativeSize();
                                microscopeScanImage.rectTransform.localScale = new Vector3(2.5f, 2.5f, 1);
                                microscopeScanImage.rectTransform.localPosition = new Vector2(UnityEngine.Random.Range(-640, 640), UnityEngine.Random.Range(-500, 500));
                                yield return null;
                            }
                        }
                    }
                    else
                    {
                        imageHolder.SetActive(false);
                        dataNumber.gameObject.SetActive(false);
                        scanning.SetActive(false);
                        noSampleScanned.SetActive(true);
                    }
                }
                break;
            case false:
                    //RFIDMicroscope.AntennaEnabled = false;
                break;
        }
        yield return null;
    }
}
