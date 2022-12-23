using UnityEngine;

namespace Games.LogicAndColoring.Scripts.ColorPalette
{
    public interface IPaintFactory
    {
        IPaint GetPaint(Vector2 pos, Transform parent, float prefabSize, Gradient gradient, float sensitivity);
    }
}