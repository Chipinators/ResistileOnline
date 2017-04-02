using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MH_Host : MonoBehaviour {
    public GameObject panelManager;

    void Start()
    {
        panelManager = GameObject.FindGameObjectWithTag("PanelManager");
    }

    public void decline()
    {
        NetworkManager.networkManager.sendMessage(MessageType.hostDecline, "HostDecline");
        panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting = true;
    }

    public void cancelSearch()
    {
        NetworkManager.networkManager.sendMessage(MessageType.cancelHost, "HostCancel");
        LoadLevel.LoadScene("MainMenu");
    }

    public void accept()
    {
        NetworkManager.networkManager.sendMessage(MessageType.startGame, "HostAccept");
        LoadLevel.LoadScene("Board");
    }
}
