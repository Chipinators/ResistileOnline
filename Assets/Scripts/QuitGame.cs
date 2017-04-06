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
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.applicationQuit, NetworkManager.networkManager.username));
        NetworkManager.networkManager.destStream.Close();
        NetworkManager.networkManager.dest.Close();
        Application.Quit();
    }

    public void declineQuit()
    {
        exitGamePanel.SetActive(false);
    }
}
