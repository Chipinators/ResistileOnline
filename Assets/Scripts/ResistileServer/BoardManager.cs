using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord.Math;

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
        public bool isGameOver = false;
        public GameTile[,] board = new GameTile[xlength, ylength];

        public GameTile startTile, endTile;
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
            // every open end should reference to blocked direction tile
            blockOpenEnds();
            addOneWireToAdjacentTWires();
            var paths = FindAllPaths();
            // one path, return sum of all.
            if (paths.Count == 1)
                return paths[0].Sum(gameTile => gameTile.resistance);
            else
            {
                //first, put current index in each tile in paths.
                int currentIndex = 0;
                var allTWires = new List<GameTile>();
                foreach (var path in paths)
                {
                    GameTile previousTile = path[0]; //should be endTile at the beginning.
                    for (var i = 1; i < path.Count; i++)
                    {
                        GameTile tile = path[i];
                        if (tile.firstAccessor == null)
                        {
                            tile.firstAccessor = previousTile;
                            tile.currentIndex = currentIndex;
                        }
                        if (tile.type == GameTileTypes.Wire.typeT)
                        {
                            currentIndex++;
                            if(!allTWires.Contains(tile))
                                allTWires.Add(tile);
                        }
                            
                        previousTile = tile;
                    }
                }
                double voltage = 12;
                double[,] solutionMatrix = new double[currentIndex + 1, currentIndex + 1];
                double[,] rightSide = new double[currentIndex + 1, 1];
                //then find first nonblockdir T mach, get current index,
                //special case start tile
                int row = 0;
                if (
                    startTile.neighbors.Values.Where(neighbor => neighbor != GameTile.blockedDirectionTile)
                        .ToList()
                        .Count == 2)
                {
                    var neighbor1 =
                        startTile.neighbors.Values.First(neighbor => neighbor != GameTile.blockedDirectionTile);
                    var neighbor2 =
                        startTile.neighbors.Values.First(
                            neighbor => neighbor != GameTile.blockedDirectionTile && neighbor != neighbor1);
                    solutionMatrix[row, 0] = 1;
                    solutionMatrix[row, neighbor1.currentIndex] = -1;
                    solutionMatrix[row, neighbor2.currentIndex] = -1;
                    rightSide[row, 0] = 0;
                }
                else
                {
                    var neighbor1 =
                        startTile.neighbors.Values.First(neighbor => neighbor != GameTile.blockedDirectionTile);
                    solutionMatrix[row, 0] = 1;
                    solutionMatrix[row, neighbor1.currentIndex] = -1;
                    rightSide[row, 0] = 0;
                }
                row++;
                foreach (var wire in allTWires)
                {
                    var neighborA = wire.neighbors.Values.First(neighbor => neighbor != GameTile.blockedDirectionTile);
                    var neighborB = wire.neighbors.Values.First(neighbor => neighbor != GameTile.blockedDirectionTile && neighbor != neighborA);
                    var neighborC = wire.neighbors.Values.First(neighbor => neighbor != GameTile.blockedDirectionTile && neighbor != neighborA && neighbor != neighborB);
                    solutionMatrix[row, neighborA.currentIndex] = 1;
                    solutionMatrix[row, neighborB.currentIndex] = -1;
                    solutionMatrix[row, neighborC.currentIndex] = -1;
                    rightSide[row, 0] = 0;
                    row += row < currentIndex ? 1 : 0; //just in case
                }
                foreach (var path in paths)
                {
                    double tempResistance = 0;
                    int tempIndex = 0;
                    for (var i = 1; i < path.Count; i++)
                    {
                        GameTile tempTile = path[i];
                        if (tempTile.currentIndex == tempIndex)
                        {
                            tempResistance += tempTile.resistance;
                        }
                        else
                        {
                            solutionMatrix[row, tempIndex] = tempResistance;
                            tempResistance = tempTile.resistance;
                        }
                        tempIndex = tempTile.currentIndex;
                    }
                    rightSide[row, 0] = voltage;
                    row += row < currentIndex ? 1 : 0; //just in case
                }
                //specific equals case for start and end tiles.
                //     set that 1, get other neighbors, their current indexes, 0 into solution matrix
                //Ideally, matrix should be square, but can be approximated if less.
                
                var amp = solutionMatrix.Solve(rightSide, leastSquares: true)[0, 0];
                double totalResistance = voltage / amp;
                return totalResistance;;
            }
        }

        private List<List<GameTile>> FindAllPaths()
        {
            List<GameTile> toBeTraversed = new List<GameTile> {endTile};
            Dictionary<GameTile, List<GameTile>> visited = new Dictionary<GameTile, List<GameTile>>();
            List<List<GameTile>> paths = new List<List<GameTile>>();
            for (var i = 0; i < toBeTraversed.Count; i++)
            {
                var tempTile = toBeTraversed[i];
                List<GameTile> currentVisits;
                if (!visited.ContainsKey(tempTile))
                {
                    currentVisits = new List<GameTile>();
                    visited.Add(tempTile, currentVisits);
                }
                else
                {
                    currentVisits = visited[tempTile];
                }

                List<GameTile> tempPath = new List<GameTile>();
                while (tempTile != null || tempTile != startTile)
                {
                    currentVisits.Add(tempTile);
                    if (tempTile.type == GameTileTypes.Wire.typeT)
                    {
                        //there will be two nodes to be traversed
                        //those neighbors should be the ones not in the current visit.
                        var addToBeTraversedList = tempTile.neighbors.Values.Where(search => search != GameTile.blockedDirectionTile && !currentVisits.Contains(search));
                        foreach (var addToBeTraversed in addToBeTraversedList)
                        {
                            toBeTraversed.Add(addToBeTraversed);
                            var copyCurrentPath = new List<GameTile>(currentVisits);
                            visited.Add(addToBeTraversed, copyCurrentPath);
                        }
                    }
                    else
                    {
                        tempTile = tempTile.neighbors.Values.FirstOrDefault(search => search != GameTile.blockedDirectionTile && !currentVisits.Contains(search));
                    }
                }

                if (tempTile == startTile)
                    paths.Add(tempPath);
                    
            }
            return paths;
        }

        private void addOneWireToAdjacentTWires()
        {
            foreach (GameTile gameTile in board)
            {
                if (gameTile != null && gameTile.type == GameTileTypes.Wire.typeT)
                {
                    var neighbors = gameTile.neighbors.ToList();
                    for (int index = 0; index < neighbors.Count; index++)
                    {
                        var neighbor = neighbors[index];
                        if (neighbor.Value.type == GameTileTypes.Wire.typeT)
                        {
                            var facingDirection = Directions.Facing[neighbor.Key];
                            var tileToBeAdded = new GameTile(GameTileTypes.Wire.typeI, 1);
                            gameTile.neighbors[neighbor.Key] = tileToBeAdded;
                            tileToBeAdded.neighbors[Directions.left] = gameTile;
                            tileToBeAdded.neighbors[Directions.right] = neighbor.Value;
                            neighbor.Value.neighbors[facingDirection] = tileToBeAdded;
                        }
                    }
                }
            }

        }
        private void blockOpenEnds()
        {
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
                            gameTile.type = GameTileTypes.Wire.typeI; // HACK, required for calculations.
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
                GameTile neighbor = gameTile;
                List<GameTile> traversed = new List<GameTile>();
                do
                {
                    traversed.Add(neighbor);
                    neighbor = 
                       neighbor.neighbors.Values.First(
                           newNeighbor => newNeighbor != null && !traversed.Contains(newNeighbor) && newNeighbor != GameTile.blockedDirectionTile);

                } while (neighbor.type != GameTileTypes.Wire.typeT);
                var beforeT = traversed.Last();
                string direction = "";
                foreach (var neighborPair in neighbor.neighbors)
                {
                    if (neighborPair.Value == beforeT)
                    {
                        direction = neighborPair.Key;
                    }
                }
                if (direction != "")
                {
                    neighbor.neighbors[direction] = GameTile.blockedDirectionTile;
                }
            }
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
            GetValue(tile, coordinates);
        }

        public void solderTile(GameTile newTile, int[] coordinates)
        {
            //Delete current tile 
            AddTile(newTile, coordinates);
        }

        private void GetValue(GameTile tile, int[] coordinates)
        {
            int x;
            int y;
            string oppositeDirection;
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
                return false;
            }

            // Case 2, 3.1, 3.2
            neighborCount = 0; //incremented by happyNeighbor
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
                    isGameOver = true;
                }
                else
                {
                    isValid = false;
                }
            }

            
            return isValid;

        }

        public bool IsValidSolder(GameTile newtile, int[] coordinates)
        {
            return IsValidSolder(newtile, new Coordinates(coordinates[0], coordinates[1]));
        }

        public bool IsValidSolder(GameTile newtile, Coordinates coordinates)
        {
            if (board[coordinates.x(), coordinates.y()] == null)
            {
                return false;
            }
            
            var tileOnBoard = board[coordinates.x(), coordinates.y()];
            //up
            if (newtile.neighbors[Directions.up] == null)
            {
                var newcoords = coordinates.up();
                var test = IsOutOfBoard(newcoords.x(), newcoords.y());
                if (test)
                    return false;
                var testedNeighbor = board[newcoords.x(), newcoords.y()];
                if (testedNeighbor != null && testedNeighbor.neighbors[Directions.down] == GameTile.blockedDirectionTile)
                    return false;
            }
            else
            {
                var newcoords = coordinates.up();
                var test = IsOutOfBoard(newcoords.x(), newcoords.y());
                if (!test)
                {
                    var testedNeighbor = board[newcoords.x(), newcoords.y()];
                    if (testedNeighbor != null && testedNeighbor.neighbors[Directions.down] == GameTile.blockedDirectionTile)
                        return false;
                }
            }
            //down
            if (newtile.neighbors[Directions.down] == null)
            {
                var newcoords = coordinates.down();
                var test = IsOutOfBoard(newcoords.x(), newcoords.y());
                if (test)
                    return false;
                var testedNeighbor = board[newcoords.x(), newcoords.y()];
                if (testedNeighbor != null && testedNeighbor.neighbors[Directions.up] == GameTile.blockedDirectionTile)
                    return false;
            }
            else
            {
                var newcoords = coordinates.down();
                var test = IsOutOfBoard(newcoords.x(), newcoords.y());
                if (!test)
                {
                    var testedNeighbor = board[newcoords.x(), newcoords.y()];
                    if (testedNeighbor != null && testedNeighbor.neighbors[Directions.up] == GameTile.blockedDirectionTile)
                        return false;
                }
            }
            //left
            if (newtile.neighbors[Directions.left] == null)
            {
                var newcoords = coordinates.left();
                var test = IsOutOfBoard(newcoords.x(), newcoords.y());
                if (test)
                    return false;
                var testedNeighbor = board[newcoords.x(), newcoords.y()];
                if (testedNeighbor != null && testedNeighbor.neighbors[Directions.right] == GameTile.blockedDirectionTile)
                    return false;
            }
            else
            {
                var newcoords = coordinates.left();
                var test = IsOutOfBoard(newcoords.x(), newcoords.y());
                if (!test)
                {
                    var testedNeighbor = board[newcoords.x(), newcoords.y()];
                    if (testedNeighbor != null && testedNeighbor.neighbors[Directions.right] == GameTile.blockedDirectionTile)
                        return false;
                }
            }
            //right
            if (newtile.neighbors[Directions.right] == null)
            {
                var newcoords = coordinates.right();
                var test = IsOutOfBoard(newcoords.x(), newcoords.y());
                if (test)
                    return false;
                var testedNeighbor = board[newcoords.x(), newcoords.y()];
                if (testedNeighbor != null && testedNeighbor.neighbors[Directions.left] == GameTile.blockedDirectionTile)
                    return false;
            }
            else
            {
                var newcoords = coordinates.right();
                var test = IsOutOfBoard(newcoords.x(), newcoords.y());
                if (!test)
                {
                    var testedNeighbor = board[newcoords.x(), newcoords.y()];
                    if (testedNeighbor != null && testedNeighbor.neighbors[Directions.left] == GameTile.blockedDirectionTile)
                        return false;
                }
            }
            return true;
        }

        // Check if tile has an end looking out of the board
        private bool IsOutOfBoard(int x, int y)
        {
            return x < 0 || y < 0 || x > xlength - 1 || y > ylength - 1;
        }

        // Check if tile has an end looking to a tile that doesn't look back
        // Not right
        private int neighborCount;

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
