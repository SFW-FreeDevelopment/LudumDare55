using System;
using UnityEngine;

namespace LD55.Models
{
    [Serializable]
    public class BattleState
    {
        public bool InputLocked { get; set; }
        public SubMenu? CurrentMenu { get; set; }
        public bool WaitingForPlayerInput { get; set; } = true;
        public BattleEnemyModel Enemy { get; set; }
        public Party PlayerParty { get; set; }
        public int CurrentIndex { get; set; }
        public MonsterInstance CurrentMonster => PlayerParty.Monsters[CurrentIndex];
    }

    public enum SubMenu
    {
        Fight,
        Party,
        Items
    }
}
