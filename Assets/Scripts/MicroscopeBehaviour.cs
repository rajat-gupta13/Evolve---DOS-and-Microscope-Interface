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


    public string[] boneRFIDTagStings;
    public string[] rockRFIDTagStings;

    public Image microscopeBackgroundImage;
    public Image microscopeScanImage;
    public GameObject imageHolder;
    public Text dataNumber;

    public Sprite startScreen;
    public Sprite sampleScanned;
    public Sprite noSampleScanned;
    public Sprite rockImage;
    public Sprite boneImage;

    private bool tagPresent = false;

    //private List<MicroscopeSamples> microscopeSamples;

    

    //public AudioClip MicroscopeClick;
    //public AudioClip MicroscopeBackground;
    //public AudioSource MicroscopeClickSource;
    //public AudioSource MicroscopeBackgroundSource;
    private int scanCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        /*
        microscopeSamples = new List<MicroscopeSamples>();
        for (int i = 0; i < boneRFIDTagStings.Length; i++)
        {
            RFIDTag tag = new RFIDTag();
            tag.TagString = boneRFIDTagStings[i];
            tag.Protocol = RFIDProtocol.EM4100;
            microscopeSamples.Add(new MicroscopeSamples(tag, "bone"));
        }
        for (int i = 0; i < rockRFIDTagStings.Length; i++)
        {
            RFIDTag tag = new RFIDTag();
            tag.TagString = rockRFIDTagStings[i];
            tag.Protocol = RFIDProtocol.EM4100;
            microscopeSamples.Add(new MicroscopeSamples(tag, "rock"));
        }
        */
        imageHolder.SetActive(false);
        dataNumber.gameObject.SetActive(false);
        microscopeBackgroundImage.sprite = startScreen;

        


        //MicroscopeBackgroundSource.Play();
 
        ScanButton.DeviceSerialNumber = 523574;
        ScanButton.Channel = 0;
        ScanButton.IsLocal = true;
        ScanButton.Attach += digitalInput_Attach;
        ScanButton.StateChange += digitalInput_StateChange;

        RFIDMicroscope.DeviceSerialNumber = 452966;
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

    public IEnumerator ThisWillBeExecutedOnTheMainThread(bool state)
    {
        Debug.Log("This is executed from the main thread");
        
        switch (state)
        {

            case true:                    //MicroscopeClickSource.PlayOneShot(MicroscopeClick);
                    
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
                            dataNumber.text = "CMNH042719_" + scanCounter.ToString("00");
                            microscopeBackgroundImage.sprite = sampleScanned;
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
                            dataNumber.text = "CMNH042719_" + scanCounter.ToString("00");
                            microscopeBackgroundImage.sprite = sampleScanned;
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
                    microscopeBackgroundImage.sprite = noSampleScanned;
                }
                break;
            case false:
                    //RFIDMicroscope.AntennaEnabled = false;
                break;
        }
        yield return null;
    }
}
