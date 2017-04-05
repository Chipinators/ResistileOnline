using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private Dictionary<int, string> secondaryObjs;
    private string yourName, opponentName;
    private bool isTurn;
    private float alertTimer;

    void Start()
    {
        gameHandler = this;
        fillObjectives();
        DrawResistor(2, 1);
        DrawResistor(-1, 1);
        DrawResistor(5, 2);
        DrawResistor(3, 1);
        DrawResistor(1, 2);
        DrawWire(1);
        DrawWire(2);
        DrawWire(3);
        DrawWire(1);
        DrawWire(2);
        setPrimaryObj(10.5);
        setSecondaryObjs(1, 2);
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
    }

    public void DrawResistor(int res, int type)
    {
        if (resHand.transform.childCount >= 5) return;
        var tile = Instantiate(tilePrefab);
        tile.transform.SetParent(resHand.transform, false);
        tile.GetComponentInChildren<Text>().text = res.ToString();
        if (res == -1)
        {
            tile.transform.FindChild("OhmIcon").gameObject.SetActive(false);
            tile.transform.FindChild("Resistance").gameObject.SetActive(false);
            tile.transform.FindChild("Background").GetComponent<Image>().sprite = solder;
        }
        else
        {
            tile.GetComponentInChildren<Text>().text = res.ToString();
            if (type == 1)
            {
                tile.transform.FindChild("Background").GetComponent<Image>().sprite = resI;

            }   
            else if (type == 2)
            {
                tile.transform.FindChild("Background").GetComponent<Image>().sprite = resII;
            }         
        }
    }

    public void DrawWire(int type)
    {
        if (wireHand.transform.childCount >= 5) return;
        var tile = Instantiate(tilePrefab);
        tile.transform.SetParent(wireHand.transform, false);
        tile.transform.FindChild("OhmIcon").gameObject.SetActive(false);
        tile.transform.FindChild("Resistance").gameObject.SetActive(false);
        if (type == 1) //Wire Straight
        {
            tile.transform.FindChild("Background").GetComponent<Image>().sprite = wireI;
        }
        else if (type == 2) //Wire Bend
        {
            tile.transform.FindChild("Background").GetComponent<Image>().sprite = wireII;
        }
        else if (type == 3) //Wire T
        {
            tile.transform.FindChild("Background").GetComponent<Image>().sprite = wireIII;
        }
    }

    public void setPrimaryObj(double obj)
    {
        primaryObj.GetComponent<Text>().text = obj.ToString();
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

    public void placeTile(int tileID, int xCoord, int yCoord, int rotation)
    {

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
