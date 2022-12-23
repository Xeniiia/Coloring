using System;

namespace Games.LogicAndColoring.Scripts.GameController
{
    public interface IUserActionsCatcher
    {
        event Action ColorCheck;
        event Action NextLevel;
        event Action PreviousLevel;
    }
}