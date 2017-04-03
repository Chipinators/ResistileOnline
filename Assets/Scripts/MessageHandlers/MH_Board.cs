using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResistileClient;

public class MH_Board : MonoBehaviour, MessageHanderInterface {

	// Use this for initialization
	void Start () {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().messageHandler = this.gameObject;
        NetworkManager.networkManager.sendMessage(ResistileMessageTypes.gameLoaded, "Game Loaded");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void doAction(ResistileMessage message)
    {
        switch (message.messageCode)
        {
            case ResistileMessageTypes.initialize:
                break;
            case ResistileMessageTypes.tilePlaced:
                break;
            case ResistileMessageTypes.draw:
                break;
            case ResistileMessageTypes.gameOver:
                break;
            case ResistileMessageTypes.validMoves:
                break;
            case ResistileMessageTypes.gameResults:
                break;
            case ResistileMessageTypes.replay:
                break;
            default: break;

        }
    }

    //RECEIVE MESSAGES FROM SERVER

    //SEND MESSAGES TO SERVER
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

    public void guessResistance()
    {

    }

}
