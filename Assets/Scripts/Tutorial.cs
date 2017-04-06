using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {
    public Sprite[] imageArray;
    private Dictionary<int, string> headerText, bodyText;
    public int currentImage;
    private GameObject imagePanel;
    public Text header, body;

    private void Start()
    {
        imagePanel = gameObject;
        fillDics();
        updateImagePanel();
        updateTextPanel();
    }

    public void onRightDown()
    {
        currentImage++;
        if (currentImage >= imageArray.Length) currentImage = imageArray.Length;
        updateImagePanel();
        updateTextPanel();
    }

    public void onLeftDown()
    {
        currentImage--;
        if (currentImage <= 0) currentImage = 0;
        updateImagePanel();
        updateTextPanel();
    }

    private void updateImagePanel()
    {
        imagePanel.GetComponent<Image>().sprite = imageArray[currentImage];
    }

    private void updateTextPanel()
    {
        header.text = headerText[currentImage];
        body.text = bodyText[currentImage];
    }

    private void fillDics()
    {
        headerText = new Dictionary<int, string>();
        bodyText = new Dictionary<int, string>();
        headerText.Add(0, "Welcome to Resistile Online!");
        bodyText.Add(0, "The objective of the game is to create a circuit that is closer to your Target Resistance than your opponent is to their Target Resistance. At the end of the game you will receive points for completing objectives and the person with the highest score wins!");
        headerText.Add(1, "Target Resistance:");
        bodyText.Add(1, "This is your Target Resistance of the game. Each player has their own Target Resistance they are trying to achieve. At the end of the game the player who is closer to their Target Resistance will earn 2 points.");
        headerText.Add(2, "Secondary Objectives:");
        bodyText.Add(2, "These are your Secondary Objectives. You will earn 1 bonus point at the end of the game for each Secondary Objective you complete.");
        headerText.Add(3, "Resistor Hand:");
        bodyText.Add(3, "This is your Resistor Hand. These tiles will increase the resistance of the circuit. Solder Tiles will also appear in this hand. A Solder Tile will allow you to remove a Tile from the current circuit and replace it with a new valid Tile.");
        headerText.Add(4, "Wire Hand:");
        bodyText.Add(4, "This is your Wire Hand. These Tiles are shared between you and your opponent. A Wire Tile can be placed on your turn instead of placing one of your Resistor Tiles if you wish to not change the resistance of the circuit. The Wire Hand will also contain T-Shape Tiles. These tiles will create a parallel circuit, reducing the overall resistance of the circuit.");
        headerText.Add(5, "Game Board:");
        bodyText.Add(5, "This is the Game Board. On your turn you will place either a Resistor Tile or Wire tile on one of the open connections from the start node. At any point of the game when a connection has been made between the Start Node and End Node the game will be over.");
        headerText.Add(6, "Start Node:");
        bodyText.Add(6, "This is the Start Node. You can place a connecting node on either side of the Start Node at any point of the game. If only one side of the Start Node has a connection the other side will not be considered in the final calculation.");
        headerText.Add(7, "End Node:");
        bodyText.Add(7, "This is the End Node. The goal of the game is to create a circuit from the Start Node to the End Node. As soon as a connection has been made to the End Node the game will be over.");
        headerText.Add(8, "How To End Your Turn:");
        bodyText.Add(8, "Once you have placed a tile on the board you may submit your tile placement by clicking the End Turn button. If the tile placement is valid you will receive your new tile and it will be your opponent’s turn.");
        headerText.Add(9, "Current Player's Turn:");
        bodyText.Add(9, "This will display whose turn it currently is.");




    }
}
