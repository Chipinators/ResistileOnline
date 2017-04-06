using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResistileClient;
using UnityEngine.SceneManagement;

public class MH_Login : MonoBehaviour, MessageHanderInterface {
    public Text inputField;
    public Button submitButton;
    private System.Object thisLock = new System.Object();
    void Start()
    {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().messageInterface = this;
    }

    void Update()
    {
        lock (thisLock)
        {
            if (inputField.text == "")
            {
                submitButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                submitButton.GetComponent<Button>().interactable = true;
            }
        }
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
    public void login()
    {
        string username = inputField.text;
        NetworkManager.networkManager.username = username;
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.login, username));
        SceneManager.LoadScene("MainMenu");
    }
}
