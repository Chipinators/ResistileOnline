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
                tempHand.Add(deck.drawResistorSolder());
            }
            playerOne = new ResistilePlayer(playerOneUsername, tempHand, GetRandomPrimary(primaryMIN, primaryMAX), CreateSecondaryObj());
            /*
            Initialize playerTwo
            */
            tempHand = new ArrayList();
            for (int i = 0; i < MAXHAND; i++)
            {
                tempHand.Add(deck.drawResistorSolder());
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

        public bool AddTile(ResistilePlayer player, GameTile tile, int[] coords)
        {
            board.AddTile(tile, coords);
            GameTile newTile;
            if (tile.type.Contains("Wire"))
            {
                wireHand.Remove(tile);
            }
            else if (tile.type.Contains("Resistor"))
            {
                player.hand.Remove(tile);
            }
            return board.isGameOver;
        }

        public bool AddTileWithSolder(ResistilePlayer player, GameTile tile, GameTile solder, int[] coords)
        {

            player.hand.Remove(solder);
            AddTile(player, tile, coords);
            return false;
        }

        public GameTile drawWire()
        {
            var wire = deck.drawWire();
            wireHand.Add(wire);
            return wire;
        }

        public GameTile drawResistorSolder(ResistilePlayer player)
        {
            var resistorSolder = deck.drawResistorSolder();
            player.hand.Add(resistorSolder);
            return resistorSolder;
        }

        public GameTile draw(GameTile tile, ResistilePlayer player)
        {
            return tile.type.Contains("Wire") ? drawWire() : drawResistorSolder(player);
        }

        private double resistance = -1;
        public double calculateResistance()
        {
            if (resistance < 0)
            {
                resistance = board.Calculate();
            }
            return resistance;
        }

        //1 EASY STREET: Have exactly three resistors in series back to back somewhere in the circuit.
        //2 LONGEST ROAD: Have five or more resistors in series without being interrupted by branches.
        //3 IT'S FUTILE: Solder out a piece (resistor or wire) and replace it with an identical piece.
        //4 CLEAN HOUSE: Ensure the completed circuit has no loose ends.
        //5 ALL THE CONNECTIONS: Esure that there are at least two loose ends when the circuit is complete.

        //Check secondary objectives - end turn
        //3 IT"S FUTILE

        private bool[] secondaryObjectiveChecks = new bool[secondaryMAX];
        private void checkEndTurnSecondaryObjectives()
        {
            
        }

        //Check secondary objectives - end game
        //1 EASY STREET
        //2 LONGEST ROAD
        //4 CLEAN HOUSE
        //5 ALL THE CONNECTIONS
        private void checkEndGameSecondaryObjectives()
        {
            
        }
    }
}