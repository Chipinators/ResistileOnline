using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResistileClient;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MH_SettingTutorial : MonoBehaviour, MessageHanderInterface {
    public GameObject alertPanel;
    public Alert alert;
    private int msgFromThread = -1;
    private ResistileMessage messageFromThread;
    private System.Object thisLock = new System.Object();

    void Start()
    {
        if(alertPanel) alert = alertPanel.GetComponent<Alert>();
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().messageInterface = this;
    }

    public void doAction(ResistileMessage message)
    {
        messageFromThread = message;
        lock (thisLock)
        {
            switch (message.messageCode)
            {
                case (ResistileMessageTypes.login):
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
        if (message.turn)
        {
            NetworkManager.networkManager.username = gameObject.GetComponent<Settings>().username;
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            alert.alert("Username Unavailable.", 3.0f, true);
        }
    }

    //SEND MESSAGES TO SERVER
    public void exitSettings()
    {
        if (ping())
        {
            if (GetComponent<Settings>().nameChange)
            {
                NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.login, GetComponent<Settings>().usernameField.transform.FindChild("Text").GetComponent<Text>().text));
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        else
        {
            SceneManager.LoadScene("LoginScreen");
        }
        
    }

    public void exitTutorial()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private bool ping()
    {
        return NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.ping, ""));
    }
}
