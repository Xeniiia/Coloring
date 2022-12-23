using System;
using Backend.Scripts.Main;
using OpenCVForUnity.CoreModule;
using UnityEngine;

namespace Games.LogicAndColoring.Scripts.ColorPalette.Paints
{
    public class Paint : MonoBehaviour, IPaint, ICell
    {
        [SerializeField] private SpriteRenderer shadeSprite;
        [SerializeField] private SpriteRenderer lightSprite;
        [SerializeField] private ParticleSystem trueColorSelectedParticles;
        private SpriteRenderer _spriteRenderer;
        private Gradient _gradient;
        private float _sensitivityDepth;
        private bool _colorCanChange = true;
        public RotatedRect Rectangle { get; set; }
        public int DefaultMiddleDepth { get; private set; }
        public int ID { get; set; }

        public float CheckColorCircleTimer { get; set; }
        public bool TimerColorCircleOff { get; set; }

        public event Action<int, float, Color, IPaint> PaintColorChanged;


        public void DestroyPaint() => Destroy(gameObject);
        public void SetColorAsTrue()
        {
            _colorCanChange = false;
            var psMain = trueColorSelectedParticles.main;
            psMain.startColor = _spriteRenderer.color;
            trueColorSelectedParticles.Play();
        }


        public int MiddleDepth => this.GetMiddleDepth();


        // ReSharper disable Unity.PerformanceAnalysis
        public void Init(Gradient gr, float depthHeight) //todo: напоминать о ините когда градиент может быть null (ошибкой)
        {
            this.GetRectangleFromWorld(transform.localScale);
            DefaultMiddleDepth = MiddleDepth;
            _gradient = gr;
            _sensitivityDepth = depthHeight;
        }


        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }


        private void Update()
        {
            if (_colorCanChange)
            {
                ResetColor(MiddleDepth);
            }
        }


        [Sirenix.OdinInspector.Button]
        private void ResetColor(float depth)
        {
            if (_gradient == null)
            {
                throw new ArgumentException( "Gradient not set, call Paint.Init(Gradient gradient, float depthHeight)");
            }

            var percent = ToPercentage(depth);
            var color = _gradient.Evaluate(percent);
            UpdatePaintColor(color);
            var luminance = color.grayscale;
            SetShadeOnPaint(luminance);
            PaintColorChanged?.Invoke(ID, percent, color, this);
        }


        private float ToPercentage(float depth)
        {
            var startDepth = DefaultMiddleDepth + _sensitivityDepth / 2f;
            if (depth > startDepth) depth = startDepth;
            if (depth < DefaultMiddleDepth - _sensitivityDepth / 2f)
                depth = DefaultMiddleDepth - _sensitivityDepth / 2f;
            var currentHeight = (depth < startDepth) ? Math.Abs(startDepth - depth) : Math.Abs(depth - startDepth);
            var percent = currentHeight / _sensitivityDepth;

            return percent;
        }

        private void UpdatePaintColor(Color newColor) => _spriteRenderer.color = newColor;


        private void SetShadeOnPaint(float luminance)
        {
            var shadeAlpha = 1 - luminance + 0.2f;
            var shadeSpriteColor = shadeSprite.color;
            shadeSpriteColor.a = shadeAlpha;
            shadeSprite.color = shadeSpriteColor;

            var lightAlpha = CalcLightAlpha();

            var lightSpriteColor = lightSprite.color;
            lightSpriteColor.a = lightAlpha;
            lightSprite.color = lightSpriteColor;
        }

        private float CalcLightAlpha()  //переделать бы..........
        {
            var shade = shadeSprite.color.a * 255;
            shade = shade > 255 ? 255 : shade;
            
            float lightAlpha = 50;
            if (shade > 225) lightAlpha = shade - 240;
            else if (shade > 200) lightAlpha = shade - 170;
            else if (shade > 150) lightAlpha = shade - 130;
            else if (shade > 100) lightAlpha = 150;
            else if (shade > 50) lightAlpha = 200;
            return lightAlpha / 255f;
        }


        [Sirenix.OdinInspector.Button]
        private void DepthInfoTest()
        {
            Debug.Log("Default Depth:" + DefaultMiddleDepth);
            Debug.Log("Middle Depth:" + MiddleDepth);
            Debug.Log("Start Depth:" + (DefaultMiddleDepth + _sensitivityDepth / 2f));
        }
    }
}