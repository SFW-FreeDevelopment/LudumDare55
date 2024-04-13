using LD55.Enums;
using UnityEngine;

namespace LD55.ScriptableObjects
{
    [CreateAssetMenu(menuName = "LD55/Monster")]
    public class Monster : ScriptableObject
    {
        [SerializeField] private byte _id;
        public byte Id => _id;
        
        [SerializeField] private string _name;
        public string Name => _name;
        
        [SerializeField] private string _description;
        public string Description => _description;
        
        [SerializeField] private MonsterType _type;
        public MonsterType Type => _type;
        
        [SerializeField] private ushort _baseExperience;
        public ushort BaseExperience => _baseExperience;
        
        [SerializeField] private byte _catchRate;
        public byte CatchRate => _catchRate;
    }
}