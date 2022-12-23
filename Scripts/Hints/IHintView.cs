using UnityEngine;

namespace Games.LogicAndColoring.Scripts.Hints
{
    public interface IHintView
    {
        void Spawn(Transform transform);
        bool Change(int index);
        void Destroy();
    }
}