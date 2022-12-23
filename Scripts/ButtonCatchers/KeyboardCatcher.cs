using System;
using Games.LogicAndColoring.Scripts.GameController;
using UnityEngine;

namespace Games.LogicAndColoring.Scripts.ButtonCatchers
{
    public class KeyboardCatcher : MonoBehaviour, IUserActionsCatcher
    {
        public event Action ColorCheck;
        public event Action NextLevel;
        public event Action PreviousLevel;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ColorCheck?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextLevel?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PreviousLevel?.Invoke();
            }
        }
    }
}