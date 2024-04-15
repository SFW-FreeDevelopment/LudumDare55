using System;
using LD55.Enums;
using LD55.Models;
using TMPro;
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
        
        [SerializeField] private Sprite _sprite;
        public Sprite Sprite => _sprite;
        
        [SerializeField] private Sprite _image;
        public Sprite Image => _image;
        
        [SerializeField] private MonsterType _type;
        public MonsterType Type => _type;
        
        [SerializeField] private ushort _baseExperience;
        public ushort BaseExperience => _baseExperience;
        
        [SerializeField] private byte _catchRate;
        public byte CatchRate => _catchRate;
        
        [SerializeField] private byte _speed = 100;
        public byte Speed => _speed;

        [SerializeField] private LearnableMove[] _learnableMoves = Array.Empty<LearnableMove>();
        public LearnableMove[] LearnableMoves => _learnableMoves;

        [SerializeField] private string _preBattleDialogue;
        public string PreBattleDialogue => _preBattleDialogue;
        
        [SerializeField] private string _postBattleDialogue;
        public string PostBattleDialogue => _postBattleDialogue;
    }
}
