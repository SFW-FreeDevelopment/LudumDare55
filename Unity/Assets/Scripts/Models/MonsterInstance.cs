using System;

namespace LD55.Models
{
    [Serializable]
    public class MonsterInstance
    {
        public string Id { get; set; }
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
