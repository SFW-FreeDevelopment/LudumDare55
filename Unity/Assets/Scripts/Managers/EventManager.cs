using UnityEngine.Events;

namespace LD55.Managers
{
    public class EventManager
    {
        public static event UnityAction OnPlayerMoved;
        public static void PlayerMoved() => OnPlayerMoved?.Invoke();
    }
}
