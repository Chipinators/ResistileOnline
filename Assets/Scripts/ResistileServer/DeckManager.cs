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
        public Stack<GameTile> wireHand = new Stack<GameTile>();
        public Stack<GameTile> tileHand = new Stack<GameTile>();
        private Stack<GameTile> wireDeck = new Stack<GameTile>();
        private Stack<GameTile> resistorSolderDeck = new Stack<GameTile>();
        private static Random rng = new Random();

        static private Dictionary<String, int> cardCountIndexes = new Dictionary<String, int>
            {
            {GameTileTypes.Wire.typeI, 14},
            {GameTileTypes.Wire.typeII, 28},
            {GameTileTypes.Wire.typeT, 39},
            {GameTileTypes.Resistor.typeI, 63},
            {GameTileTypes.Resistor.typeII, 87},
            {GameTileTypes.solder, 87}
            };

        public DeckManager()
        {
            int i;
            /*
            WIRES
            */
            for (i=0; i<cardCountIndexes[GameTileTypes.Wire.typeI]; i++) //Straight Wires
            {
                wireDeck.Push(new GameTile(GameTileTypes.Wire.typeI, i));
            }
            for (i=cardCountIndexes[GameTileTypes.Wire.typeI]; i<cardCountIndexes[GameTileTypes.Wire.typeII]; i++) //Angle Wires
            {
                wireDeck.Push(new GameTile(GameTileTypes.Wire.typeII, i));
            }
            for (i=cardCountIndexes[GameTileTypes.Wire.typeII]; i<cardCountIndexes[GameTileTypes.Wire.typeT]; i++) //T Wires
            {
                wireDeck.Push(new GameTile(GameTileTypes.Wire.typeT, i));
            }
            /*
            STRAIGHT RESISTORS
            */
            for (i=cardCountIndexes[GameTileTypes.Wire.typeT]; i<cardCountIndexes[GameTileTypes.Resistor.typeI]; i++)
            {
                if (i - cardCountIndexes[GameTileTypes.Wire.typeT] < 6)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeI, i, 1));
                }
                else if(i - cardCountIndexes[GameTileTypes.Wire.typeT] < 12)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeI, i, 2));
                }
                else if(i - cardCountIndexes[GameTileTypes.Wire.typeT] < 17)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeI, i, 3));
                }
                else if(i - cardCountIndexes[GameTileTypes.Wire.typeT] < 21)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeI, i, 4));
                }
                else
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeI, i, 5));
                }
            }
            /*
            ANGLE RESISTORS
            */
            for (i=cardCountIndexes[GameTileTypes.Resistor.typeI]; i<cardCountIndexes[GameTileTypes.Resistor.typeII]; i++)
            {
                if(i - cardCountIndexes[GameTileTypes.Resistor.typeI] < 6)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeII, i, 1));
                }
                else if(i - cardCountIndexes[GameTileTypes.Resistor.typeI] < 12)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeII, i, 2));
                }
                else if (i - cardCountIndexes[GameTileTypes.Resistor.typeI] < 17)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeII, i, 3));
                }
                else if (i - cardCountIndexes[GameTileTypes.Resistor.typeI] < 21)
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeII, i, 4));
                }
                else
                {
                    resistorSolderDeck.Push(new GameTile(GameTileTypes.Resistor.typeII, i, 5));
                }
            }
            /*
            SOLDER
            */
            for (i=cardCountIndexes[GameTileTypes.solder]; i<MAX; i++)
            {
                resistorSolderDeck.Push(new GameTile(GameTileTypes.solder, i));
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
    }
}
