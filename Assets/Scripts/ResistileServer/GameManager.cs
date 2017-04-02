using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResistileServer
{
    class GameManager
    {
        private const int MAXHAND = 5;
        Player playerOne;
        Player playerTwo;
        //BoardManager board;
        DeckManager deck;
        ArrayList tempHand;



        GameManager(String playerOneUsername, String playerTwoUsername, int clientID, int gameID)
        {
            // board = new BoardManager;
            deck = new DeckManager();
            for(int i=0; i<MAXHAND; i++)
            {
                tempHand.Add(deck.draw());
            }
            //playerOne = new Player();
        }
    }
}
