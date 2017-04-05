using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResistileClient;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MH_Board : MonoBehaviour, MessageHanderInterface {

	// Use this for initialization
	void Start () {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().messageHandler = this.gameObject;
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.gameLoaded, ""));
    }

    public void doAction(ResistileMessage message)
    {
        switch (message.messageCode)
        {
            case ResistileMessageTypes.initializeGame:
                initializeGame(message);
                break;
            case ResistileMessageTypes.tilePlaced:
                tilePlaced(message);
                break;
            case ResistileMessageTypes.drawResistor:
                drawResistor(message);
                break;
            case ResistileMessageTypes.drawWire:
                drawWire(message);
                break;
            case ResistileMessageTypes.invalidMove:
                invalidMove(message);
                break;
            case ResistileMessageTypes.gameResults:
                gameResults(message);
                break;
            case ResistileMessageTypes.replay:
                replay(message);
                break;
            case ResistileMessageTypes.opponentQuit:
                opponentQuit(message);
                break;
            default:
                Debug.Log("Unrecognized Message Type: " + message.messageCode + " --- " + message.message);
                break;

        }
    }

    //RECEIVE MESSAGES FROM SERVER
    private void initializeGame(ResistileMessage message)
    {
        //TODO: initialize hands, player, etc.

        GameHandler.gameHandler.setTurn();
    }

    private void tilePlaced(ResistileMessage message) //Opponent Ends Turn
    {
        //TODO: handleTile
        GameHandler.gameHandler.changeTurn();
        GameHandler.gameHandler.setTurn();
    }

    private void drawResistor(ResistileMessage message) //You End Turn
    {
        //TODO:
        GameHandler.gameHandler.DrawResistor(1, 2);
        GameHandler.gameHandler.changeTurn();
        GameHandler.gameHandler.setTurn();
    }

    private void drawWire(ResistileMessage message)
    {
        //TODO:
        GameHandler.gameHandler.DrawWire(1);
        GameHandler.gameHandler.changeTurn();
        GameHandler.gameHandler.setTurn();
    }

    private void invalidMove(ResistileMessage message)
    {
        GameHandler.gameHandler.alert("Not OKAY!");
    }

    private void gameResults(ResistileMessage message)
    {
        //TODO:
        //GameHandler.gameHandler.gameOver(isWinner, pScore, s1Score, s2Score, gScore, tScore);
    }

    private void replay(ResistileMessage message)
    {
        if(message.message == "true")
        {
            SceneManager.LoadScene("Board");
        }
        else
        {
            GameHandler.gameHandler.noPlayAgain();
        }
    }

    private void opponentQuit(ResistileMessage message)
    {
        GameHandler.gameHandler.alert("Opponent Left The Game, Returning To Main Menu");
        StartCoroutine(backToMainMenu());
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator backToMainMenu()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("MainMenu");
    }

    //SEND MESSAGES TO SERVER
    public void quitGame()
    {
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.quitGame, ""));
        SceneManager.LoadScene("MainMenu");
    }

    public void endTurn()
    {
        //TODO: Send server tile, coordinates, 
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.endTurn, ""));
        GameHandler.gameHandler.DrawResistor(Random.Range(1,6), Random.Range(1, 3));
        GameHandler.gameHandler.DrawWire(Random.Range(1, 4));
    }

    public void guessResistance()
    {

    }

}
