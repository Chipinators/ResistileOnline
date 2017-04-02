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

        static private Dictionary<String, int> cardCounts = new Dictionary<String, int>
            {
            {GameTileTypes.Wire.typeI, 14},
            {GameTileTypes.Wire.typeII, 14},
            {GameTileTypes.Wire.typeT, 11}, //11
            {GameTileTypes.Resistor.typeI, 24}, //24
            {GameTileTypes.Resistor.typeII, 24}, //24
            {GameTileTypes.solder, 5} //5
            };

        private static int CreateId()
        {
            currId++;
            return currId;
        }

        public DeckManager()
        {
            int i;
            /*
            WIRES
            */
            for (i=0; i<cardCounts[GameTileTypes.Wire.typeI]; i++) //Straight Wires
            {
                wireDeck.Push(new GameTile(GameTileTypes.Wire.typeI, CreateId()));
            }
            for (i=0; i<cardCounts[GameTileTypes.Wire.typeII]; i++) //Angle Wires
            {
                wireDeck.Push(new GameTile(GameTileTypes.Wire.typeII, CreateId()));
            }
            for (i=0; i<cardCounts[GameTileTypes.Wire.typeT]; i++) //T Wires
            {
                wireDeck.Push(new GameTile(GameTileTypes.Wire.typeT, CreateId()));
            }
            /*
            STRAIGHT RESISTORS
            */
            for (i=0; i<cardCounts[GameTileTypes.Resistor.typeI]; i++)
            {
                if (i < 6)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeI, CreateId(), 1));
                }
                else if(i < 12)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeI, CreateId(), 2));
                }
                else if(i < 17)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeI, CreateId(), 3));
                }
                else if(i < 21)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeI, CreateId(), 4));
                }
                else
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeI, CreateId(), 5));
                }
            }
            /*
            ANGLE RESISTORS
            */
            for (i=0; i<cardCounts[GameTileTypes.Resistor.typeII]; i++)
            {
                if(i < 6)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeII, CreateId(), 1));
                }
                else if(i < 12)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeII, CreateId(), 2));
                }
                else if (i < 17)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeII, CreateId(), 3));
                }
                else if (i < 21)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeII, CreateId(), 4));
                }
                else
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeII, CreateId(), 5));
                }
            }
            /*
            SOLDER
            */
            for (i=0; i<cardCounts[GameTileTypes.solder]; i++)
            {
                resistorSolderDeck.Push(new GameTile(GameTileTypes.solder, CreateId()));
            }

            /*
            SHUFFLE
            */
            Shuffle(wireDeck);
            Shuffle(wireDeck);

            Shuffle(resistorSolderDeck);
            Shuffle(resistorSolderDeck);

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
    }
}
