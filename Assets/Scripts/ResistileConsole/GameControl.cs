using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Math;

namespace ResistileConsole
{
    class GameControl
    {
        private GameNode _root;
        private GameNode _end;
        GameNode _downNeighbor, _leftNeighbor, _rightNeighbor, _upNeighbor;

        public static readonly Coordinates BoardSize = new Coordinates(7, 7);
        private GameNode[,] _gameMap = new GameNode[BoardSize.X, BoardSize.Y];

        public GameControl()
        {
            _root = new GameNodeTypeRoot();
            _root.CurrentIndex = 0;
            _end = new GameNodeTypeEnd();

            _gameMap[0, 0] = _root;
            _gameMap[BoardSize.X-1, BoardSize.Y-1] = _end;
        }

        private bool IsPossibleMove(GameNode nodeToAdd, Coordinates coordinates)
        {
            //given coordinate has an element already.
            if (_gameMap[coordinates.X, coordinates.Y] != null)
                return false;

            //given coordinate is not within the board size
            if (coordinates.X >= BoardSize.X || coordinates.X < 0 || coordinates.Y >= BoardSize.Y || coordinates.Y < 0)
                return false;
            
            //check if given coordinate have a possible neighbor
            FindNeighbors(nodeToAdd, coordinates);
            if (_downNeighbor == null && _leftNeighbor == null && _rightNeighbor == null && _upNeighbor == null)
                return false;
            //TODO
            //Also check new element is not blocking neighbor!
            if (!BlockedNeighbor(nodeToAdd, coordinates))
                return false;
            return true;
            
        }

        private bool BlockedNeighbor(GameNode nodeToAdd, Coordinates coordinates)
        {
            GameNode tempNeighbor = !(coordinates.Y == BoardSize.Y - 1 || _gameMap[coordinates.X, coordinates.Y + 1] == null) ? _gameMap[coordinates.X, coordinates.Y + 1] : null;
            if (tempNeighbor != null && (nodeToAdd.Down != null && tempNeighbor.Up == null))
                return false;
            tempNeighbor = !(coordinates.X == 0 || _gameMap[coordinates.X - 1, coordinates.Y] == null) ? _gameMap[coordinates.X - 1, coordinates.Y] : null;
            if (tempNeighbor != null && (nodeToAdd.Left != null && tempNeighbor.Right == null))
                return false;
            tempNeighbor = !(coordinates.X == BoardSize.X - 1 || _gameMap[coordinates.X + 1, coordinates.Y] == null) ? _gameMap[coordinates.X + 1, coordinates.Y] : null;
            if (tempNeighbor != null && (nodeToAdd.Right != null && tempNeighbor.Left == null))
                return false;
            tempNeighbor = !(coordinates.Y == 0 || _gameMap[coordinates.X, coordinates.Y - 1] == null) ? _gameMap[coordinates.X, coordinates.Y - 1] : null;
            if (tempNeighbor != null && (nodeToAdd.Up != null && tempNeighbor.Down == null))
                return false;
            return true;
        }

        private void FindNeighbors(GameNode nodeToAdd, Coordinates coordinates)
        {
            _downNeighbor = null;
            _leftNeighbor = null;
            _rightNeighbor = null;
            _upNeighbor = null;
            if (nodeToAdd.Down == null)
            {
                _downNeighbor = !(coordinates.Y == BoardSize.Y - 1 || _gameMap[coordinates.X, coordinates.Y + 1] == null) ? _gameMap[coordinates.X, coordinates.Y + 1] : null;
            }
            if (nodeToAdd.Left == null)
            {
                _leftNeighbor = !(coordinates.X == 0 || _gameMap[coordinates.X - 1, coordinates.Y] == null) ? _gameMap[coordinates.X - 1, coordinates.Y] : null;
            }
            if (nodeToAdd.Right == null)
            {
                _rightNeighbor = !(coordinates.X == BoardSize.X - 1 || _gameMap[coordinates.X + 1, coordinates.Y] == null) ? _gameMap[coordinates.X + 1, coordinates.Y] : null;
            }
            if (nodeToAdd.Up == null)
            {
                _upNeighbor = !(coordinates.Y == 0 || _gameMap[coordinates.X, coordinates.Y - 1] == null) ? _gameMap[coordinates.X, coordinates.Y - 1] : null;
            }
        }

        public bool AddGameNodeToBoard(GameNode nodeToAdd, Coordinates coordinates)
        {
            //return true if node is successfully added to the board
            if (!IsPossibleMove(nodeToAdd, coordinates))
                return false;
            _gameMap[coordinates.X, coordinates.Y] = nodeToAdd;
            if (_downNeighbor != null)
            {
                nodeToAdd.Down = _downNeighbor;
                _downNeighbor.Up = nodeToAdd;
            }
            if (_leftNeighbor != null)
            {
                nodeToAdd.Left = _leftNeighbor;
                _leftNeighbor.Right = nodeToAdd;
            }
            if (_rightNeighbor != null)
            {
                nodeToAdd.Right = _rightNeighbor;
                _rightNeighbor.Left = nodeToAdd;
            }
            if (_upNeighbor != null)
            {
                nodeToAdd.Up = _upNeighbor;
                _upNeighbor.Down = nodeToAdd;
            }
            return true;
        }

        private bool IsCircuitComplete()
        {
            //check if the end node is conected 
            if (_end.Up == null && _end.Left == null)
                return false;
            //check if any node has an unconnected end, return false if there are open ends, true otherwise
            return _gameMap.Cast<GameNode>().All(gameNode => gameNode == null || (gameNode.Down != null && gameNode.Left != null && gameNode.Right != null && gameNode.Down != null));
        }

        public double CalculateTotalResistance()
        {
            if (!IsCircuitComplete())
                throw new CircuitNotCompleteException();
            countOfWireTypeT = _gameMap.Cast<GameNode>().Count(gameNode => gameNode != null && (gameNode.GetType() == typeof (GameNodeTypeTWire)));

            //no parallel circuits
            if (countOfWireTypeT == 0)
            {
                return (from GameNode gameNode in _gameMap where gameNode != null select gameNode.Resistance).Sum();
            }

            PopulateSolutionLists(_root);
            PopulateSolutionMatrix();
            double[,] solutions = _solutionMatrix.Solve(_rightSide, leastSquares: true);

            double amp = solutions[0, 0];
            double totalResistance = voltage/amp;
            return totalResistance;
        }

        private void PopulateSolutionMatrix()
        {
            int n = Math.Max(_solutionMatrixList.Count,_solutionMatrixList[0].Length);
            _solutionMatrix = new double[n, n];
            _rightSide = new double[n, 1];
            for (var i = 0; i < _solutionMatrixList.Count; i++)
            {
                for (var i1 = 0; i1 < _solutionMatrixList[i].Length; i1++)
                {
                    _solutionMatrix[i, i1] = _solutionMatrixList[i][i1];
                }
            }
            for (var i = 0; i < _rightSideList.Count; i++)
            {
                _rightSide[i, 0] = _rightSideList[i];
            }
        }

        private readonly double voltage = 10;
        private int countOfWireTypeT;
        private double[,] _solutionMatrix;
        private List<double[]> _solutionMatrixList; 
        private double[,] _rightSide;
        private List<double> _rightSideList;
        private List<IGameNode> _visitedTNodes; 
        private void PopulateSolutionLists(IGameNode node, List<IGameNode> visited = null )
        {
            if (node == _end)
            {
                //visited will have important stuff
                //populate list with it.
                visited.Add(node);
                /*foreach (var gameNode in visited)
                {
                    gameNode.CurrentIndex = -1;
                }*/

                int i = 0;
                double[] arr = new double[countOfWireTypeT * 2];
                double tempSum = 0;
                
                IGameNode prevVisited = null;
                if (_solutionMatrixList == null)
                    _solutionMatrixList = new List<double[]>();
                if (_rightSideList == null)
                    _rightSideList = new List<double>();

                for (int visitedNodeIndex = 0; visitedNodeIndex < visited.Count; visitedNodeIndex++)
                {
                    IGameNode gameNode = visited[visitedNodeIndex];
                    if (gameNode == _root)
                    {
                        prevVisited = _root;
                    }
                    else if (gameNode == _end)
                    {
                        arr[prevVisited.CurrentIndex] += tempSum;
                    }
                    else if (gameNode.GetType() == typeof (GameNodeTypeTWire))
                    {
                        arr[i] += tempSum;
                        tempSum = 0;

                        if(_visitedTNodes == null)
                            _visitedTNodes = new List<IGameNode>();

                        bool shouldMarkT = true;
                        if (_visitedTNodes.Contains(gameNode))
                        {
                            shouldMarkT = false;
                        }
                        else
                            _visitedTNodes.Add(gameNode);

                        double[] arrT = new double[countOfWireTypeT*2];

                        int maxIndex = 0;
                        foreach (var gameNode1 in _gameMap)
                        {
                            if ((gameNode1 != null) && (gameNode1.CurrentIndex > maxIndex))
                                maxIndex = gameNode1.CurrentIndex;
                        }

                        IGameNode firstChild = null;
                        IGameNode secondChild = null;
                        if (gameNode.Down == prevVisited)
                        {
                            if (gameNode.Left.GetType() == typeof (BlockedDirection))
                            {
                                firstChild = gameNode.Right;
                                secondChild = gameNode.Up;
                            }
                            else if (gameNode.Right.GetType() == typeof (BlockedDirection))
                            {
                                firstChild = gameNode.Left;
                                secondChild = gameNode.Up;
                            }
                            else if (gameNode.Up.GetType() == typeof (BlockedDirection))
                            {
                                firstChild = gameNode.Left;
                                secondChild = gameNode.Right;
                            }
                        }
                        if (gameNode.Left == prevVisited)
                        {
                            if (gameNode.Down.GetType() == typeof (BlockedDirection))
                            {
                                firstChild = gameNode.Right;
                                secondChild = gameNode.Up;
                            }
                            else if (gameNode.Right.GetType() == typeof (BlockedDirection))
                            {
                                firstChild = gameNode.Down;
                                secondChild = gameNode.Up;
                            }
                            else if (gameNode.Up.GetType() == typeof (BlockedDirection))
                            {
                                firstChild = gameNode.Down;
                                secondChild = gameNode.Right;
                            }
                        }
                        if (gameNode.Right == prevVisited)
                        {
                            if (gameNode.Down.GetType() == typeof (BlockedDirection))
                            {
                                firstChild = gameNode.Left;
                                secondChild = gameNode.Up;
                            }
                            else if (gameNode.Left.GetType() == typeof (BlockedDirection))
                            {
                                firstChild = gameNode.Down;
                                secondChild = gameNode.Up;
                            }
                            else if (gameNode.Up.GetType() == typeof (BlockedDirection))
                            {
                                firstChild = gameNode.Down;
                                secondChild = gameNode.Left;
                            }
                        }
                        if (gameNode.Up == prevVisited)
                        {
                            if (gameNode.Down.GetType() == typeof (BlockedDirection))
                            {
                                firstChild = gameNode.Left;
                                secondChild = gameNode.Right;
                            }
                            else if (gameNode.Left.GetType() == typeof (BlockedDirection))
                            {
                                firstChild = gameNode.Down;
                                secondChild = gameNode.Right;
                            }
                            else if (gameNode.Right.GetType() == typeof (BlockedDirection))
                            {
                                firstChild = gameNode.Down;
                                secondChild = gameNode.Left;
                            }
                        }
                        arrT[i] = 1;
                        if (firstChild.CurrentIndex == -1)
                        {
                            firstChild.CurrentIndex = ++maxIndex;
                            arrT[firstChild.CurrentIndex] = -1;
                        }
                        else
                        {
                            arrT[firstChild.CurrentIndex] = 1;
                        }
                        if (secondChild.CurrentIndex == -1)
                        {
                            secondChild.CurrentIndex = ++maxIndex;
                            arrT[secondChild.CurrentIndex] = -1;
                        }
                        else
                        {
                            arrT[secondChild.CurrentIndex] = 1;
                        }

                        if (shouldMarkT)
                        {
                            _solutionMatrixList.Add(arrT);
                            _rightSideList.Add(0);
                        }
                        
                    }
                    else
                    {
                        tempSum += gameNode.ResistanceWithDirection(prevVisited);
                        if (gameNode.CurrentIndex == -1)
                            gameNode.CurrentIndex = i;
                        i = gameNode.CurrentIndex;
                    }
                    prevVisited = gameNode;
                }
                _solutionMatrixList.Add(arr);
                _rightSideList.Add(voltage);
                return;
            }
            if (visited != null && visited.Contains(node))
            {
                return;
            }

            if (node == _root)
            {
                visited = new List<IGameNode> {node};
                PopulateSolutionLists(node.Right, visited);
                return;
            }

            List<IGameNode> clone;
            var visitedLast = visited.Last();
            bool added = false;
            if (visitedLast != node.Down && node.Down.GetType() != typeof(BlockedDirection))
            {
                if (!added)
                {
                    visited.Add(node);
                    added = true;
                }
                clone = new List<IGameNode>(visited);
                if(!visited.Contains(node.Down))
                    PopulateSolutionLists(node.Down, node.GetType() == typeof(GameNodeTypeTWire) ? clone : visited);
            }
            if (visitedLast != node.Left && node.Left.GetType() != typeof(BlockedDirection))
            {
                if (!added)
                {
                    visited.Add(node);
                    added = true;
                }
                clone = new List<IGameNode>(visited);
                if (!visited.Contains(node.Left))
                    PopulateSolutionLists(node.Left, node.GetType() == typeof(GameNodeTypeTWire) ? clone : visited);
            }
            if (visitedLast != node.Right && node.Right.GetType() != typeof(BlockedDirection))
            {
                if (!added)
                {
                    visited.Add(node);
                    added = true;
                }
                clone = new List<IGameNode>(visited);
                if (!visited.Contains(node.Right))
                    PopulateSolutionLists(node.Right, node.GetType() == typeof(GameNodeTypeTWire) ? clone : visited);
            }
            if (visitedLast != node.Up && node.Up.GetType() != typeof(BlockedDirection))
            {
                if (!added)
                {
                    visited.Add(node);
                    added = true;
                }
                clone = new List<IGameNode>(visited);
                if (!visited.Contains(node.Up))
                    PopulateSolutionLists(node.Up, node.GetType() == typeof(GameNodeTypeTWire) ? clone : visited);
            }
            
        }

    }

    internal class CircuitNotCompleteException : Exception
    {
    }

}
