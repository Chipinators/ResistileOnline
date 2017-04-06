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
        NetworkManager.networkManager.readDataThread.Abort();

#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void declineQuit()
    {
        exitGamePanel.SetActive(false);
    }
}
