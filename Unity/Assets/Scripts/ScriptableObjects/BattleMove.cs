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

        private void Init(string name, string description, BattleMoveCategory category, MonsterType moveType, byte damage, byte accuracy)
        {
            this.name = name;
            this._description = description;
            this._category = category;
            this._moveType = moveType;
            this._damage = damage;
            this._accuracy = accuracy;
        }

        public BattleMove CreateBattleMoveFromItem(string name, string description, BattleMoveCategory category, MonsterType moveType, byte damage, byte accuracy)
        {
            var data = ScriptableObject.CreateInstance<BattleMove>();
            data.Init(name, description, category, moveType, damage, accuracy);
            return data;
        }
    }

}
