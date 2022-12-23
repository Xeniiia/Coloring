using System;
using UnityEngine;

namespace Games.LogicAndColoring.Scripts.ColorPalette
{
    public interface IPaint
    {
        event Action<int, float, Color, IPaint> PaintColorChanged;
        int ID { get; set; }
        void DestroyPaint();
        void SetColorAsTrue();

        float CheckColorCircleTimer { get; set; }
        bool TimerColorCircleOff { get; set; }
    }
}