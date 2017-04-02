using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {
    public static GameHandler gameHandler;
    public GameObject resHand, resDeck, wireHand, wireDeck, gameBoard;
    public GameObject tilePrefab;
    public Button endTurn;
    public Sprite solder, resI, resII, wireI, wireII, wireIII;
    public GameObject primaryObj, secondaryObjI, secondaryObjII;
    private Dictionary<int, string> secondaryObjs;

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
        Debug.Log("Set Secondary I: " + secondaryObjs[obj]);
        secondaryObjI.GetComponent<Text>().text = secondaryObjs[obj];
    }

    public void setSecondaryII(int obj)
    {
        secondaryObjII.GetComponent<Text>().text = secondaryObjs[obj];
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
