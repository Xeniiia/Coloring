using System.Collections.Generic;
using Games.LogicAndColoring.Scripts.GameController.GameDifficulty;
using Games.LogicAndColoring.Scripts.Pictures;
using UnityEngine;

namespace Games.LogicAndColoring.Scripts.Hints.Colors
{
    public class Colors : IHintView
    {
        private readonly RectTransform _spawnArea;
        private readonly ColorCircle _proto;
        private readonly Gradient _gradient;
        private List<ColorCircle> _examples;
        private Picture[] _pictures;


        public Colors(ColorCircle proto, Gradient colorsOnDifficulty, Picture[] pictures)
        {
            //_spawnArea = spawnArea as RectTransform;
            _proto = proto;
            _gradient = colorsOnDifficulty;
            _pictures = pictures;
        }
    
    
        public void Spawn(Picture picture)
        {
            var detailsTrueColors = picture.GetColors();
            _examples = new List<ColorCircle>();
        
            foreach (var detail in detailsTrueColors)
            {
                var newColorExample = Object.Instantiate(_proto, _spawnArea.position, Quaternion.identity, _spawnArea);
                newColorExample.SetNumber(detail.Key);
                newColorExample.SetColor(_gradient.Evaluate(detail.Value));
                _examples.Add(newColorExample);
            }

            picture.Unload += Unload;   //todo: отписка?? ?
        }

        public void Spawn(Transform transform)
        {
            throw new System.NotImplementedException();
        }

        public bool Change(int index)
        {
            throw new System.NotImplementedException();
        }

        public void Destroy()
        {
            throw new System.NotImplementedException();
        }

        private void Unload()
        {
            foreach (var c in _examples)
            {
                c.Destroy();
            }
        }
    }
}