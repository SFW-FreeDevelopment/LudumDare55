using LD55.Enums;
using UnityEngine;

namespace LD55.ScriptableObjects
{
    [CreateAssetMenu(menuName = "LD55/Battle Move")]
    public class BattleMove : ScriptableObject
    {
        [SerializeField] private string _name;
        public string Name => _name;
        
        [SerializeField] private string _description;
        public string Description => _description;
        
        [SerializeField] private BattleMoveCategory _category;
        public BattleMoveCategory Category => _category;
        
        [SerializeField] private MonsterType _moveType;
        public MonsterType MoveType => _moveType;
        
        [SerializeField] private byte _damage;
        public byte Damage => _damage;
        
        [SerializeField] private byte _accuracy = 100;
        public byte Accuracy => _accuracy;
    }
}
