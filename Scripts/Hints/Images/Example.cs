using Games.LogicAndColoring.Scripts.Pictures;
using UnityEngine;
using UnityEngine.UI;

namespace Games.LogicAndColoring.Scripts.Hints.Images
{
    public class Example : IHintView
    {
        private readonly Picture[] _pictures;
        private Image _hint;


        public Example(Picture[] pictures)
        {
            _pictures = pictures;
        }
    
    
        // public void Spawn(Picture picture)
        // {
        //     var example = _pictureImage.GetSprite();
        //     //var size = _hint.size;
        //     _hint.color = Color.white;
        //     _hint.sprite = example;
        //     //_hint.size = size;
        //     picture.Unload += Unload;
        // }

        public void Spawn(Transform transform)
        {
            var image = Object.Instantiate(new GameObject(), Vector3.zero, transform.rotation, transform as RectTransform);
            _hint = image.AddComponent<Image>();
        }

        public bool Change(int index)
        {
            var res = index >= 0 && index < _pictures.Length;
            if (res)
            {
                _hint.sprite = _pictures[index].GetSprite();
                _hint.preserveAspect = true;
            }

            return res;
        }

        public void Destroy()
        {
            Object.Destroy(_hint.gameObject);
        }
    }
}