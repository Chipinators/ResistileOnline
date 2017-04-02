using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MH_ServerBrowser : MonoBehaviour {
    public GameObject panelManager;

    void Start()
    {
        panelManager = GameObject.FindGameObjectWithTag("PanelManager");
    }

    public void getHostList()
    {
        NetworkManager.networkManager.sendMessage(MessageType.hostList, "HostList");
        LoadLevel.LoadScene("HostWaitingScene");
    }

    public void joinLobby()
    {
        NetworkManager.networkManager.sendMessage(MessageType.guestRequest, "guestRequest");
        panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting = false;
    }

    public void cancelRequest()
    {
        NetworkManager.networkManager.sendMessage(MessageType.cancelJoin, "CancelJoin");
        panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting = true;
    }

    public void goBack()
    {
        if (panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting) ping();
        else NetworkManager.networkManager.sendMessage(MessageType.cancelJoin, "CancelJoin");
        LoadLevel.LoadScene("MainMenu");
    }

    public void ping()
    {
        NetworkManager.networkManager.sendMessage(MessageType.ping, "Ping");
    }
}
