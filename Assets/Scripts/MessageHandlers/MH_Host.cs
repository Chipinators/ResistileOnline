using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResistileClient;
using System;
using UnityEngine.SceneManagement;

public class MH_Host : MonoBehaviour, MessageHanderInterface {
    public GameObject panelManager;

    void Start()
    {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().messageHandler = this.gameObject;
        panelManager = GameObject.FindGameObjectWithTag("PanelManager");
    }

    public void doAction(ResistileMessage message)
    {
        switch (message.messageCode)
        {
            case ResistileMessageTypes.opponentFound:
                opponentFound(message);
                break;
            case ResistileMessageTypes.opponentCanceled:
                opponentCanceled(message);
                break;
            case ResistileMessageTypes.startGame:
                startGame(message);
                break;
            default:
                Debug.Log("Unrecognized Message Type: " + message.messageCode + " --- " + message.message);
                break;
        }
    }

    //RECEIVE MESSAGES FROM SERVER
    private void opponentFound(ResistileMessage message)
    {
        panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting = false;
    }

    private void opponentCanceled(ResistileMessage message)
    {
        panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting = true;
    }

    private void startGame(ResistileMessage message)
    {
        SceneManager.LoadScene("Board");
    }

    //SEND MESSAGES TO SERVER
    public void declineOpponent()
    {
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.declineOpponent, ""));
        panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting = true;
    }

    public void cancelSearch()
    {
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.cancelSearch, ""));
        SceneManager.LoadScene("MainMenu");
    }

    public void acceptOpponent()
    {
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.acceptOpponent, ""));
        SceneManager.LoadScene("Board");
    }


}
