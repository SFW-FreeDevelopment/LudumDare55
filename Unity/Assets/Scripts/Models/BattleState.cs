using System;
using UnityEngine;

namespace LD55.Models
{
    [Serializable]
    public class BattleState
    {
        public SubMenu? CurrentMenu { get; set; }
        public bool WaitingForPlayerInput { get; set; } = true;
        public BattleEnemyModel Enemy { get; set; }
    }

    public enum SubMenu
    {
        Fight,
        Party,
        Items
    }
}
