using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResistileClient;

public class MH_Board : MonoBehaviour {

	// Use this for initialization
	void Start () {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.gameLoaded, "Game Loaded");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void quitGame()
    {
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.quitGame, "Quit Game");
        LoadLevel.LoadScene("MainMenu");
    }

    public void endTurn()
    {
        //NetworkManager.networkManager.sendMessage(MessageType.endTurn, "End Turn");
        GameHandler.gameHandler.DrawResistor(Random.Range(1,6), Random.Range(1, 3));
        GameHandler.gameHandler.DrawWire(Random.Range(1, 4));
    }
}
