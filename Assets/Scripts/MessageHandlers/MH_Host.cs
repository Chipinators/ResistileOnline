using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResistileClient;
using System;

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
                opponentFound(message.gameID, message.message);
                break;
            case ResistileMessageTypes.opponentCanceled:
                opponentCanceled(message.gameID, message.message);
                break;
            case ResistileMessageTypes.startGame:
                break;
            default: break;
        }
    }

    //RECEIVE MESSAGES FROM SERVER
    private void opponentCanceled(int gameID, string data)
    {
        throw new NotImplementedException();
    }

    private void opponentFound(int gameID, string data)
    {
        throw new NotImplementedException();
    }

    //SEND MESSAGES TO SERVER
    public void decline()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.hostDecline, "HostDecline");
        panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting = true;
    }

    public void cancelSearch()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.cancelHost, "HostCancel");
        LoadLevel.LoadScene("MainMenu");
    }

    public void accept()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.startGame, "HostAccept");
        LoadLevel.LoadScene("Board");
    }


}
