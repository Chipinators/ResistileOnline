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
        private const double primaryMIN = 8.0;
        private const double primaryMAX = 17.0;
        private const int secondaryMIN = 1;
        private const int secondaryMAX = 6;
        int[] secondaryObj = new int[2];
        Random random = new Random();

        Player playerOne;
        Player playerTwo;
        //BoardManager board;
        DeckManager deck;
        ArrayList tempHand;

        GameManager(string playerOneUsername, string playerTwoUsername, int clientID, int gameID)
        {
            // board = new BoardManager;
            deck = new DeckManager();
            //Initialize playerOne
            for (int i = 0; i < MAXHAND; i++)
            {
                tempHand.Add(deck.draw());
            }
            playerOne = new Player(playerOneUsername, tempHand, GetRandomPrimary(primaryMIN, primaryMAX), CreateSecondaryObj());
            //Initialize playerTwo
            for (int i = 0; i < MAXHAND; i++)
            {
                tempHand.Add(deck.draw());
            }
            playerTwo = new Player(playerTwoUsername, tempHand, GetRandomPrimary(primaryMIN, primaryMAX), CreateSecondaryObj());
        }



        private double GetRandomPrimary(double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        private int GetRandomSecondary()
        {
            int randomNumber = random.Next(secondaryMIN, secondaryMAX);
            return randomNumber;
        }

        private int[] CreateSecondaryObj()
        {
            secondaryObj[0] = GetRandomSecondary();
            do
            {
                secondaryObj[1] = GetRandomSecondary();
            } while (secondaryObj[0] == secondaryObj[1]);
            return secondaryObj;
        }
    }
}
