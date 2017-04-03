using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResistileClient;

public class MH_Login : MonoBehaviour, MessageHanderInterface {
    public Text inputField;

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
    public void login()
    {
        string username = inputField.text;
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.login, username);
        LoadLevel.LoadScene("MainMenu");
    }
}
