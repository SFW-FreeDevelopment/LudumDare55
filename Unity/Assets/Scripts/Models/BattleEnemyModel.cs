using System;

namespace LD55.Models
{
    [Serializable]
    public class BattleEnemyModel
    {
        public string Name { get; set; }
        public Party Party { get; set; }
    }
}
