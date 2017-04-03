using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResistileClient;

public class MH_MainMenu : MonoBehaviour {
    public void startHosting()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.host, "StartHosting");
        LoadLevel.LoadScene("HostWaitingScreen");
    }

    public void serverBrowser()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.serverList, "serverList");
        LoadLevel.LoadScene("ServerBrowser");
    }

    public void tutorial()
    {
        LoadLevel.LoadScene("Tutorial");
        ping();
    }

    public void settings()
    {
        LoadLevel.LoadScene("SettingsMenu");
        ping();
    }

    public void ping()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.ping, "Ping");
    }

}
