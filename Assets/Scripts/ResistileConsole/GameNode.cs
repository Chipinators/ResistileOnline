namespace ResistileConsole
{
    class GameNode : IGameNode
    {
        public IGameNode Up { get; set; }
        public IGameNode Down { get; set; }
        public IGameNode Left { get; set; }
        public IGameNode Right { get; set; }
        public double Resistance { get; set; }

        public int CurrentIndex { get; set; }

        private IGameNode accessedBy; 
        public double ResistanceWithDirection(IGameNode accessor)
        {
            if (accessedBy == null)
            {
                accessedBy = accessor;
                return Resistance;
            }
            if(accessedBy == accessor)
                return Resistance;
            return -Resistance;
        }
        public GameNode()
        {
            CurrentIndex = -1;
        }

        public void Rotate(bool clockwiseDirection = true)
        {
            IGameNode temp;
            if (clockwiseDirection)
            {
                temp = Up;
                Up = Left;
                Left = Down;
                Down = Right;
                Right = temp;
            }
            else
            {
                temp = Up;
                Up = Right;
                Right = Down;
                Down = Left;
                Left = temp;
            }
        }
    }
}