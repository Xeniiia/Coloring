namespace Games.LogicAndColoring.Scripts.GameController.GameDifficulty
{
    public interface IIterator
    {
        bool Next();
        bool Previous();
        void Reset();
        int CurrentIndex { get; set; }
    }
}