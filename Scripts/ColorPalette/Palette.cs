using System;
using System.Collections.Generic;
using Games.LogicAndColoring.Scripts.GameController;
using Games.LogicAndColoring.Scripts.Pictures;
using UnityEngine;

namespace Games.LogicAndColoring.Scripts.ColorPalette
{
    public class Palette : MonoBehaviour, IPaletteControl
    {
        [SerializeField] private float height = 10;
        [SerializeField] private float width = 18;
        [SerializeField] private float intervalForOneColor = 10;
        private IPaintFactory _paintFactory;
        private List<IPaint> _paints;
    

        private void Awake()
        {
            _paintFactory = GetComponent<IPaintFactory>();
        }

        
        public void LoadLevel(Picture picture)
        {
            picture.LoadLevel(_paints.ToArray());
        }

        
        public void UnloadLevel(Picture picture)
        {
            picture.UnloadLevel(_paints.ToArray());
            ClearPalette();
        }

        private void ClearPalette()
        {
            foreach (var paint in _paints)
            {
                paint.DestroyPaint();
            }

            _paints.Clear();
        }

    
        public void GeneratePalette(int paintsCount, Gradient gradient, int colorsCount)
        {
            var pos = CalculateStartPosition();
            var step = CalculateStepOnGeneration(pos, paintsCount);
            var paintSize = CalculatePaintSize(step);
            _paints = new List<IPaint>();

            var sensitivity = colorsCount * intervalForOneColor;
            pos = new Vector2(pos.x + step / 2f, pos.y);
            for (var i = 0; i < paintsCount; i++)
            {
                var paint = _paintFactory.GetPaint(pos, transform, paintSize, gradient, sensitivity);
                paint.ID = i;
                _paints.Add(paint);
                pos.x += step;
            }
        }

        private Vector2 CalculateStartPosition()
        {
            var startX = 0 - width / 3f;
            var startY = 0 - height / 3f;

            return new Vector2(startX, startY);
        }

        private float CalculateStepOnGeneration(Vector2 startPos, int paintsCount)
        {
            var widthForPaints = width - Math.Abs(width / 2f - Math.Abs(startPos.x)) * 2f;
            var widthForOnePaint = widthForPaints / paintsCount;

            return widthForOnePaint;
        }

        private static float CalculatePaintSize(float step) => step / 2f * 0.6f;
    }
}