using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ResistileServer
{
    [TestClass]
    public class TestCases
    {
        [TestMethod]
        public void TestCase1False()
        {
            var boardManager = new BoardManager();
            var tile = new GameTile(GameTileTypes.Wire.typeI, -1);
            tile.Rotate();
            var check = boardManager.IsValidMove(tile, new int[] { 1, 0 });
            Assert.IsFalse(check, "Case 2: Straight wire end off board, block start");
        }
        [TestMethod]
        public void TestCase2True()
        {
            var boardManager = new BoardManager();
            var tile = new GameTile(GameTileTypes.Wire.typeI, -1);
            var check = boardManager.IsValidMove(tile, new int[] { 1, 0 });
            Assert.IsTrue(check, "Case 2: Straight wire next to start node");
        }
        [TestMethod]
        public void TestCase3False()
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
    }
}
