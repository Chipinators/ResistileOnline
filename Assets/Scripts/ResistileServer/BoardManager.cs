using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ResistileServer
{
    class BoardManager
    {
        public const int xlength = 9;
        public const int ylength = 9;
        public GameTile[,] board = new GameTile[xlength, ylength];

        private GameTile startTile, endTile;
        public BoardManager()
        {
            startTile = new GameTile(GameTileTypes.Wire.typeT);
            endTile = new GameTile(GameTileTypes.Wire.typeT);
            endTile.Rotate();
            board[0, 0] = startTile;
            board[xlength - 1, ylength - 1] = endTile;
        }

        public void AddTile(GameTile tile, int[] coordinates)
        {
            board[coordinates[0], coordinates[1]] = tile;
            // Assume valid move, find non blocked directions of tile,
            // if neighbors exist:
            //   connect all neigbors to corresponding direction
        }

        public bool IsValidMove(GameTile tile, int[] coordinates)
        {
            // Maybe keep a list of available placement coordinates
            // 1 Check if board has anything on that spot
            // 2 End is pointing off the board
            // 3 Check neighbors
            //    3.1 at least one neighbor looking to be connected
            //    3.2 no blocking neighbor
            // 4 if tile is on terminal spot(neighbors of end tile)
            //        should have another connection to another tile.

            bool isValid = true;
            // Case 1
            if (board[coordinates[0], coordinates[1]] != null)
            {
                isValid = false;
            }
            // Case 2 and 3
            foreach (var neighbor in tile.neighbors)
            {
                if (!neighbor.Value.Equals(GameTile.blockedDirectionTile))
                {
                    int x = -1, y = -1;
                    string oppositeDirection = "";
                    switch (neighbor.Key)
                    {
                        case Directions.up:
                            x = coordinates[0];
                            y = coordinates[1] - 1;
                            oppositeDirection = Directions.down;
                            break;
                        case Directions.left:
                            x = coordinates[0] - 1;
                            y = coordinates[1];
                            oppositeDirection = Directions.right;
                            break;
                        case Directions.down:
                            x = coordinates[0];
                            y = coordinates[1] + 1;
                            oppositeDirection = Directions.up;
                            break;
                        case Directions.right:
                            x = coordinates[0] + 1;
                            y = coordinates[1];
                            oppositeDirection = Directions.left;
                            break;
                    }
                    isValid = isValid && IsOutOfBoard(x, y) && HappyNeighbor(board[x, y], oppositeDirection);
                }
            }

            // case 4

            return isValid;

        }

        // Check if tile has an end looking out of the board
        private bool IsOutOfBoard(int x, int y)
        {
            return x < 0 || y < 0 || x > xlength - 1 || y > ylength - 1;
        }

        // Check if tile has an end looking to a tile that doesn't look back
        // Not right
        private bool HappyNeighbor(GameTile tile, string direction)
        {
            return !tile.neighbors[direction].Equals(GameTile.blockedDirectionTile);
        }


    }
}
