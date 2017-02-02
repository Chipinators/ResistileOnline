namespace ResistileConsole
{
    class GameNodeTypeIIResistor : GameNodeResistor
    {
        public GameNodeTypeIIResistor(double resistance)
        {
            Up = new BlockedDirection();
            Down = null;
            Left = null;
            Right = new BlockedDirection();
            Resistance = resistance;
        }
    }
}