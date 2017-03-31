﻿using System;
using System.Collections.Generic;

/*
Changed Gametile ID to Int (was Guid)
*/
namespace ResistileServer
{
    class GameTile
    {
        public int id;
        private Dictionary<string, GameTile> neighbors = new Dictionary<string, GameTile>
        {
            {Directions.up, null},
            {Directions.down, null},
            {Directions.left, null},
            {Directions.right, null}
        };
        private double resistance;
        private int currentIndex = -1;
        public string type;
        
        public GameTile(string type, int id = 0, double resistance = double.Epsilon)
        {
            this.type = type;
            this.id = id;
            this.resistance = resistance;
            SetNeighbors(type);
            id = new int();
        }

        public GameTile Clone()
        {
            //clone a tile with a different id
            //usefull for temperory objects and path finding
            GameTile copy = new GameTile(type, id, resistance);
            copy.neighbors = new Dictionary<string, GameTile>(neighbors);
            copy.currentIndex = currentIndex;
            return copy;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var gameTile = (GameTile) obj;
            return gameTile != null && id == gameTile.id;
        }

        public static readonly GameTile blockedDirectionTile = new GameTile(GameTileTypes.blockedDirection, -1);
        private void SetNeighbors(string type)
        {
            switch (type)
            {
                case GameTileTypes.Resistor.typeI:
                case GameTileTypes.Wire.typeI:
                    neighbors[Directions.up] = blockedDirectionTile;
                    neighbors[Directions.down] = blockedDirectionTile;
                    break;
                case GameTileTypes.Resistor.typeII:
                case GameTileTypes.Wire.typeII:
                    neighbors[Directions.up] = blockedDirectionTile;
                    neighbors[Directions.right] = blockedDirectionTile;
                    break;
                case GameTileTypes.Wire.typeT:
                    neighbors[Directions.up] = blockedDirectionTile;
                    break;
                case GameTileTypes.blockedDirection:
                case GameTileTypes.solder:
                    break;
            }
        }
        
        public void Rotate()
        {
            //Rotate tile references clockwise
            GameTile temp = neighbors[Directions.up];
            neighbors[Directions.up] = neighbors[Directions.left];
            neighbors[Directions.left] = neighbors[Directions.down];
            neighbors[Directions.down] = neighbors[Directions.right];
            neighbors[Directions.down] = temp;
        }
    }

    public struct Directions
    {
        public const string up = "up";
        public const string down = "down";
        public const string left = "left";
        public const string right = "right";
    }

    public struct GameTileTypes
    {
        // IMPORTANT
        // When adding a new type
        // make sure each type has unique string
        public struct Resistor
        {
            public const string typeI = "ResistorTypeI";
            public const string typeII = "ResistorTypeII";
        }

        public struct Wire
        {
            public const string typeI = "WireTypeI";
            public const string typeII = "WireTypeII";
            public const string typeT = "WireTypeT";
        }
        public const string solder = "Solder";
        public const string blockedDirection = "blockedDirection";
    }
}