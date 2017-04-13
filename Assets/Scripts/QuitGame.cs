using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResistileClient;

public class QuitGame : MonoBehaviour {
    public GameObject exitGamePanel;

    public void popupQuit()
    {
        exitGamePanel.SetActive(true);
    }

    public void quitGame()
    {
        NetworkManager.networkManager.runNetworkThread = false;
        NetworkManager.networkManager.quitGame = true;
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.applicationQuit, NetworkManager.networkManager.username));
    }

    public void declineQuit()
    {
        exitGamePanel.SetActive(false);
    }

}
