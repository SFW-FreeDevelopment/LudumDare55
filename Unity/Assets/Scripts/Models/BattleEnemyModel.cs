using System;

namespace LD55.Models
{
    [Serializable]
    public class BattleEnemyModel
    {
        public string Name { get; set; }
        public Party Party { get; set; }
        public int CurrentIndex { get; set; }
        public MonsterInstance CurrentMonster => Party.Monsters[CurrentIndex];
    }
}
