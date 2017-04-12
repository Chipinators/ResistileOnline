using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResistileClient;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MH_SettingTutorial : MonoBehaviour, MessageHanderInterface {
    void Start()
    {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().messageInterface = this;
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
    public void MainMenu()
    {
        ping();
        if(GetComponent<Settings>().nameChange) NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.login, GetComponent<Settings>().usernameField.transform.FindChild("Text").GetComponent<Text>().text));
        SceneManager.LoadScene("MainMenu");
    }

    private void ping()
    {
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.ping, ""));
    }
}
