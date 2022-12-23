using System;
using System.Collections;
using System.Collections.Generic;
using Backend.Scripts.Calibration;
using Games.LogicAndColoring.Scripts.Hints;
using Games.LogicAndColoring.Scripts.Pictures;
using Games.Shapes.Scripts;
using Main.Menu.Scripts.Settings;
using Main.Scripts;
using Main.Scripts.Menu.Overlay;
using Main.Scripts.ScensGeneral;
using UnityEngine;

namespace Games.LogicAndColoring.Scripts.GameController
{
    public class GameController_LogicAndColoring : GameControllerBase
    {
        [SerializeField] private AudioSource win;
        [SerializeField] private AudioSource lose;
        [SerializeField] private List<GameDifficulty.Difficulty> difficulty; //todo: difficulty -> difficulties
        [SerializeField] private PaletteGradient imageWithGradient;
        [SerializeField] private Hint referenceField; //todo: интерфейс

        private static Func<IEnumerator> _kinectCalibration = WaitKinectCalibration;
        private IUserActionsCatcher _keyboardCatcher;
        private TaskController _taskController;
        private IPaletteControl _palette;
        private Coroutine _cellCoroutine;
        private Picture _currentPicture; //todo: интерфейс
        private bool _calibrationExists;
        private bool _firstLoad = true;
        private int _indexDiff;

        protected override void OnAwake() //todo
        {
            _palette = GetComponent<IPaletteControl>();
            _keyboardCatcher = GetComponent<IUserActionsCatcher>();
            _taskController = TaskController.NewInstance(this, "Games_LogicAndColoring", "Games_LogicAndColoring_Speech");
            InitDifficulty();
        }

        private void InitDifficulty()
        {
            MainSettings.AgeCategory = AgeCategory.Age1;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _keyboardCatcher.PreviousLevel += LoadPreviousLevel;
            _keyboardCatcher.NextLevel += LoadNextLevel; //todo: отписка
        }

        protected override void OnDelayedStart()
        {
            InitHint();
            _cellCoroutine = StartCoroutine(CellCoroutine());
            HideNeedlessDifficulties();
            MainSettings.AgeCategoryWasChanged += OnDifficultyChange;
        }

        private void InitHint()
        {
            referenceField.Reset();
            var hintView = difficulty[_indexDiff].GetHintView();
            referenceField.CreateHintView(hintView);
            referenceField.Next();
        }

        private static void HideNeedlessDifficulties()
        {
            if (TaskToolbar.Instance == null) return;
            TaskToolbar.Instance.HideAgeButtonByCategory(AgeCategory.Age2, false);
            TaskToolbar.Instance.HideAgeButtonByCategory(AgeCategory.Age3, false);
            TaskToolbar.Instance.HideAgeButtonByCategory(AgeCategory.Age4, false);
            TaskToolbar.Instance.HideAgeButtonByCategory(AgeCategory.Age5, false);
        }


        private void OnDifficultyChange(AgeCategory diff)
        {
            _indexDiff = AgeToDiff(diff);
            UnloadLevel();
            InitHint();
            ReloadLevel();
        }

        private int AgeToDiff(AgeCategory diff) => diff switch
        {
            AgeCategory.Age3 => 1,
            AgeCategory.Age5 => 2,
            _ => 0
        };

        private void UnloadLevel()
        {
            if (_currentPicture == null) return;
            _palette.UnloadLevel(_currentPicture);
            _currentPicture.SelectedTrueColors -= Win;
            _currentPicture.SelectedFalseColors -= Lose;
            _keyboardCatcher.ColorCheck -= CheckColors;
        }

        private void ReloadLevel()
        {
            if (_cellCoroutine != null) StopCoroutine(_cellCoroutine);
            _cellCoroutine = StartCoroutine(CellCoroutine());
        }


        protected override void CreateTask()
        {
            UnloadLevel();
            ReloadLevel();
        }

        private IEnumerator CellCoroutine()
        {
            if (_kinectCalibration != null)
                yield return _kinectCalibration();

            _currentPicture = CreatePicture();
            GeneratePalette(_currentPicture, difficulty[_indexDiff]);
            _palette.LoadLevel(_currentPicture);
            referenceField.ZoomOnTime(() => _keyboardCatcher.ColorCheck += CheckColors);
            LoadTaskOnLevel(difficulty[_indexDiff].GetTask());
            _currentPicture.SelectedTrueColors += Win;
            _currentPicture.SelectedFalseColors += Lose;
        }

        private static IEnumerator WaitKinectCalibration()
        {
            if (!KinectManager.Instance)
                yield return new WaitWhile(() => KinectManager.Instance == null);

            yield return new WaitWhile(() =>
                !KinectManager.Instance.IsInitialized() || !CalibrationDataHolder.CalibrationExists);

            _kinectCalibration = null;
        }

        private Picture CreatePicture()
        {
            var picture = difficulty[_indexDiff].GetPicture();
            var pos = difficulty[_indexDiff].GetPicturePosition();

            return Instantiate(picture, pos, Quaternion.identity);
        }

        private void GeneratePalette(Picture picture, GameDifficulty.Difficulty diff)
        {
            var colorsCount = diff.GetColorsCount();
            var paintsCount = picture.GetPaintsCount();
            var gradient = CreateGradient();
            _palette.GeneratePalette(paintsCount, gradient, colorsCount);
        }

        private Gradient CreateGradient()
        {
            var diff = difficulty[_indexDiff];
            var gradient = diff.GetGradient();
            imageWithGradient.EffectGradient = gradient;

            return gradient;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void LoadTaskOnLevel(string key)
        {
            _taskController.NewTask();
            _taskController.AddStringTask(key);
            if (_firstLoad)
            {
                _taskController.AddAudioTask(key);
                _firstLoad = false;
            }
            _taskController.SetTask();
        }


        private void CheckColors()
        {
            if (!referenceField.Open()) _currentPicture.PassLevel();
        }


        private void LoadNextLevel()
        {
            if (difficulty[_indexDiff].Next())
            {
                referenceField.Next();
                UnloadLevel();
                ReloadLevel();
            }
        }


        private void LoadPreviousLevel()
        {
            if (difficulty[_indexDiff].Previous())
            {
                referenceField.Previous();
                UnloadLevel();
                ReloadLevel();
            }
        }


        private void Win()
        {
            win.Play();
            StartCoroutine(PlayAnimAndLoadNextLevel());
        }

        private IEnumerator
            PlayAnimAndLoadNextLevel() //todo: переназвать и подумать КОСТЫЛЬНУЮ ОТПИСКУ УБРАТЬ И ВООБЩЕ ПРИБРАТЬ ТУТ ууууууууу
        {
            _currentPicture.SelectedTrueColors -= Win;
            _currentPicture.SelectedFalseColors -= Lose;
            _keyboardCatcher.ColorCheck -= CheckColors;
            _keyboardCatcher.PreviousLevel -= LoadPreviousLevel;
            _keyboardCatcher.NextLevel -= LoadNextLevel;
            yield return _currentPicture.AwaitAnimation();
            _keyboardCatcher.PreviousLevel += LoadPreviousLevel;
            _keyboardCatcher.NextLevel += LoadNextLevel;

            if (!difficulty[_indexDiff].Next())
            {
                difficulty[_indexDiff].Reset();
                referenceField.OnFirst();
            }
            else
            {
                referenceField.Next();
            }

            UnloadLevel();
            ReloadLevel();
        }


        private void Lose()
        {
            lose.Play();
        }


        protected override void OnDisabled()
        {
            MainSettings.AgeCategoryWasChanged -= OnDifficultyChange;
            _keyboardCatcher.ColorCheck -= CheckColors;
            _keyboardCatcher.NextLevel -= LoadNextLevel;
            _keyboardCatcher.PreviousLevel -= LoadPreviousLevel;
        }


        private void OnDestroy()
        {
            difficulty[_indexDiff].Reset();
        }
    }
}