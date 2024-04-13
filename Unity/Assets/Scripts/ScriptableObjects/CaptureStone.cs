using UnityEngine;

namespace LD55.ScriptableObjects
{
    [CreateAssetMenu(menuName = "LD55/Capture Stone")]
    public class CaptureStone : Item
    {
        [SerializeField] private byte _catchRateModifier;
        public byte CatchRateModifier => _catchRateModifier;
    }
}
