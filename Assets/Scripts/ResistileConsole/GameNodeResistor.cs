namespace ResistileConsole
{
    class GameNodeResistor : GameNode
    {
    }

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