using System;
using LD55.ScriptableObjects;
using Newtonsoft.Json;
using UnityEngine;

namespace LD55.Models
{
    [Serializable]
    public class MonsterInstance
    {
        [SerializeField] private Monster _monster;
        [JsonIgnore] public Monster Monster => _monster;

        public string Id => Monster.Id.ToString();
        public byte Level { get; set; }
        public int Experience { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public bool IsKO => CurrentHealth == 0;

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth < 0)
                CurrentHealth = 0;
        }

        public void Heal(int health)
        {
            CurrentHealth += health;
            if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;
        }
    }
}
