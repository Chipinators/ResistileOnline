using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResistileClient;

public class MH_ServerBrowser : MonoBehaviour {
    public GameObject panelManager;

    void Start()
    {
        panelManager = GameObject.FindGameObjectWithTag("PanelManager");
    }

    public void getHostList()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.hostList, "HostList");
        LoadLevel.LoadScene("HostWaitingScene");
    }

    public void joinLobby()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.guestRequest, "guestRequest");
        panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting = false;
    }

    public void cancelRequest()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.cancelJoin, "CancelJoin");
        panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting = true;
    }

    public void goBack()
    {
        if (panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting) ping();
        else NetworkManager.networkManager.sendMessage(ResistileMessageTypes.cancelJoin, "CancelJoin");
        LoadLevel.LoadScene("MainMenu");
    }

    public void ping()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.ping, "Ping");
    }
}
