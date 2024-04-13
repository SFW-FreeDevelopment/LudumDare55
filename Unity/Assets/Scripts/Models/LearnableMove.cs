using System;
using LD55.ScriptableObjects;
using UnityEngine;

namespace LD55.Models
{
    [Serializable]
    public class LearnableMove
    {
        [SerializeField] private BattleMove _move;
        public BattleMove Move => _move;

        [SerializeField] private byte _level;
        public byte Level => _level;
    }
}
