﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class GameHandler : MonoBehaviour {
    public static GameHandler gameHandler;
    public GameObject headerText;
    public GameObject resHand, resDeck, wireHand, wireDeck, gameBoard;
    public GameObject tilePrefab;
    public Button endTurn;
    public Sprite solder, resI, resII, wireI, wireII, wireIII;
    public GameObject primaryObj, secondaryObjI, secondaryObjII;
    public GameObject gameOverPanelHeader, primaryScore, secondary1Score, secondary2Score, guessScore, totalScore, starPanel, yellowStar, blueStar;
    public Sprite winTrophy, loseTrophy;
    public GameObject alertPanel, alertText;
    public Button playAgain;
    public string yourName, opponentName;
    public GameObject currentTile, solderTile;
    public int gameID;

    private Dictionary<int, string> secondaryObjs;
    private Dictionary<int, ResistileServer.GameTile> tileLookup = (new ResistileServer.DeckManager()).allTiles;
    private bool isTurn;
    private float alertTimer;

    void Start()
    {
        gameHandler = this;
        fillObjectives();
        Draw(5);
        alertTimer = 0.0f;
    }

    void Update()
    {
        if (alertTimer > 0)
        {
            alertTimer -= Time.deltaTime;
            alertPanel.SetActive(true);
        }
        if (alertTimer < 0)
        {
            alertPanel.SetActive(false);
        }
        if (currentTile == null) endTurn.GetComponent<Button>().interactable = false;
        else if (currentTile != null && currentTile.GetComponent<TileData>().type != ResistileServer.GameTileTypes.solder) endTurn.GetComponent<Button>().interactable = true;
    }

    public void Draw(int tileID)
    {
        ResistileServer.GameTile gameTile = getGameTile(tileID);
        GameObject tile = createGameTile(tileID);
        if (gameTile.type.Contains("Resistor") || gameTile.type == ResistileServer.GameTileTypes.solder)
        {
            tile.transform.SetParent(resHand.transform, false);
        }
        else if (gameTile.type.Contains("Wire"))
        {
            tile.transform.SetParent(wireHand.transform, false);
        }
    }

    public void placeTile(int tileID, int x, int y, int rotate)
    {
        ResistileServer.GameTile gameTile = getGameTile(tileID);
        if(gameTile.type.Contains("Wire"))
        {
            foreach(GameObject wireTile in wireHand.transform)
            {
                if(wireTile.GetComponent<TileData>().tileID == tileID)
                {
                    Destroy(wireTile);
                }
            }
        }

        GameObject boardNode = BoardHandler.GetNodeAt(x, y);
        
        var tile = createGameTile(tileID);
        for(int i = 0; i < rotate; i++)
        {
            tile.GetComponent<RotateTile>().TaskOnClick();
        }
        tile.transform.SetParent(boardNode.transform, false);
        tile.GetComponent<Draggable>().enabled = false;

    }

    public GameObject createGameTile(int id) //Created the game tile object
    {
        ResistileServer.GameTile gameTile = getGameTile(id);    //Tile data
        var tile = Instantiate(tilePrefab);                     //Tile Object
        TileData tileData = tile.GetComponent<TileData>();
        if (gameTile.type.Contains("Resistor"))
        {
            tile.GetComponentInChildren<Text>().text = gameTile.resistance.ToString();  //Set Resistance text on tile
            tileData.resistance = gameTile.resistance;                                  //Set Resistance data on TileData
            if (gameTile.type.Equals(ResistileServer.GameTileTypes.Resistor.typeI))
            {
                tile.transform.FindChild("Background").GetComponent<Image>().sprite = resI;
                tileData.type = ResistileServer.GameTileTypes.Resistor.typeI;
            }
            else if (gameTile.type.Equals(ResistileServer.GameTileTypes.Resistor.typeII))
            {
                tile.transform.FindChild("Background").GetComponent<Image>().sprite = resII;
                tileData.type = ResistileServer.GameTileTypes.Resistor.typeII;
            }
            
        }
        else
        {
            tile.transform.FindChild("OhmIcon").gameObject.SetActive(false);    //Disable Ohm Icon
            tile.transform.FindChild("Resistance").gameObject.SetActive(false); //Disable resistance text

            if (gameTile.type.Contains("Wire"))
            {
                if (gameTile.type.Equals(ResistileServer.GameTileTypes.Wire.typeI)) //Wire Straight
                {
                    tile.transform.FindChild("Background").GetComponent<Image>().sprite = wireI;
                    tileData.type = ResistileServer.GameTileTypes.Wire.typeI;
                }
                else if (gameTile.type.Equals(ResistileServer.GameTileTypes.Wire.typeII)) //Wire Bend
                {
                    tile.transform.FindChild("Background").GetComponent<Image>().sprite = wireII;
                    tileData.type = ResistileServer.GameTileTypes.Wire.typeII;
                }
                else if (gameTile.type.Equals(ResistileServer.GameTileTypes.Wire.typeT)) //Wire T
                {
                    tile.transform.FindChild("Background").GetComponent<Image>().sprite = wireIII;
                    tileData.type = ResistileServer.GameTileTypes.Wire.typeT;
                }
            }
            else if (gameTile.type == ResistileServer.GameTileTypes.solder)
            {
                tile.transform.FindChild("Background").GetComponent<Image>().sprite = solder;
                tileData.type = ResistileServer.GameTileTypes.solder;
            }
        }

        return tile;
    }

    public void setPrimaryObj(double obj)
    {

        primaryObj.GetComponent<Text>().text = string.Format("{0:N1}", obj);
    }

    public void setSecondaryObjs(int obj1, int obj2)
    {
        setSecondaryI(obj1);
        setSecondaryII(obj2);
    }

    public void setSecondaryI(int obj)
    {
        secondaryObjI.GetComponent<Text>().text = secondaryObjs[obj];
    }

    public void setSecondaryII(int obj)
    {
        secondaryObjII.GetComponent<Text>().text = secondaryObjs[obj];
    }

    public void initializeTurn(bool turn)
    {
        isTurn = turn;
    }

    public void setTurn()
    {
        string header;
        if (isTurn)
        {
            header = "Your Turn!";
            endTurn.GetComponent<Button>().interactable = true;

        }
        else
        {
            header = opponentName + "'s Turn";
            endTurn.GetComponent<Button>().interactable = false;
        }
        headerText.GetComponent<Text>().text = header;
        
    }

    public void changeTurn()
    {
        isTurn = !isTurn;
    }

    public void alert(string alertStr)
    {
        alertText.GetComponent<Text>().text = alertStr;
        alertTimer = 2.0f;
    }

    public void gameOver(bool isWinner, int pScore, int s1Score, int s2Score, int gScore, int tScore)
    {
        GameObject overlay = GameObject.Find("EndGameOverlay");
        setGameOver(isWinner, pScore, s1Score, s2Score, gScore, tScore);
        overlay.SetActive(true);
    }

    private void setGameOver(bool isWinner, int pScore, int s1Score, int s2Score, int gScore, int tScore)
    {
        if (isWinner)
        {
            gameOverPanelHeader.transform.FindChild("winLose").GetComponent<Text>().text = "WIN";
            gameOverPanelHeader.transform.FindChild("winLose").GetComponent<Text>().color = new Color(215.0f / 255.0f, 182.0f / 255.0f, 0.0f / 255.0f);
            gameOverPanelHeader.transform.FindChild("Trophy1").GetComponent<Image>().sprite = winTrophy;
            gameOverPanelHeader.transform.FindChild("Trophy2").GetComponent<Image>().sprite = winTrophy;
            primaryScore.GetComponent<Text>().text = pScore.ToString();
            secondary1Score.GetComponent<Text>().text = s1Score.ToString();
            secondary2Score.GetComponent<Text>().text = s2Score.ToString();
            guessScore.GetComponent<Text>().text = gScore.ToString();
            totalScore.GetComponent<Text>().text = tScore.ToString();
            for(int i = 0; i < tScore; i++)
            {
                var star = Instantiate(yellowStar);
                star.transform.SetParent(starPanel.transform, false);
            }
            for(int i = 0; i < 5-tScore; i++)
            {
                var star = Instantiate(blueStar);
                star.transform.SetParent(starPanel.transform, false);
            }
        }
        else
        {
            gameOverPanelHeader.transform.FindChild("winLose").GetComponent<Text>().text = "LOSE";
            gameOverPanelHeader.transform.FindChild("winLose").GetComponent<Text>().color = new Color(99.0f / 255.0f, 17.0f / 255.0f, 17.0f / 255.0f);
            gameOverPanelHeader.transform.FindChild("Trophy1").GetComponent<Image>().sprite = loseTrophy;
            gameOverPanelHeader.transform.FindChild("Trophy2").GetComponent<Image>().sprite = loseTrophy;
            primaryScore.GetComponent<Text>().text = pScore.ToString();
            secondary1Score.GetComponent<Text>().text = s1Score.ToString();
            secondary2Score.GetComponent<Text>().text = s2Score.ToString();
            guessScore.GetComponent<Text>().text = gScore.ToString();
            totalScore.GetComponent<Text>().text = tScore.ToString();
            for (int i = 0; i < tScore; i++)
            {
                var star = Instantiate(yellowStar);
                star.transform.SetParent(starPanel.transform, false);
            }
            for (int i = 0; i < 5 - tScore; i++)
            {
                var star = Instantiate(blueStar);
                star.transform.SetParent(starPanel.transform, false);
            }
        }
    }

    public void noPlayAgain()
    {
        playAgain.GetComponent<Button>().interactable = false;
    }

    public ResistileServer.GameTile getGameTile(int i)
    {
        return tileLookup[i];
    }

    private void fillObjectives()
    {
        secondaryObjs = new Dictionary<int, string>();
        secondaryObjs.Add(1, "EASY STREET: Have exactly three resistors in series back to back somewhere in the circuit.");
        secondaryObjs.Add(2, "LONGEST ROAD: Have five or more resistors in series without being interrupted by branches.");
        secondaryObjs.Add(3, "IT'S FUTILE: Solder out a piece (resistor or wire) and replace it with an identical piece.");
        secondaryObjs.Add(4, "CLEAN HOUSE: Ensure the completed circuit has no loose ends.");
        secondaryObjs.Add(5, "ALL THE CONNECTIONS: Esure that there are at least two loose ends when the circuit is complete.");
    }
}
