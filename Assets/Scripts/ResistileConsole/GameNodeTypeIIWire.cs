namespace ResistileConsole
{
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
}