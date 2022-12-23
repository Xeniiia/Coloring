using Games.LogicAndColoring.Scripts.GameController.GameDifficulty;
using UnityEngine;
using UnityEngine.UI;

namespace Games.LogicAndColoring.Scripts.Hints.Images
{
    [CreateAssetMenu(fileName = nameof(GameController.GameDifficulty.Difficulty), menuName = "LogicAndColoring Difficulty Data" + "/With Image")]
    public class DifficultyWithImage : GameController.GameDifficulty.Difficulty
    {
        public override IHintView GetHintView() => new Example(pictures);
    }
}