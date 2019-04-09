using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DOSBehaviour : MonoBehaviour
{
    public InputField command;
    public GameObject dosTextPrefab;
    public Transform contentParentContainer;
    public ScrollRect scrollRect;

    public string OPEN = "open ";
    public string ENTER = "enter ";
    public string ACCESS = "access ";
    
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

    private int commandCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        command.Select();
        command.ActivateInputField();
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
            if (command.text == "help")
            {
                string commandList = "\nList of commands:\nopen <ID # of object to be opened>\tOpens the object with the specified ID number" +
                        "\nenter <Data # on evaluation conducted>\tEnters the data evaluated from the unique data number entered\n" +
                        "create-report\tCreates a report with the currently uploaded research files" +
                        "\naccess <Access codes>\tAccesses the object with the respective access codes";
               
                GameObject dosText = CreateDOSText();
                dosText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 180);
                dosText.GetComponent<Text>().text = "> " + command.text + commandList;
            }
            else if (command.text.StartsWith(OPEN))
            {
                string id = command.text.Replace(OPEN, "");
                string unlocked = "\nCrate " + id + " unlocked.";
                if (id.Length > 0)
                {
                    if (string.Compare(id, CRATE1, true) == 0)
                    {
                        GameObject dosText = CreateDOSText();     
                        dosText.GetComponent<Text>().text = "> " + command.text + unlocked;
                    }
                    else if ((string.Compare(id, CRATE2, true) == 0) && !crate2Access)
                    { 
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = "> " + command.text + "\nAccess Denied";
                    }
                    else if ((string.Compare(id, CRATE2, true) == 0) && crate2Access)
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = "> " + command.text + unlocked;
                    }
                    else if ((string.Compare(id, CRATE3, true) == 0) && (!crate3Access))
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = "> " + command.text + "\nAccess Denied.";
                    }
                    else if ((string.Compare(id, CRATE3, true) == 0) && (crate3Access))
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = "> " + command.text + unlocked;
                    }
                    else
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = "> " + command.text + "\nPlease enter a valid id";
                    }
                }
            }
            else if (command.text.StartsWith(ACCESS))
            {
                string id = command.text.Replace(ACCESS, "");
                string unlocked = "\nAccess codes: " + id + " accepted.";
                if (id.Length > 0)
                {
                    if (string.Compare(id, CRATE2ACCESSCODES, true) == 0)
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = "> " + command.text + unlocked;
                     
                        crate2Access = true;
                    }
                    else if (string.Compare(id, CRATE3ACCESSCODES, true) == 0)
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = "> " + command.text + unlocked;
                       
                        crate3Access = true;
                    }
                    else
                    {
                        GameObject dosText = CreateDOSText();
                        dosText.GetComponent<Text>().text = "> " + command.text + "\nPlease enter a valid access code";
                    }
                }

            }
            else
            {
                GameObject dosText = CreateDOSText();
                dosText.GetComponent<Text>().text = "> " + command.text + "\nInvalid Command. Please use \"help\" to get a list of valid commands";
            }
            command.Select();
            command.text = "";
            command.ActivateInputField();
        }
    }
}
