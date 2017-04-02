using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;

public class NetworkManager : MonoBehaviour {
    public static NetworkManager networkManager;
    public TcpClient dest;
    public NetworkStream destStream;
    public int gameID;

	// Use this for initialization
	void Start () {
        networkManager = this;
        dest = new TcpClient();
        try
        {
            dest.Connect("127.0.0.1", 8888);
            Debug.Log("Client Socket Program - Server Connected ...");
        }
        catch (Exception e)
        {
            Debug.Log("Couldn't connect to server.\n" + e);
        }
    }
	
	// Update is called once per frame
	void Update () {
        //receiveMessage();
	}

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void sendMessage(int messageType, string data)
    {
        string finalMsg = gameID + "/////" + messageType + "/////" + data;
        Debug.Log("MESSAGE SENT = " + finalMsg);
        transmitMessage(finalMsg);
    }

    private void transmitMessage(string message)
    {
        NetworkStream serverStream = dest.GetStream();
        byte[] outStream = Encoding.ASCII.GetBytes(message + "$");
        serverStream.Write(outStream, 0, outStream.Length);
        serverStream.Flush();
    }

    private void receiveMessage()
    {
        NetworkStream serverStream = dest.GetStream();
        byte[] inStream = new byte[dest.ReceiveBufferSize];
        serverStream.Read(inStream, 0, dest.ReceiveBufferSize);
        string returndata = Encoding.ASCII.GetString(inStream);
        returndata = returndata.Substring(0, returndata.IndexOf("$"));
        serverStream.Flush();
        parseMessage( returndata);
    }

    private void parseMessage(string message)
    {
       
    }
}
