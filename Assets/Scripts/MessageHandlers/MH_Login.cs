using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResistileClient;
using UnityEngine.SceneManagement;

public class MH_Login : MonoBehaviour, MessageHanderInterface {
    private int msgFromThread = -1;
    private ResistileMessage messageFromThread;
    public Text inputField;
    public Button submitButton;
    public GameObject alertPanel;
    public Alert alert;
    private System.Object thisLock = new System.Object();
    void Start()
    {
        alert = alertPanel.GetComponent<Alert>();
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().messageInterface = this;
    }

    public void doAction(ResistileMessage message)
    {
        messageFromThread = message;
        lock (thisLock)
        {
            switch (message.messageCode)
            {
                case ResistileMessageTypes.ping:
                    Debug.Log("Received Ping From Server");
                    break;
                case ResistileMessageTypes.login:
                    msgFromThread = message.messageCode;
                    break;
                default:
                    Debug.Log("Unrecognized Message Type: " + message.messageCode + " --- " + message.message);
                    break;
            }
        }
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
            if (msgFromThread == ResistileMessageTypes.login)
            {
                login(messageFromThread);
            }
            msgFromThread = -1;
        }
    }

    //RECEIVE MESSAGES FROM SERVER
    public void login(ResistileMessage message)
    {
        Debug.Log("Received Login Message, isValid = " + message.turn);
        if (message.turn)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            alert.alert("Username Unavailable.",3.0f, true);
        }
    }

    //SEND MESSAGES TO SERVER
    public void login()
    {
        string username = inputField.text;
        NetworkManager.networkManager.username = username;
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.login, username));
    }
}
