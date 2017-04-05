using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResistileServer
{
    class BoardManager
    {
        public const int xlength = 9;
        public const int ylength = 9;
        public const int xUpTerminal = 8;
        public const int yUpTerminal = 7;
        public const int xLeftTerminal = 7;
        public const int yLeftTerminal = 8;
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

        public double Calculate()
        {
            var resistance = 0;
            // every open end should reference to blocked direction tile
            foreach (var gameTile in board)
            {
                if (gameTile != null)
                {
                    foreach (var key in gameTile.neighbors.Keys.ToList())
                    {
                        if (gameTile.neighbors[key] == null)
                            gameTile.neighbors[key] = GameTile.blockedDirectionTile;
                    }
                }
            }
            // for every open end,
            //    if wireT with only one null
            //         put blocked direction,
            //     else
            //         save them in a list
            //     then iterate board, find them, then put blockedNeighbor to its closes't wireT parent.
            List<GameTile> openEnds = new List<GameTile>();
            foreach (var gameTile in board)
            {
                if (gameTile != null)
                {
                    if (gameTile.type == GameTileTypes.Wire.typeT)
                    {
                        var nullNeighborCount = gameTile.neighbors.Count(neighbor => neighbor.Value == null);
                        if (nullNeighborCount == 1)
                        {
                            var nullNeighbor = gameTile.neighbors.First(neighbor => neighbor.Value == null).Key;
                            gameTile.neighbors[nullNeighbor] = GameTile.blockedDirectionTile;
                        }
                        else if (nullNeighborCount == 2)
                        {
                            openEnds.Add(gameTile);
                        }
                    }
                    else
                    {
                        foreach (var neighbor in gameTile.neighbors.Keys.ToList())
                        {
                            if (gameTile.neighbors[neighbor] == null)
                            {
                                openEnds.Add(gameTile);
                            }
                        }
                    }
                }
            }
            foreach (var gameTile in openEnds)
            {

            }

            // if there is no T wires, return sum of all.
            if (
                board.Cast<GameTile>().Count(gameTile => gameTile != null && gameTile.type == GameTileTypes.Wire.typeT) ==
                0)
                return board.Cast<GameTile>().Sum(gameTile => gameTile != null ? gameTile.resistance : 0);

            


            return resistance;
        }

        public void AddTile(GameTile tile, int[] coordinates)
        {
            board[coordinates[0], coordinates[1]] = tile;
            // Assume valid move, find non blocked directions of tile,
            // if neighbors exist:
            //   connect all neigbors to corresponding direction

            //up
            int x = coordinates[0];
            int y = coordinates[1] - 1;
            string oppositeDirection = Directions.down;
            if (tile.neighbors[Directions.up] == null && !IsOutOfBoard(x, y) && board[x, y] != null)
            {
                board[x, y].neighbors[oppositeDirection] = tile;
                tile.neighbors[Directions.up] = board[x, y];
            }
            //left
            x = coordinates[0] - 1;
            y = coordinates[1];
            oppositeDirection = Directions.right;
            if (tile.neighbors[Directions.left] == null && !IsOutOfBoard(x, y) && board[x, y] != null)
            {
                board[x, y].neighbors[oppositeDirection] = tile;
                tile.neighbors[Directions.left] = board[x, y];
            }
            //down
            x = coordinates[0];
            y = coordinates[1] + 1;
            oppositeDirection = Directions.up;
            if (tile.neighbors[Directions.down] == null && !IsOutOfBoard(x, y) && board[x, y] != null)
            {
                board[x, y].neighbors[oppositeDirection] = tile;
                tile.neighbors[Directions.down] = board[x, y];
            }
            //right
            x = coordinates[0] + 1;
            y = coordinates[1];
            oppositeDirection = Directions.left;
            if (tile.neighbors[Directions.right] == null && !IsOutOfBoard(x, y) && board[x, y] != null)
            {
                board[x, y].neighbors[oppositeDirection] = tile;
                tile.neighbors[Directions.right] = board[x, y];
            }
        }

        public bool IsValidMove(GameTile tile, int[] coordinates)
        {
            // Maybe keep a list of available placement coordinates
            // 1 Check if board has anything on that spot
            // 2 End is pointing off the board
            // 3 Check neighbors
            //    3.1 at least one neighbor looking to be connected
            //    3.2 no blocking neighbor
            //    3.3 has at least one neighbor
            // 4 if tile is on terminal spot(neighbors of end tile)
            //        should have another connection to another tile.

            bool isValid = true;
            // Case 1
            if (board[coordinates[0], coordinates[1]] != null)
            {
                isValid = false;
            }

            // Case 2, 3.1, 3.2
            neighborCount = 0;
            foreach (var neighbor in tile.neighbors)
            {
                if (neighbor.Value == null)
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
                    isValid = isValid && !IsOutOfBoard(x, y) && HappyNeighbor(board[x, y], oppositeDirection);
                }
            }
            //Case 3.3
            isValid = isValid & HasAtLeastOneNeighbor();
            //Case 4
            if ((coordinates[0] == xUpTerminal && coordinates[1] == yUpTerminal) || 
                (coordinates[0] == xLeftTerminal && coordinates[1] == yLeftTerminal))
            {
                if (neighborCount > 1)
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
            }

            
            return isValid;

        }

        // Check if tile has an end looking out of the board
        private bool IsOutOfBoard(int x, int y)
        {
            return x < 0 || y < 0 || x > xlength - 1 || y > ylength - 1;
        }

        // Check if tile has an end looking to a tile that doesn't look back
        // Not right
        private static int neighborCount;

        private bool HappyNeighbor(GameTile tile, string direction)
        {
            if(tile == null)
            {
                return true;
            }
            else
            {
                neighborCount++;
                return tile.neighbors[direction] == null;
            }
            
        }

        private bool HasAtLeastOneNeighbor()
        {
            return neighborCount > 0;
        }
    }
}
