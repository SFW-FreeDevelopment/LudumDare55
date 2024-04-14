using System;
using UnityEngine;

namespace LD55.Models
{
    [Serializable]
    public class Party
    {
        [SerializeField] private MonsterInstance[] _monsters;
        public MonsterInstance[] Monsters => _monsters;
    }
}
