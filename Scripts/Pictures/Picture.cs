using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Games.LogicAndColoring.Scripts.ColorPalette;
using Games.LogicAndColoring.Scripts.GameController;
using UnityEngine;

namespace Games.LogicAndColoring.Scripts.Pictures
{
    internal enum SelectedColorsState
    {
        True,
        False
    }
    
    public class Picture : MonoBehaviour, IPicture
    {
        private static readonly int PassLevelTrigger = Animator.StringToHash("PassLevelTrigger");
        [SerializeField] private int countPaints;
        [SerializeField] private Sprite example;
        [SerializeField] private Animator protoWithAnim;
        private Details[] _details;
        private List<List<Details>> _detailsListToID;   //todo: В МАССИВ ДВУМЕРНЫЙ
        private Dictionary<Details, bool> _resultsColorDetail;
        private SelectedColorsState _previousState;
        private float _checkTimer;
        private bool _timerOff;

        private float CheckTimer
        {
            get => _checkTimer;
            set
            {
                if (_previousState == SelectedColorsState.False)
                {
                    _checkTimer = 0;
                }
                if (_previousState == SelectedColorsState.True)
                {
                    _checkTimer = value;
                }
            }
        }
        
        public event Action SelectedTrueColors;
        public event Action SelectedFalseColors;
        public event Action Unload;


        public Sprite GetSprite() => example;

        
        public int GetPaintsCount() => countPaints;


        private void Awake()
        {
            _details = GetComponentsInChildren<Details>();
        }


        private void Update()
        {
            TryPassLevelOnTime();
        }

        private void TryPassLevelOnTime()
        {
            CheckTimer += Time.deltaTime;
            _previousState = CheckColors() ? SelectedColorsState.True : SelectedColorsState.False;
            if (CheckTimer > 3 && !_timerOff)
            {
                Debug.Log("True colors");
                SelectedTrueColors?.Invoke();
                CheckTimer = 0;
                _timerOff = true;
            }
        }

        private bool CheckColors() => _resultsColorDetail.All(x => x.Value);


        public IEnumerator AwaitAnimation()
        {
            var anim = Instantiate(protoWithAnim, protoWithAnim.transform.position, Quaternion.identity);
            anim.SetTrigger(PassLevelTrigger);
            var duration = anim.runtimeAnimatorController.animationClips[0].averageDuration;    //todo: по-другому получать длительность анимации
            yield return new WaitForSeconds(duration);
        }


        public void PassLevel()
        {
            Debug.Log(CheckColors() ? "True colors" : "False colors");
            SelectedTrueColors?.Invoke();
        }

    
        public SortedDictionary<int, float> GetColors()
        {
            var detailsData = new SortedDictionary<int, float>();
            foreach (var detail in _details)
            {
                if (!detailsData.ContainsKey(detail.GetNumber()))
                {
                    detailsData.Add(detail.GetNumber(), detail.GetTrueColor());
                }
            }

            return detailsData;
        }

        
        public void LoadLevel(IPaint[] paints)
        {
            FillDetailsList();
            FillDetailsBoolDictionary();
            SubscribeOnPaints(paints);
        }


        public void UnloadLevel(IPaint[] paints)
        {
            Unload?.Invoke();
            UnsubscribeFromPaints(paints);
            DestroyPicture();
            _detailsListToID.Clear();
            _resultsColorDetail.Clear();
        }

    
        private void FillDetailsList()  //todo: лист в двумерный массив
        {
            _detailsListToID = new List<List<Details>>();
        
            for (var i = 0; i < countPaints; i++)
            {
                _detailsListToID.Add(new List<Details>());
            }

            foreach (var detail in _details)
            {
                var id = detail.GetID();
                if (id > countPaints) throw new ArgumentException("Detail in picture has ID greater than paints count");
                _detailsListToID[id].Add(detail);
            }
        }
    
    
        private void FillDetailsBoolDictionary()
        {
            _resultsColorDetail = new Dictionary<Details, bool>(_details.Length);

            foreach (var t in _details)
            {
                _resultsColorDetail.Add(t, false);
            }
        }

    
        private void SubscribeOnPaints(IPaint[] paints)
        {
            foreach (var paint in paints)
            {
                paint.PaintColorChanged += ChangeColorOnPaint;
            }
        }

    
        private void UnsubscribeFromPaints(IPaint[] paints)
        {
            foreach (var paint in paints)
            {
                paint.PaintColorChanged -= ChangeColorOnPaint;
            }
        }

        private void ChangeColorOnPaint(int id, float percentage, Color color, IPaint paint)
        {
            for (var i = 0; i < _detailsListToID[id].Count; i++)
            {
                var detail = _detailsListToID[id][i];
                var isCorrect = detail.ChangeColor(color, percentage);
                _resultsColorDetail[detail] = isCorrect;
                if (isCorrect)
                {
                    TrySetColorInCircle(paint);
                }
                else
                {
                    paint.CheckColorCircleTimer = 0;
                }
            }
        }

        private void TrySetColorInCircle(IPaint paint)
        {
            paint.CheckColorCircleTimer += Time.deltaTime;
            if (paint.CheckColorCircleTimer > 3 && !paint.TimerColorCircleOff)
            {
                paint.TimerColorCircleOff = true;
                paint.SetColorAsTrue();
            }
        }

        private void DestroyPicture()
        {
            Destroy(gameObject);
        }
    }
}