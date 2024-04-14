using System;
using UnityEngine;

namespace LD55.Models
{
    [Serializable]
    public class BattleState
    {
        public bool WaitingForPlayerInput { get; set; } = true;
        public BattleEnemyModel Enemy { get; set; }
    }
}
