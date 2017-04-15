using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ResistileServer
{
    [TestClass]
    public class TestCases
    {
        [TestMethod]
        public void testCalculateOnePath()
        {
            var boardManager = new BoardManager();
            for (var i = 1; i < 8; i++)
            {
                boardManager.AddTile(new GameTile(GameTileTypes.Wire.typeI),new []{i, 0} );
            }
            boardManager.AddTile(new GameTile(GameTileTypes.Wire.typeII), new[] { 8, 0 });
            for (var i = 1; i < 8; i++)
            {
                var tile = new GameTile(GameTileTypes.Wire.typeI);
                tile.Rotate();
                boardManager.AddTile(tile, new []{8, i});
            }
            var result = boardManager.Calculate();
            Assert.IsTrue(result.IsLessThan(0.1) && result.IsGreaterThan(-0.1));
        }

        [TestMethod]
        public void testCalculateTwoPaths()
        {
            var boardManager = new BoardManager();
            for (var i = 1; i < 7; i++)
            {
                boardManager.AddTile(new GameTile(GameTileTypes.Wire.typeI), new[] { i, 0 });
            }
            boardManager.AddTile(new GameTile(GameTileTypes.Wire.typeT), new []{7, 0});
            var tile1 = new GameTile(GameTileTypes.Resistor.typeII, 0, 4);
            tile1.Rotate();
            tile1.Rotate();
            boardManager.AddTile(tile1, new[] { 7, 1 });
            boardManager.AddTile(new GameTile(GameTileTypes.Resistor.typeII, 0, 4), new[] { 8, 0 });
            var tile2 = new GameTile(GameTileTypes.Wire.typeT);
            tile2.Rotate();
            boardManager.AddTile(tile2, new []{8, 1});
            for (var i = 2; i < 8; i++)
            {
                var tile = new GameTile(GameTileTypes.Wire.typeI);
                tile.Rotate();
                boardManager.AddTile(tile, new[] { 8, i });
            }
            var result = boardManager.Calculate();
            Assert.IsTrue(result.IsLessThan(2.1) && result.IsGreaterThan(1.9));
        }

        [TestMethod]
        public void testCalculateThreePaths()
        {
            var boardManager = new BoardManager();
            for (var i = 1; i < 6; i++)
            {
                boardManager.AddTile(new GameTile(GameTileTypes.Wire.typeI), new[] { i, 0 });
            }
            boardManager.AddTile(new GameTile(GameTileTypes.Wire.typeT), new[] { 6, 0 });
            boardManager.AddTile(new GameTile(GameTileTypes.Wire.typeT), new[] { 7, 0 });
            boardManager.AddTile(new GameTile(GameTileTypes.Resistor.typeII, 0, 4), new[] { 8, 0 });

            var tile = new GameTile(GameTileTypes.Wire.typeI);
            tile.Rotate();
            boardManager.AddTile(tile, new[] { 6, 1 });

            tile = new GameTile(GameTileTypes.Resistor.typeII, 0, 12);
            tile.Rotate();
            tile.Rotate();
            boardManager.AddTile(tile, new[] { 6, 2 });

            tile = new GameTile(GameTileTypes.Wire.typeI);
            boardManager.AddTile(tile, new[] { 7, 2 });

            tile = new GameTile(GameTileTypes.Resistor.typeII, 0, 12);
            tile.Rotate();
            tile.Rotate();
            boardManager.AddTile(tile, new[] { 7, 1 });
            
            tile = new GameTile(GameTileTypes.Wire.typeT);
            tile.Rotate();
            boardManager.AddTile(tile, new[] { 8, 1 });

            tile = new GameTile(GameTileTypes.Wire.typeT);
            tile.Rotate();
            boardManager.AddTile(tile, new[] { 8, 2 });

            for (var i = 3; i < 8; i++)
            {
                tile = new GameTile(GameTileTypes.Wire.typeI);
                tile.Rotate();
                boardManager.AddTile(tile, new[] { 8, i });
            }
            var result = boardManager.Calculate();
            Assert.IsTrue(result.IsLessThan(4.1) && result.IsGreaterThan(3.9));
        }

        [TestMethod]
        public void testWire()
        {
            var boardManager = new BoardManager();
            var tile = new GameTile(GameTileTypes.Wire.typeI, -1);
            var tile2 = new GameTile(GameTileTypes.Wire.typeII, -1);
            tile2.Rotate();
            boardManager.AddTile(tile, new[] { 1, 0 });
            var check = boardManager.IsValidMove(tile2, new[] {2, 1});
            Assert.IsFalse(check, "Not a valid placement on 2,1 because blocks type1 on 1,0 false");
        }
        [TestMethod]
        public void BlockStart()
        {
            var boardManager = new BoardManager();
            var tile = new GameTile(GameTileTypes.Wire.typeI, -1);
            tile.Rotate();
            var check = boardManager.IsValidMove(tile, new int[] { 1, 0 });
            Assert.IsFalse(check, "Case 2: Straight wire end off board, block start");
        }
        [TestMethod]
        public void ProperStart()
        {
            var boardManager = new BoardManager();
            var tile = new GameTile(GameTileTypes.Wire.typeI, -1);
            var check = boardManager.IsValidMove(tile, new int[] { 1, 0 });
            Assert.IsTrue(check, "Case 2: Straight wire next to start node");
        }
        [TestMethod]
        public void ProperTwoTiles()
        {
            var boardManager = new BoardManager();

            var tile = new GameTile(GameTileTypes.Wire.typeI, -1);
            var check = boardManager.IsValidMove(tile, new int[] { 1, 0 });
            Assert.IsTrue(check);
            if (check == true)
            {
                boardManager.AddTile(tile, new int[] { 1, 0 });
            }
            Assert.IsNotNull(boardManager.board[1,0], "Is tile placed");
            tile = new GameTile(GameTileTypes.Wire.typeII, -1);
            check = boardManager.IsValidMove(tile, new int[] { 2, 0 });
            if (check == true)
            {
                boardManager.AddTile(tile, new int[] { 2, 0 });
            }
            Assert.IsNotNull(boardManager.board[2, 0], "Is tile placed");
            Assert.IsTrue(check, "Case 3: Straight followed by Angle");
        }
        [TestMethod]
        public void ProperResistorWire()
        {
            var boardManager = new BoardManager();
            var tile = new GameTile(GameTileTypes.Wire.typeI, -1);
            var res = new GameTile(GameTileTypes.Resistor.typeI, 0, 4);

            var check = boardManager.IsValidMove(tile, new int[] { 1, 0 });
            Assert.IsTrue(check);
            if (check == true)
            {
                boardManager.AddTile(tile, new int[] { 1, 0 });
            }
            check = boardManager.IsValidMove(res, new int[] { 2, 0 });
            Assert.IsTrue(check);
            if (check == true)
            {
                boardManager.AddTile(res, new int[] { 2, 0 });
            }
        }
        [TestMethod]
        public void GameEndingLioop()
        {
            var boardManager = new BoardManager();
            var tile = new GameTile(GameTileTypes.Wire.typeI, -1);
            var res = new GameTile(GameTileTypes.Resistor.typeII, 0, 4);
            var tile2 = new GameTile(GameTileTypes.Wire.typeII, -1);
            tile2.Rotate();
            var res2 = new GameTile(GameTileTypes.Resistor.typeI, 0, 3);
            var tile3 = new GameTile(GameTileTypes.Wire.typeII, -1);
            tile3.Rotate();
            tile3.Rotate();


            var check = boardManager.IsValidMove(tile, new int[] { 1, 0 });
            Assert.IsTrue(check, "Is tile placed");
            if (check == true)
            {
                boardManager.AddTile(tile, new int[] { 1, 0 });
            }
            check = boardManager.IsValidMove(res, new int[] { 2, 0 });
            Assert.IsTrue(check, "Is resistor placed");
            if (check)
            {
                boardManager.AddTile(res, new int[] { 2, 0 });
            }
            check = boardManager.IsValidMove(tile2, new int[] { 2, 1 });
            Assert.IsTrue(check, "Is resistor placed");
            if (check == true)
            {
                boardManager.AddTile(tile2, new int[] { 2, 1 });
            }
            check = boardManager.IsValidMove(res2, new int[] { 1, 1 });
            Assert.IsTrue(check, "Is resistor placed");
            if (check == true)
            {
                boardManager.AddTile(res2, new int[] { 1, 1 });
            }
            check = boardManager.IsValidMove(tile3, new int[] { 0, 1 });
            Assert.IsTrue(check, "Is resistor placed");
            if (check == true)
            {
                boardManager.AddTile(tile3, new int[] { 0, 1 });
            }
        }
        [TestMethod]
        public void PlaceOutOfBounds() //Point end towards perimeter
        {
            var boardManager = new BoardManager();
            var tile = new GameTile(GameTileTypes.Wire.typeII, -1);
            tile.Rotate();
            var check = boardManager.IsValidMove(tile, new int[] { 0, 1 });
            Assert.IsFalse(check, "Does tile point to end of board");
            if(check)
            {
                boardManager.AddTile(tile, new int[] { 0, 1 });
            }
        }

        [TestMethod]
        public void TestSolderValidation()
        {
            var boardManager = new BoardManager();

            var tile = new GameTile(GameTileTypes.Wire.typeT, -1);
            boardManager.AddTile(tile, new int[] { 1, 0 });
            var tileReplace = new GameTile(GameTileTypes.Resistor.typeI, 0, 4);
            var check = boardManager.IsValidSolder(tileReplace, new Coordinates(1, 0));
            Assert.IsTrue(check, "is valid solder spot");
        }
    }
}