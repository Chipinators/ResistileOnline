namespace ResistileConsole
{
    class GameNodeTypeRoot : GameNode
    {
        public GameNodeTypeRoot()
        {
            Down = new BlockedDirection();
            Left = new BlockedDirection();
            Right = null;
            Up = new BlockedDirection();
            Resistance = double.Epsilon;
        }

    }

    class GameNodeTypeEnd : GameNode
    {
        public GameNodeTypeEnd()
        {
            Down = new BlockedDirection();
            Right = new BlockedDirection();
            Left = new BlockedDirection();
            Up = null;
            Resistance = double.Epsilon;
        }
    }
}