using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Games.LogicAndColoring.Scripts.Hints.Colors
{
    [Serializable]
    public class ColorCircle : MonoBehaviour
    {
        [SerializeField] private TMP_Text numberOfColor;
        private Image _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<Image>();
        }

        public void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }

        public void SetNumber(int num)
        {
            numberOfColor.text = " - " + num;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}