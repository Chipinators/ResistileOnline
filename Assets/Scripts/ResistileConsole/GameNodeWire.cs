namespace ResistileConsole
{
    class GameNodeWire : GameNode
    {
        public GameNodeWire()
        {
            Resistance = double.Epsilon;
        }
    }

    class GameNodeTypeIWire : GameNodeWire
    {
        public GameNodeTypeIWire()
        {
            Up = null;
            Down = null;
            Left = new BlockedDirection();
            Right = new BlockedDirection();        }
    }
    class GameNodeTypeIIWire : GameNodeWire
    {
        public GameNodeTypeIIWire()
        {
            Up = new BlockedDirection();
            Down = null;
            Left = null;
            Right = new BlockedDirection();
        }
    }
    class GameNodeTypeTWire : GameNodeWire
    {
        public GameNodeTypeTWire()
        {
            Up = new BlockedDirection();
            Down = null;
            Left = null;
            Right = null;
            CurrentIndex = -1;
        }
    }

}