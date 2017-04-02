using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MH_Board : MonoBehaviour {

	// Use this for initialization
	void Start () {
        NetworkManager.networkManager.sendMessage(MessageType.gameLoaded, "Game Loaded");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void quitGame()
    {
        NetworkManager.networkManager.sendMessage(MessageType.quitGame, "Quit Game");
        LoadLevel.LoadScene("MainMenu");
    }

    public void endTurn()
    {
        //NetworkManager.networkManager.sendMessage(MessageType.endTurn, "End Turn");
        GameHandler.gameHandler.DrawResistor(Random.Range(1,6), Random.Range(1, 3));
        GameHandler.gameHandler.DrawWire(Random.Range(1, 4));
    }
}
