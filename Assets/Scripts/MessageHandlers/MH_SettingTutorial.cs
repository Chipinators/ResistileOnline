using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResistileClient;

public class MH_SettingTutorial : MonoBehaviour, MessageHanderInterface {

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
    public void MainMenu()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.ping, "Ping");
        LoadLevel.LoadScene("MainMenu");
    }
}
