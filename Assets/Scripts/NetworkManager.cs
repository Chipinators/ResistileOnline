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
using ResistileClient;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour {
    public static NetworkManager networkManager = null;
    public TcpClient dest;
    public NetworkStream destStream;
    public int gameID;
    public string username;
    private static XmlSerializer serializer;
    public Thread readDataThread, serverConnectionThread;
    public MessageHanderInterface messageInterface;
    public string opponent;
    public bool runNetworkThread = true, quitGame = false, searchingForServer = true, serverFound = false;
    public float refreshTimer;

    // Use this for initialization
    void Start () {
        networkManager = this;
        serializer = new XmlSerializer(typeof(ResistileMessage));
        dest = new TcpClient();

        serverConnectionThread = new Thread(connectToServer);
        serverConnectionThread.IsBackground = true;
        serverConnectionThread.Start();
    }

    void OnApplicationQuit()
    {
        runNetworkThread = false;
        quitGame = true;
        searchingForServer = false;
        if (!Application.isEditor) System.Diagnostics.Process.GetCurrentProcess().Kill();


    }

    void onDestroy()
    {
        runNetworkThread = false;
        quitGame = true;
        searchingForServer = false;
    }

    void Update()
    {
        if (quitGame)
        {
            if (!Application.isEditor) System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        if (searchingForServer)
        {
            try
            {
                GameObject.Find("AlertPanel").GetComponent<Alert>().alert("Server Is Offline", refreshTimer + 1.0f, true);
            }
            catch (Exception e2)
            {
            }
            refreshTimer -= Time.deltaTime;
        }
        else
        {
            if (serverFound)
            {
                GameObject.Find("AlertPanel").GetComponent<Alert>().alert("Connected to Server", 2.0f, false);
                serverFound = false;
            }
        }
    }

    void Awake()
    {
        //if (networkManager == null)
        //    networkManager = this;
        //else if (networkManager != this)
        //    Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    public void connectToServer()
    {
        while (searchingForServer)
        {
            if (refreshTimer <= 0)
            {
                try
                {
                    refreshTimer = 5.0f;
                    dest.Connect("127.0.0.1", 8888);
                    //dest.Connect(Dns.GetHostAddresses("witr90gme4p.wit.private"), 8888);
                    Debug.Log("Client Socket Program - Server Connected ...");

                    readDataThread = new Thread(receiveMessage);
                    readDataThread.IsBackground = true;
                    readDataThread.Start();
                    serverFound = true;
                    searchingForServer = false;
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    searchingForServer = true;
                    serverFound = false;
                    Debug.Log("Couldn't connect to server.\n" + e);
                }
            }
            else
            {
                
            }
        }
    }

    public bool sendMessage(ResistileMessage message)
    {
        using (StringWriter textWriter = new StringWriter())
        {
            serializer.Serialize(textWriter, message);
            NetworkStream serverStream = dest.GetStream();
            byte[] outStream = Encoding.ASCII.GetBytes(textWriter.ToString() + "$");
            try {
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                return true;
            }
            catch (Exception e)
            {
                if (!quitGame)
                {
                    SceneManager.LoadScene("LoginScreen");
                    networkManager.searchingForServer = true;
                    DestroyImmediate(gameObject);
                    //DestroyImmediate(GameObject.Find("AudioManager"));
                }
                else
                {
                    if (!Application.isEditor) System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                runNetworkThread = false;
                return false;
            }
        }
        
    }

    private void receiveMessage()
    {
        while (runNetworkThread)
        {
            NetworkStream serverStream = dest.GetStream();
            byte[] inStream = new byte[dest.ReceiveBufferSize];
            serverStream.Read(inStream, 0, dest.ReceiveBufferSize);
            string returndata = Encoding.ASCII.GetString(inStream);
            returndata = returndata.Substring(0, returndata.IndexOf("$"));
            var bytes = Encoding.ASCII.GetBytes(returndata);
            ResistileMessage message;
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (StreamReader ms = new StreamReader(stream, Encoding.ASCII))
                {
                    message = (ResistileMessage)serializer.Deserialize(ms);
                }
            }
            messageInterface.doAction(message);
            var dataFromClient = message.gameID + " " + message.messageCode + " " + message.message;
            Debug.Log("Data from Server : " + dataFromClient);
            serverStream.Flush();
        }
    }
}
