using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResistileClient;
using UnityEngine.SceneManagement;

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
            default:
                Debug.Log("Unrecognized Message Type: " + message.messageCode + " --- " + message.message);
                break;

        }
    }

    //RECEIVE MESSAGES FROM SERVER

    //SEND MESSAGES TO SERVER
    public void login()
    {
        string username = inputField.text;
        NetworkManager.networkManager.username = username;
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.login, username));
        SceneManager.LoadScene("MainMenu");
    }
}
