using UnityEngine;

namespace Games.LogicAndColoring.Scripts.ColorPalette.Paints
{
    public class PaintFactory : MonoBehaviour, IPaintFactory
    {
        [SerializeField] private Paint proto;
        [SerializeField] private float possiblePaintRotation;
        [SerializeField] private Vector2 possiblePaintOffset;

        public IPaint GetPaint(Vector2 pos, Transform parent, float prefabSize, Gradient gradient,
            float sensitivity)
        {
            var rotation = CalculateRandomRotation();
            var posWithOffset = CalculatePositionWithRandomOffset(pos);
            var paint = Instantiate(proto, posWithOffset, rotation, parent);
            var size = new Vector3(prefabSize, prefabSize, 0);
            paint.transform.localScale = size;
            paint.Init(gradient, sensitivity);
        
            return paint;
        }

        private Quaternion CalculateRandomRotation()
        {
            var rotation = proto.transform.rotation *
                           Quaternion.Euler(0, 0, Random.Range(-possiblePaintRotation, possiblePaintRotation));
            
            return rotation;
        }

        private Vector2 CalculatePositionWithRandomOffset(Vector2 pos)
        {
            var offsetX = possiblePaintOffset.x;
            var offsetY = possiblePaintOffset.y;
            var posWithOffset = pos + new Vector2(Random.Range(-offsetX, offsetX), Random.Range(-offsetY, offsetY));
            
            return posWithOffset;
        }
    }
}