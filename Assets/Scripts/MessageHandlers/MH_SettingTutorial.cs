using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MH_SettingTutorial : MonoBehaviour {

    public void MainMenu()
    {
        NetworkManager.networkManager.sendMessage(MessageType.ping, "Ping");
        LoadLevel.LoadScene("MainMenu");
    }
}
