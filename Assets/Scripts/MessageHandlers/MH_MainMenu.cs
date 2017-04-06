using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResistileClient;
using UnityEngine.SceneManagement;

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
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.startHosting, NetworkManager.networkManager.username));
        SceneManager.LoadScene("HostWaitingScreen");
    }

    public void serverBrowser()
    {
        SceneManager.LoadScene("ServerBrowser");
    }

    public void tutorial()
    {
        ping();
        SceneManager.LoadScene("Tutorial");
    }

    public void settings()
    {
        ping();
        SceneManager.LoadScene("SettingsMenu");
    }

    private void ping()
    {
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.ping, ""));
    }

}
