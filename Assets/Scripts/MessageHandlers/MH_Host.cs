using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResistileClient;
using System;
using UnityEngine.SceneManagement;

public class MH_Host : MonoBehaviour, MessageHanderInterface {
    public GameObject panelManager, opponentName;
    private int msgFromThread = -1;
    private ResistileMessage messageFromThread;
    private System.Object thisLock = new System.Object();

    void Start()
    {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().messageInterface = this;
        panelManager = GameObject.FindGameObjectWithTag("PanelManager");
    }

    public void doAction(ResistileMessage message)
    {
        messageFromThread = message;
        switch (message.messageCode)
        {
            case ResistileMessageTypes.opponentFound:
                msgFromThread = message.messageCode;
                break;
            case ResistileMessageTypes.opponentCanceled:
                msgFromThread = message.messageCode;
                break;
            case ResistileMessageTypes.startGame:
                msgFromThread = message.messageCode;
                break;
            default:
                Debug.Log("Unrecognized Message Type: " + message.messageCode + " --- " + message.message);
                break;
        }
    }

    void Update()
    {
        if (msgFromThread == ResistileMessageTypes.opponentFound)
            opponentFound(messageFromThread);
        else if (msgFromThread == ResistileMessageTypes.opponentCanceled)
        {
            opponentCanceled(messageFromThread);
        }
        else if (msgFromThread == ResistileMessageTypes.startGame)
        {
            startGame(messageFromThread);
        }
        msgFromThread = -1;
    }

    //RECEIVE MESSAGES FROM SERVER
    private void opponentFound(ResistileMessage message)
    {
        panelManager.GetComponent<HostScreenPanelAdapter>().changeWaiting();
        opponentName.GetComponent<Text>().text = message.message;
    }

    private void opponentCanceled(ResistileMessage message)
    {
        panelManager.GetComponent<HostScreenPanelAdapter>().changeWaiting();
    }

    private void startGame(ResistileMessage message)
    {
        SceneManager.LoadScene("Board");
    }

    //SEND MESSAGES TO SERVER
    public void declineOpponent()
    {
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.declineOpponent, ""));
        panelManager.GetComponent<HostScreenPanelAdapter>().changeWaiting();
    }

    public void cancelSearch()
    {
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.cancelSearch, NetworkManager.networkManager.username));
        SceneManager.LoadScene("MainMenu");
    }

    public void acceptOpponent()
    {
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.acceptOpponent, opponentName.GetComponent<Text>().text));
        SceneManager.LoadScene("Board");
    }


}
