using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResistileClient;
using UnityEngine.SceneManagement;
using System;

public class MH_MainMenu : MonoBehaviour, MessageHanderInterface {
    void Start()
    {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().messageInterface = this;
    }

    public void doAction(ResistileMessage message)
    {
        switch (message.messageCode)
        {
            case ResistileMessageTypes.ping:
                Debug.Log("Received Ping From Server");
                break;
            default:
                Debug.Log("Unrecognized Message Type: " + message.messageCode + " --- " + message.message);
                break;
        }
    }

    //RECEIVE MESSAGES FROM SERVER

    //SEND MESSAGES TO SERVER
    public void startHosting()
    {
        if(NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.startHosting, NetworkManager.networkManager.username))) {
            SceneManager.LoadScene("HostWaitingScreen");
        }
        
    }

    public void serverBrowser()
    {
        if(ping())
            SceneManager.LoadScene("ServerBrowser");
    }

    public void tutorial()
    {
        if (ping())
            SceneManager.LoadScene("Tutorial");
    }

    public void settings()
    {
        if (ping())
            SceneManager.LoadScene("SettingsMenu");
    }

    private bool ping()
    {
        return NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.ping, ""));
    }

}
