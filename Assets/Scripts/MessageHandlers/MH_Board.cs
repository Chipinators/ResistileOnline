using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResistileClient;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MH_Board : MonoBehaviour, MessageHanderInterface {
    private int msgFromThread = -1;
    private ResistileMessage messageFromThread;


    // Use this for initialization
    void Start () {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().messageInterface = this;
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.gameLoaded, ""));
    }

    public void doAction(ResistileMessage message)
    {
        messageFromThread = message;
        switch (message.messageCode)
        {
            case ResistileMessageTypes.initializeGame:
                msgFromThread = message.messageCode;
                break;
            case ResistileMessageTypes.tilePlaced:
                msgFromThread = message.messageCode;
                break;
            case ResistileMessageTypes.drawResistor:
                msgFromThread = message.messageCode;
                break;
            case ResistileMessageTypes.drawWire:
                msgFromThread = message.messageCode;
                break;
            case ResistileMessageTypes.invalidMove:
                msgFromThread = message.messageCode;
                break;
            case ResistileMessageTypes.gameResults:
                msgFromThread = message.messageCode;
                break;
            case ResistileMessageTypes.replay:
                msgFromThread = message.messageCode;
                break;
            case ResistileMessageTypes.opponentQuit:
                msgFromThread = message.messageCode;
                break;
            default:
                Debug.Log("Unrecognized Message Type: " + message.messageCode + " --- " + message.message);
                break;

        }
    }

    void Update()
    {
        switch (msgFromThread)
        {
            case ResistileMessageTypes.initializeGame:
                initializeGame(messageFromThread);
                break;
            case ResistileMessageTypes.tilePlaced:
                tilePlaced(messageFromThread);
                break;
            case ResistileMessageTypes.drawResistor:
                drawResistor(messageFromThread);
                break;
            case ResistileMessageTypes.drawWire:
                drawWire(messageFromThread);
                break;
            case ResistileMessageTypes.invalidMove:
                invalidMove(messageFromThread);
                break;
            case ResistileMessageTypes.gameResults:
                gameResults(messageFromThread);
                break;
            case ResistileMessageTypes.replay:
                replay(messageFromThread);
                break;
            case ResistileMessageTypes.opponentQuit:
                opponentQuit(messageFromThread);
                break;
            default:
                break;
        }
        msgFromThread = -1;
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
        //GameHandler.gameHandler.DrawResistor(1, 2);
        GameHandler.gameHandler.changeTurn();
        GameHandler.gameHandler.setTurn();
    }

    private void drawWire(ResistileMessage message)
    {
        //TODO:
        //GameHandler.gameHandler.DrawWire(1);
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
        //GameHandler.gameHandler.DrawResistor(Random.Range(1,6), Random.Range(1, 3));
        //GameHandler.gameHandler.DrawWire(Random.Range(1, 4));
    }

    public void guessResistance()
    {

    }

}
