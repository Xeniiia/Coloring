using System;
using System.Collections;
using Games.LogicAndColoring.Scripts.GameController.GameDifficulty;
using UnityEngine;
using UnityEngine.UI;

namespace Games.LogicAndColoring.Scripts.Hints
{
    public class Hint : MonoBehaviour, IIterator, IHint
    {
        private static readonly int HintOpened = Animator.StringToHash("HintOpened");
        [SerializeField] private Animator referenceFieldWithAnimation;
        [SerializeField] private Transform spawnArea;
        [SerializeField] private Button button;
        private Coroutine _hintOpenCoroutine;
        private IHintView _hintView;
        private bool _isPressedZoomButton;
        public int CurrentIndex { get; set; } = -1;


        public bool Open() => _isPressedZoomButton;
        
    
        public void CreateHintView(IHintView hintView)
        {
            _hintView = hintView;
            _hintView.Spawn(spawnArea);
        }


        public bool Next()
        {
            return _hintView.Change(++CurrentIndex);
        }

        public bool Previous()
        {
            return _hintView.Change(--CurrentIndex);
        }

        public void Reset()
        {
            CurrentIndex = -1;
            if (_hintView == null) return;
            _hintView.Destroy();
        }


        public void OnFirst()
        {
            CurrentIndex = 0;
            _hintView.Change(CurrentIndex);
        }

        
        public void ZoomOnTime(Action callback)
        {
            if (_hintOpenCoroutine != null) StopCoroutine(_hintOpenCoroutine);
            _hintOpenCoroutine = StartCoroutine(ViewHint(callback));
        }

        private IEnumerator ViewHint(Action callback)
        {
            button.interactable = false;
            referenceFieldWithAnimation.SetBool(HintOpened, true);
            yield return new WaitForSeconds(3f);
            referenceFieldWithAnimation.SetBool(HintOpened, false);
            yield return new WaitForSeconds(2f);
            button.interactable = true;
            yield return null;
            callback();
        }


        public void Zoom()
        {
            if (_isPressedZoomButton)
            {
                referenceFieldWithAnimation.SetBool(HintOpened, false);
                _isPressedZoomButton = !_isPressedZoomButton;
            }
            else
            {
                referenceFieldWithAnimation.SetBool(HintOpened, true);
                _isPressedZoomButton = !_isPressedZoomButton;
            }
        }
    }
}