namespace ResistileConsole
{
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