using UnityEngine;
using System.Collections;

using System.Net.Sockets;
using System.IO;
using System;

public class CrateOpener : MonoBehaviour
{
    bool socketReady1 = false;                // global variables are setup here
    TcpClient mySocket1;
    public NetworkStream theStream1;
    StreamWriter theWriter1;
    StreamReader theReader1;
    public String host1 = "192.168.0.151";
    public Int32 port1 = 50001;

    bool socketReady2 = false;                // global variables are setup here
    TcpClient mySocket2;
    public NetworkStream theStream2;
    StreamWriter theWriter2;
    StreamReader theReader2;
    public String host2 = "192.168.0.206";
    public Int32 port2 = 50002;

    bool socketReady3 = false;                // global variables are setup here
    TcpClient mySocket3;
    public NetworkStream theStream3;
    StreamWriter theWriter3;
    StreamReader theReader3;
    public String host3 = "192.168.0.230";
    public Int32 port3 = 50003;


    void Start()
    {
        SetupSocket();                        // setup the server connection when the program starts
    }


    public void WriteSocket(string theLine)
    {            // function to write data out
        if (!socketReady1)
            return;
        String tmpString1 = theLine;
        theWriter1.Write(tmpString1);
        theWriter1.Flush();

        if (!socketReady2)
            return;
        String tmpString2 = theLine;
        theWriter2.Write(tmpString2);
        theWriter2.Flush();

        if (!socketReady3)
            return;
        String tmpString3 = theLine;
        theWriter3.Write(tmpString3);
        theWriter3.Flush();


    }

    public String ReadSocket()
    {                        // function to read data in
        if (!socketReady1)
            return "";
        if (theStream1.DataAvailable)
            return theReader1.ReadLine();
        if (!socketReady2)
            return "";
        if (theStream2.DataAvailable)
            return theReader2.ReadLine();
        if (!socketReady3)
            return "";
        if (theStream3.DataAvailable)
            return theReader3.ReadLine();
        return "NoData";
    }

    public void CloseSocket()
    {                            // function to close the socket
        if (!socketReady1)
            return;
        theWriter1.Close();
        theReader1.Close();
        mySocket1.Close();
        socketReady1 = false;

        if (!socketReady2)
            return;
        theWriter2.Close();
        theReader2.Close();
        mySocket2.Close();
        socketReady2 = false;

        if (!socketReady3)
            return;
        theWriter3.Close();
        theReader3.Close();
        mySocket3.Close();
        socketReady3 = false;
    }

    public void MaintainConnection()
    {                    // function to maintain the connection (not sure why! but Im sure it will become a solution to a problem at somestage)
        if (!theStream1.CanRead)
        {
            SetupSocket();
        }
        if (!theStream2.CanRead)
        {
            SetupSocket();
        }
        if (!theStream3.CanRead)
        {
            SetupSocket();
        }
    }

    // Update is called once per frame
    void SetupSocket()
    {

        try
        {
            mySocket1 = new TcpClient(host1, port1);
            theStream1 = mySocket1.GetStream();
            theWriter1 = new StreamWriter(theStream1);
            theReader1 = new StreamReader(theStream1);
            socketReady1 = true;

            mySocket2 = new TcpClient(host2, port2);
            theStream2 = mySocket2.GetStream();
            theWriter2 = new StreamWriter(theStream2);
            theReader2 = new StreamReader(theStream2);
            socketReady2 = true;

            mySocket3 = new TcpClient(host3, port3);
            theStream3 = mySocket3.GetStream();
            theWriter3 = new StreamWriter(theStream3);
            theReader3 = new StreamReader(theStream3);
            socketReady3 = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error:" + e);
        }
    }

    void Update()
    {
        
    }

    public void OpenCrate1()
    {
        if (!socketReady1)
            return;
        theWriter1.Write("open crate 1");
        theWriter1.Flush();
    }

    public void OpenCrate2()
    {
        if (!socketReady2)
            return;
        theWriter2.Write("open crate 2");
        theWriter2.Flush();
    }

    public void OpenCrate3()
    {
        if (!socketReady3)
            return;
        theWriter3.Write("open crate 3");
        theWriter3.Flush();
    }
}
