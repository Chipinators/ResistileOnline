using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Calculate : MonoBehaviour {

	// Use this for initialization
    public void Clicked()
    {
        Text text = gameObject.GetComponent<Text>();
        double totalResistance = BoardHandler.myGame.CalculateTotalResistance();
        text.text = totalResistance.ToString();
    }
}
