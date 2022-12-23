using System;
using Games.LogicAndColoring.Scripts.Hints;
using Games.LogicAndColoring.Scripts.Pictures;
using UnityEngine;


public enum DifficultyTasks
{
    Image,
    Numbers,
    Calculation
}


namespace Games.LogicAndColoring.Scripts.GameController.GameDifficulty
{
    [Serializable]
    public abstract class Difficulty : ScriptableObject, IIterator
    {
        [SerializeField] protected Gradient colorsOnDifficulty;
        [SerializeField] private int colorsCount;
        [SerializeField] protected Vector3 positionForPicture;
        [SerializeField] protected Picture[] pictures;
            //[SerializeField] protected Transform spawnArea;
        [SerializeField] private DifficultyTasks task;
        public int CurrentIndex { get; set; } = 0;

    
        public Picture GetPicture() => pictures[CurrentIndex];

    
        public Gradient GetGradient() => colorsOnDifficulty;


        public int GetColorsCount() => colorsCount;
    
    
        public Vector3 GetPicturePosition() => positionForPicture;

        public string GetTask() => task.ToString();
    
    
        public bool Next()
        {
            var res = false;
            if (CurrentIndex < pictures.Length - 1)
            {
                CurrentIndex++;
                res = true;
            }

            return res;
        }

    
        public bool Previous()
        {
            var res = false;
            if (CurrentIndex > 0)
            {
                CurrentIndex--;
                res = true;
            }

            return res;
        }

    
        public void Reset()
        {
            CurrentIndex = 0;
        }

    
        public abstract IHintView GetHintView();
    }
}