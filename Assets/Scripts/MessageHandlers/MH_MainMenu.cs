using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResistileClient;

public class MH_MainMenu : MonoBehaviour, MessageHanderInterface {

    void Start()
    {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().messageHandler = this.gameObject;
    }

    public void doAction(ResistileMessage message)
    {
        switch (message.messageCode)
        {
            default: break;
        }
    }

    //RECEIVE MESSAGES FROM SERVER

    //SEND MESSAGES TO SERVER
    public void startHosting()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.host, "StartHosting");
        LoadLevel.LoadScene("HostWaitingScreen");
    }

    public void serverBrowser()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.serverList, "serverList");
        LoadLevel.LoadScene("ServerBrowser");
    }

    public void tutorial()
    {
        ping();
        LoadLevel.LoadScene("Tutorial");
    }

    public void settings()
    {
        ping();
        LoadLevel.LoadScene("SettingsMenu");
    }

    public void ping()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.ping, "Ping");
    }

}
