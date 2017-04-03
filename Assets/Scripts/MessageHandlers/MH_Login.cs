using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResistileClient;

public class MH_Login : MonoBehaviour {
    public Text inputField;

    public void login()
    {
        string username = inputField.text;
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.initialize, username);
        LoadLevel.LoadScene("MainMenu");
    }
}
