using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResistileClient;

public class MH_Host : MonoBehaviour {
    public GameObject panelManager;

    void Start()
    {
        panelManager = GameObject.FindGameObjectWithTag("PanelManager");
    }

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
