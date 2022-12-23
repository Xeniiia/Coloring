using Games.LogicAndColoring.Scripts.Pictures;
using UnityEngine;

namespace Games.LogicAndColoring.Scripts.GameController
{
    public interface IPaletteControl
    {
        void GeneratePalette(int paintsCount, Gradient gradient, int colorsCount);
        void LoadLevel(Picture picture);
        void UnloadLevel(Picture picture);
    }
}