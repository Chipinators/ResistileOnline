namespace ResistileConsole
{
    class GameNodeTypeIResistor : GameNodeResistor
    {
        public GameNodeTypeIResistor(double resistance)
        {
            Up = null;
            Down = null;
            Left = new BlockedDirection();
            Right = new BlockedDirection();
            Resistance = resistance;
        }
    }
}