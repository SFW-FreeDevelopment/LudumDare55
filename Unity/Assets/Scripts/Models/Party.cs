using System;
using UnityEngine;

namespace LD55.Models
{
    [Serializable]
    public class Party
    {
        [SerializeField] private MonsterInstance[] _monsters = Array.Empty<MonsterInstance>();
        public MonsterInstance[] Monsters
        {
            get => _monsters;
            set => _monsters = value;
        }
    }
}
