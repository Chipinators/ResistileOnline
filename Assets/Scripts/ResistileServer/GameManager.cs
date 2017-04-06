using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResistileClient;

namespace ResistileServer
{
    class GameManager
    {
        private const int MAXHAND = 5;
        private const double primaryMIN = 8.0;
        private const double primaryMAX = 17.0;
        private const int secondaryMIN = 1;
        private const int secondaryMAX = 6;
        
        Random random = new Random();

        public ResistilePlayer playerOne;
        public ResistilePlayer playerTwo;
        public DeckManager deck;
        public BoardManager board;
        public ResistilePlayer currentTurnPlayer;
        public ArrayList wireHand = new ArrayList();
        public GameManager(string playerOneUsername, string playerTwoUsername)
        {
            /*
            Initialize Board
            */
            board = new BoardManager();
            /*
            Initialize Deck
            */
            deck = new DeckManager();
            /*
            Initialize playerOne
            */
            ArrayList tempHand = new ArrayList();
            for (int i = 0; i < MAXHAND; i++)
            {
                tempHand.Add(deck.draw());
            }
            playerOne = new ResistilePlayer(playerOneUsername, tempHand, GetRandomPrimary(primaryMIN, primaryMAX), CreateSecondaryObj());
            /*
            Initialize playerTwo
            */
            tempHand = new ArrayList();
            for (int i = 0; i < MAXHAND; i++)
            {
                tempHand.Add(deck.draw());
            }
            playerTwo = new ResistilePlayer(playerTwoUsername, tempHand, GetRandomPrimary(primaryMIN, primaryMAX), CreateSecondaryObj());

            currentTurnPlayer = random.Next(0, 2) == 1 ? playerOne : playerTwo;
            for (int i = 0; i < MAXHAND; i++)
            {
                wireHand.Add(deck.drawWire());
            }
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
            int[] secondaryObj = new int[2];
            secondaryObj[0] = GetRandomSecondary();
            do
            {
                secondaryObj[1] = GetRandomSecondary();
            } while (secondaryObj[0] == secondaryObj[1]);
            return secondaryObj;
        }

        public ResistilePlayer getPlayer(string clName)
        {
            return playerOne.userName == clName ? playerOne : playerTwo;
        }

        public ResistilePlayer getOpponent(string clName)
        {
            return playerOne.userName == clName ? playerTwo : playerOne;
        }
    }
}