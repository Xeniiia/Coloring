using System.Collections.Generic;

namespace Games.LogicAndColoring.Scripts.GameController
{
    public interface IColors
    {
        SortedDictionary<int, float> GetColors();
    }
}