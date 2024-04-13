using UnityEngine;

namespace LD55.ScriptableObjects
{
    [CreateAssetMenu(menuName = "LD55/Potion")]
    public class Potion : Item
    {
        [SerializeField] private ushort _health;
        public ushort Health => _health;
    }
}
