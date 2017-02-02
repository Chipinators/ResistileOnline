using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using ResistileConsole;
using UnityEngine;

public class GameNodeAdapter : MonoBehaviour {

	// Use this for initialization
    internal GameNode gameNode;
	void Start ()
	{
        Debug.Log("Start called " + gameObject.tag);
	    string tag = gameObject.tag;

        if (tag == "ResistorTypeI")
        {
            gameNode = new GameNodeTypeIResistor(1);
            gameNode.Rotate();
        }
        else if (tag == "ResistorTypeII")
	    {
            gameNode = new GameNodeTypeIIResistor(1);
        }
        else if (tag == "WireTypeI")
	    {
            gameNode = new GameNodeTypeIWire();
	        gameNode.Rotate();
	    }
        else if (tag == "WireTypeII")
        {
            gameNode = new GameNodeTypeIIWire();
        }
        else if (tag == "WireTypeT")
        {
            gameNode = new GameNodeTypeTWire();
        }

	    Rotate gameObjectRotate = gameObject.GetComponent<Rotate>();
	    for (int i = 0; i < gameObjectRotate.rotation; i++)
	    {
	        Rotate();
	    }
	}

    public void Rotate()
    {
        gameNode.Rotate();
    }
}
