namespace ResistileConsole
{
    class GameNodeTypeIWire : GameNodeWire
    {
        public GameNodeTypeIWire()
        {
            Up = null;
            Down = null;
            Left = new BlockedDirection();
            Right = new BlockedDirection();        }
    }
}