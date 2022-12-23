using UnityEngine;

namespace Games.LogicAndColoring.Scripts.Pictures.Animations
{
    public class AnimationDestroyer : MonoBehaviour
    {
        
        public void DestroyAnimatedPicture()
        {
            Destroy(gameObject);
        }
    }
}