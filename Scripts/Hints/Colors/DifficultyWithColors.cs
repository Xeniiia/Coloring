using UnityEngine;

namespace Games.LogicAndColoring.Scripts.Hints.Colors
{
    [CreateAssetMenu(fileName = nameof(GameController.GameDifficulty.Difficulty), menuName = "LogicAndColoring Difficulty Data" + "/With Colors Examples")]
    public class DifficultyWithColors : GameController.GameDifficulty.Difficulty
    {
        [SerializeField] private ColorCircle proto;
        public override IHintView GetHintView() => new Colors(proto, colorsOnDifficulty, pictures);
    }
}


// todo: один ScrollView для красок и имейджа
// todo: новый ReferenceField
// todo: одна SpawnArea (как поле в Hint) - Content со ScrollView (см выше)
// todo: чото еще я забыла 
// todo: pictures в IHintView
// todo: Change вместо спавна

// Сделать реализацию IHintView на установку цветов
// В реализации IHintView в методе Spawn спавн объекта, отображающего подсказку (цвета)
// При вызове метода Change в IHintView реализовать смену состояния объекта отображения подсказки
// При вызове метода Destroy в IHintView реализовать удаления объекта отображения подсказки
// Для объекта отображения подсказки цвета сделать класс управления смены цвета в шариках 

// Сделать реализацию интерфейса IHintView методами Spawn(Transform), Change(Int), Destroy().
// Реализовать переключение подсказок вперед-назад и смену подсказок через _hintView.Change()
// Сделать вызов _hintView.Destroy() в вызове Reset
// Сделать реализацию IHintView на установку картинок
// В реализации IHintView в методе Spawn спавн объекта, отображающего подсказку (картинка)