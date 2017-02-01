namespace ResistileConsole
{
    interface IGameNode
    {
        IGameNode Up { get; set; }
        IGameNode Down { get; set; }
        IGameNode Left { get; set; }
        IGameNode Right { get; set; }
        double Resistance { get; set; }
        double ResistanceWithDirection(IGameNode accessor);
        int CurrentIndex { get; set; }
    }
}
