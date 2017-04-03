using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResistileClient;

public class MH_SettingTutorial : MonoBehaviour {

    public void MainMenu()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.ping, "Ping");
        LoadLevel.LoadScene("MainMenu");
    }
}
