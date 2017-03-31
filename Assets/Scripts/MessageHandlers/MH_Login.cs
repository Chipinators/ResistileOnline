using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MH_Login : MonoBehaviour {
    public Text inputField;

    public void login()
    {
        Debug.Log("OKAY BUTTON PRESSED");
        string username = inputField.text;
        Debug.Log("Username = " + username);
        NetworkManager.networkManager.sendMessage(MessageType.initialize, username);
        LoadLevel.LoadScene("MainMenu");
    }
}
