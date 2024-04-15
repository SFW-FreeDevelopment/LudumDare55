using System;
using LD55.Models;
using UnityEngine;

namespace LD55.Models
{
    [Serializable]
    public class TrainerData
    {
        [SerializeField] private string _name;
        public string Name => _name;

        [SerializeField] private Sprite _sprite;
        public Sprite Sprite => _sprite;
        
        [SerializeField] private Sprite _image;
        public Sprite Image => _image;
        
        [SerializeField] private string _preBattleDialogue;
        public string PreBattleDialogue => _preBattleDialogue;
        
        [SerializeField] private string _postBattleDialogue;
        public string PostBattleDialogue => _postBattleDialogue;
        
        [SerializeField] private Party _party = new Party();
        public Party Party => _party;
        public bool IsDefeated { get; set; }
    }
}
