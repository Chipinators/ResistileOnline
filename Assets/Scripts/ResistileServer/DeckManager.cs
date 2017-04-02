using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ResistileServer
{
    class DeckManager
    {
        private const int MAX = 92;
        private static int currId = -1;
        private static Random rng = new Random();
        public Stack<GameTile> wireHand = new Stack<GameTile>();
        public Stack<GameTile> tileHand = new Stack<GameTile>();
        private Stack<GameTile> wireDeck = new Stack<GameTile>();
        private Stack<GameTile> resistorSolderDeck = new Stack<GameTile>();
        private Dictionary<int, GameTile> allTiles = new Dictionary<int, GameTile>();
        static private Dictionary<String, int> cardCounts = new Dictionary<String, int>
            {
            {GameTileTypes.Wire.typeI, 14}, //0-13
            {GameTileTypes.Wire.typeII, 14}, //14-27
            {GameTileTypes.Wire.typeT, 11}, //28-38
            {GameTileTypes.Resistor.typeI, 24}, //
            {GameTileTypes.Resistor.typeII, 24}, //24
            {GameTileTypes.solder, 5} //5
            };

        //static private Dictionary<int, String> idList = new Dictionary<int, String>
        //{
        //    {0, GameTileTypes.Wire.typeI},
        //    {1, GameTileTypes.Wire.typeI},
        //    {2, GameTileTypes.Wire.typeI},
        //    {3, GameTileTypes.Wire.typeI},
        //    {4, GameTileTypes.Wire.typeI},
        //    {5, GameTileTypes.Wire.typeI},
        //}

        public DeckManager()
        {
            int i;
            /*
            WIRES
            */
            for (i=0; i<cardCounts[GameTileTypes.Wire.typeI]; i++) //Straight Wires
            {
                var tile = new GameTile(GameTileTypes.Wire.typeI, CreateId());
                wireDeck.Push(tile);
                allTiles.Add(i, tile);
            }
            for (i=0; i<cardCounts[GameTileTypes.Wire.typeII]; i++) //Angle Wires
            {
                var tile = new GameTile(GameTileTypes.Wire.typeII, CreateId());
                wireDeck.Push(tile);
                allTiles.Add(i, tile);

            }
            for (i=0; i<cardCounts[GameTileTypes.Wire.typeT]; i++) //T Wires
            {
                var tile = new GameTile(GameTileTypes.Wire.typeT, CreateId());
                wireDeck.Push(tile);
                allTiles.Add(i, tile);
            }
            /*
            STRAIGHT RESISTORS
            */
            for (i=0; i<cardCounts[GameTileTypes.Resistor.typeI]; i++)
            {
                if (i < 6)
                {
                    var tile = new GameTile(GameTileTypes.Resistor.typeI, CreateId(), 1);
                    resistorSolderDeck.Push(tile);
                    allTiles.Add(i, tile);
                }
                else if(i < 12)
                {
                    var tile = new GameTile(GameTileTypes.Resistor.typeI, CreateId(), 2);
                    resistorSolderDeck.Push(tile);
                    allTiles.Add(i, tile);
                }
                else if(i < 17)
                {
                    var tile = new GameTile(GameTileTypes.Resistor.typeI, CreateId(), 3);
                    resistorSolderDeck.Push(tile);
                    allTiles.Add(i, tile);
                }
                else if(i < 21)
                {
                    var tile = new GameTile(GameTileTypes.Resistor.typeI, CreateId(), 4);
                    resistorSolderDeck.Push(tile);
                    allTiles.Add(i, tile);
                }
                else
                {
                    var tile = new GameTile(GameTileTypes.Resistor.typeI, CreateId(), 5);
                    resistorSolderDeck.Push(tile);
                    allTiles.Add(i, tile);
                }
            }
            /*
            ANGLE RESISTORS
            */
            for (i=0; i<cardCounts[GameTileTypes.Resistor.typeII]; i++)
            {
                if(i < 6)
                {
                    var tile = new GameTile(GameTileTypes.Resistor.typeII, CreateId(), 1);
                    resistorSolderDeck.Push(tile);
                    allTiles.Add(i, tile);
                }
                else if(i < 12)
                {
                    var tile = new GameTile(GameTileTypes.Resistor.typeII, CreateId(), 2);
                    resistorSolderDeck.Push(tile);
                    allTiles.Add(i, tile);
                }
                else if (i < 17)
                {
                    var tile = new GameTile(GameTileTypes.Resistor.typeII, CreateId(), 3);
                    resistorSolderDeck.Push(tile);
                    allTiles.Add(i, tile);
                }
                else if (i < 21)
                {
                    var tile = new GameTile(GameTileTypes.Resistor.typeII, CreateId(), 4);
                    resistorSolderDeck.Push(tile);
                    allTiles.Add(i, tile);
                }
                else
                {
                    var tile = new GameTile(GameTileTypes.Resistor.typeII, CreateId(), 5);
                    resistorSolderDeck.Push(tile);
                    allTiles.Add(i, tile);
                }
            }
            /*
            SOLDER
            */
            for (i=0; i<cardCounts[GameTileTypes.solder]; i++)
            {
                var tile = new GameTile(GameTileTypes.solder, CreateId());
                resistorSolderDeck.Push(tile);
                allTiles.Add(i, tile);
            }

            /*
            SHUFFLE
            */
            Shuffle(wireDeck);
            Shuffle(wireDeck);

            Shuffle(resistorSolderDeck);
            Shuffle(resistorSolderDeck);

        }
        private static int CreateId()
        {
            currId++;
            return currId;
        }

        public void Shuffle<GameTile>(Stack<GameTile> stack)
        {
            var values = stack.ToArray();
            stack.Clear();
            foreach (var value in values.OrderBy(x => rng.Next()))
                stack.Push(value);
        }
        public GameTile draw()
        {
            return resistorSolderDeck.Pop();
        }
        public Dictionary<int, GameTile> returnAllTileDict()
        {
            return allTiles;
        }
    }
}
