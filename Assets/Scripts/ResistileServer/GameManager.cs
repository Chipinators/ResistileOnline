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
            var p1SecObj = CreateSecondaryObj();
            //var p1SecObj = new[] {1, 2}; //to set objectives manually for testing
            playerOne = new ResistilePlayer(playerOneUsername, tempHand, GetRandomPrimary(primaryMIN, primaryMAX), p1SecObj);
            /*
            Initialize playerTwo
            */
            tempHand = new ArrayList();
            for (int i = 0; i < MAXHAND; i++)
            {
                tempHand.Add(deck.drawResistorSolder());
            }
            var p2SecObj = CreateSecondaryObj();
            //var p2SecObj = new[] {3, 5};
            playerTwo = new ResistilePlayer(playerTwoUsername, tempHand, GetRandomPrimary(primaryMIN, primaryMAX), p2SecObj);

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
            checkItsFutile(player, tile, coords);
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
        private bool calculated = false;
        public void calculateResistance()
        {
            if (!calculated)
            {
                resistance = 15;
                //resistance = board.Calculate();
                calculated = true;
            }
        }

        //Check secondary objectives - end turn
        //private void checkEndTurnSecondaryObjectives(GameTile tile, int[] coords)
        //{
        //}

        //3 IT'S FUTILE: Solder out a piece (resistor or wire) and replace it with an identical piece.
        private void checkItsFutile(ResistilePlayer player, GameTile tile, int[] coords)
        {
            if (player.secondaryObjectiveChecks[3] == false)
            {
                var tileToBeChecked = board.board[coords[0], coords[1]];
                bool check = tileToBeChecked.type == tile.type && tileToBeChecked.rotation == tile.rotation &&
                    (Math.Abs(tileToBeChecked.resistance - tile.resistance) < 0.01);

                player.secondaryObjectiveChecks[3] = check; // once its true, its always true.
            }
        }

        public void checkEndGamePrimaryObjective(ResistilePlayer player, ResistilePlayer opponent)
        {
            player.primaryAchieved = Math.Abs(resistance - player.primaryObjective) < Math.Abs(resistance - opponent.primaryObjective);
            opponent.primaryAchieved = !player.primaryAchieved;
        }

        public void checkGuessAchieved(ResistilePlayer player)
        {
            player.guessAchieved = Math.Abs(resistance - player.guess) < 0.2;
        }

        //Check secondary objectives - end game
        public void checkEndGameSecondaryObjectives(ResistilePlayer player)
        {
            var objectives = player.secondaryObjective;
            foreach (var objective in objectives)
            {
                switch (objective)
                {
                    case 1:
                        player.secondaryObjectiveChecks[1] = checkEasyStreet();
                        break;
                    case 2:
                        player.secondaryObjectiveChecks[2] = checkLongestRoad();
                        break;
                    case 4:
                        player.secondaryObjectiveChecks[4] = checkCleanHouse();
                        break;
                    case 5:
                        player.secondaryObjectiveChecks[5] = checkAllTheConnections();
                        break;
                }
            }
        }

        //1 EASY STREET: Have exactly three resistors in series back to back somewhere in the circuit.
        private bool checkEasyStreet()
        {
            //foreach tile on board, check backward, then check forward neighbors.
            List<GameTile> allResistorTiles = new List<GameTile>();
            foreach (var gameTile in board.board)
            {
                if (gameTile != null && gameTile.type.Contains("Resistor"))
                {
                    allResistorTiles.Add(gameTile);
                }
            }   
            foreach (var aResistorTile in allResistorTiles)
            {
                var neighborTraversed = new ArrayList();
                neighborTraversed.Add(aResistorTile);
                var neighborA = aResistorTile.neighbors.First(neighbor => neighbor.Value != GameTile.blockedDirectionTile).Value;
                var neighborB = aResistorTile.neighbors.First(neighbor => neighbor.Value != GameTile.blockedDirectionTile && neighbor.Value != neighborA).Value;
                for (var i = 0; i < neighborTraversed.Count; i++)
                {
                    if (neighborA != null && neighborA.type.Contains("Resistor"))
                    {
                        neighborTraversed.Add(neighborA);
                        neighborA =
                            neighborA.neighbors.First(
                                neighbor =>
                                    neighbor.Value != GameTile.blockedDirectionTile &&
                                    !neighborTraversed.Contains(neighbor.Value)).Value;
                    }
                    if (neighborB != null && neighborB.type.Contains("Resistor"))
                    {
                        neighborTraversed.Add(neighborB);
                        neighborB =
                            neighborB.neighbors.First(
                                neighbor =>
                                    neighbor.Value != GameTile.blockedDirectionTile &&
                                    !neighborTraversed.Contains(neighbor.Value)).Value;
                    }
                }
                if (neighborTraversed.Count == 3)
                    return true;
            }

            return false;
        }
        //2 LONGEST ROAD: Have five or more resistors in series without being interrupted by branches.
        private bool checkLongestRoad()
        {
            //foreach tile on board, check backward, then check forward neighbors.
            List<GameTile> allResistorTiles = new List<GameTile>();
            foreach (var gameTile in board.board)
            {
                if (gameTile != null && gameTile.type.Contains("Resistor"))
                {
                    allResistorTiles.Add(gameTile);
                }
            }
            foreach (var aResistorTile in allResistorTiles)
            {
                var neighborTraversed = new List<GameTile>();
                neighborTraversed.Add(aResistorTile);
                var neighborA = aResistorTile.neighbors.First(neighbor => neighbor.Value != GameTile.blockedDirectionTile).Value;
                var neighborB = aResistorTile.neighbors.First(neighbor => neighbor.Value != GameTile.blockedDirectionTile && neighbor.Value != neighborA).Value;
                for (var i = 0; i < neighborTraversed.Count; i++)
                {
                    if (neighborA != null && neighborA.type != GameTileTypes.Wire.typeT)
                    {
                        neighborTraversed.Add(neighborA);
                        neighborA =
                            neighborA.neighbors.First(
                                neighbor =>
                                    neighbor.Value != GameTile.blockedDirectionTile &&
                                    !neighborTraversed.Contains(neighbor.Value)).Value;
                    }
                    if (neighborB != null && neighborB.type != GameTileTypes.Wire.typeT)
                    {
                        neighborTraversed.Add(neighborB);
                        neighborB =
                            neighborB.neighbors.First(
                                neighbor =>
                                    neighbor.Value != GameTile.blockedDirectionTile &&
                                    !neighborTraversed.Contains(neighbor.Value)).Value;
                    }
                }
                if (neighborTraversed.FindAll(neighbor => neighbor.type.Contains("Resistor")).Count >= 5)
                    return true;
            }
            return false;
        }
        //4 CLEAN HOUSE: Ensure the completed circuit has no loose ends.
        private bool checkCleanHouse()
        {
            var allPlacedTiles = new List<GameTile>();
            foreach (var gameTile in board.board)
            {
                if(gameTile != null && gameTile != board.startTile && gameTile != board.endTile)
                    allPlacedTiles.Add(gameTile);   
            }
            return allPlacedTiles.FindAll(tile => tile.neighbors.Values.Contains(null)).Count == 0;
        }
        //5 ALL THE CONNECTIONS: Esure that there are at least two loose ends when the circuit is complete.
        private bool checkAllTheConnections()
        {
            var allPlacedTiles = new List<GameTile>();
            foreach (var gameTile in board.board)
            {
                if (gameTile != null && gameTile != board.startTile && gameTile != board.endTile)
                    allPlacedTiles.Add(gameTile);
            }
            return allPlacedTiles.FindAll(tile => tile.neighbors.Values.Contains(null)).Count >= 2;
        }

        public void calculateWhoWon(ResistilePlayer player, ResistilePlayer opponent)
        {
            int playerScore = (player.primaryAchieved ? 1 : 0 ) + (player.guessAchieved ? 1 : 0) + player.getSecondaryObjectiveResults().Count(e => e);
            int opponentScore = (opponent.primaryAchieved ? 1 : 0) + (opponent.guessAchieved ? 1 : 0) + opponent.getSecondaryObjectiveResults().Count(e => e);
            player.won = playerScore > opponentScore;
            opponent.won = !player.won;
        }
    }
}