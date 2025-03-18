using UnityEngine;
using UnityEngine.Events;

namespace Characters.Player.PlayerBehaviour
{
    [CreateAssetMenu(menuName = "Player/HidePort")]
    public class HidePort : ScriptableObject
    {
        public UnityAction<bool> OnHidden = delegate(bool isHidden){ };

        public void Hidden(bool isHidden)
        {
            OnHidden(isHidden);
        }
    }
}
