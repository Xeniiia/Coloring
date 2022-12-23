using UnityEngine;

namespace Games.LogicAndColoring.Scripts.Pictures
{
    public class Details : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField, Range(0, 1)] private float percentageOfTrueColor;
        [SerializeField, Range(0, 1)] private float percentageOffset;
        [SerializeField] private int numberOfDetail;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        
        public int GetID() => id;

        
        public int GetNumber() => numberOfDetail;

        
        public float GetTrueColor() => percentageOfTrueColor;

        
        public bool ChangeColor(Color color, float percentage)
        {
            _spriteRenderer.color = color;
            return CheckColor(percentage);
        }

        private bool CheckColor(float percentage)
        {
            return percentage <= (percentageOfTrueColor + percentageOffset) &&
                   percentage >= (percentageOfTrueColor - percentageOffset);
        }
    }
}