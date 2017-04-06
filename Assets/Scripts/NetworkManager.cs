using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using System.Net;

public class NetworkManager : MonoBehaviour {
    public static NetworkManager networkManager;
    public TcpClient dest;
    public NetworkStream destStream;
    public int gameID;
    public string username;
    private static XmlSerializer serializer;
    public Thread readDataThread;
    public MessageHanderInterface messageInterface;

    // Use this for initialization
    void Start () {
        networkManager = this;
        serializer = new XmlSerializer(typeof(ResistileClient.ResistileMessage));
        dest = new TcpClient();
        
        try
        {
            //dest.Connect("127.0.0.1", 8888);
            dest.Connect(Dns.GetHostAddresses("witr90gme4p.wit.private"), 8888);
            Debug.Log("Client Socket Program - Server Connected ...");
        }
        catch (Exception e)
        {
            Debug.Log("Couldn't connect to server.\n" + e);
        }
        readDataThread = new Thread(receiveMessage);
        readDataThread.Start();
    }


    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    //public void sendMessage(ResistileClient.ResistileMessage message)
    //{
    //    var finalMsg = new ResistileClient.ResistileMessage(gameID, messageType, data);
    //    transmitMessage(message);
    //}

    public void sendMessage(ResistileClient.ResistileMessage message)
    {
        using (StringWriter textWriter = new StringWriter())
        {
            serializer.Serialize(textWriter, message);
            NetworkStream serverStream = dest.GetStream();
            byte[] outStream = Encoding.ASCII.GetBytes(textWriter.ToString() + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }
        
    }

    private void receiveMessage()
    {
        while (true)
        {
            NetworkStream serverStream = dest.GetStream();
            byte[] inStream = new byte[dest.ReceiveBufferSize];
            serverStream.Read(inStream, 0, dest.ReceiveBufferSize);
            string returndata = Encoding.ASCII.GetString(inStream);
            returndata = returndata.Substring(0, returndata.IndexOf("$"));
            var bytes = Encoding.ASCII.GetBytes(returndata);
            ResistileClient.ResistileMessage message;
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (StreamReader ms = new StreamReader(stream, Encoding.ASCII))
                {
                    message = (ResistileClient.ResistileMessage)serializer.Deserialize(ms);
                }
            }
            messageInterface.doAction(message);
            var dataFromClient = message.gameID + " " + message.messageCode + " " + message.message;
            Debug.Log("Data from Server : " + dataFromClient);
            serverStream.Flush();
        }
    }
}
