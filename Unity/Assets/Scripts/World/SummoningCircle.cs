using LD55.Behaviors;
using LD55.Managers;
using LD55.Models;
using LD55.ScriptableObjects;
using UnityEngine;

namespace LD55.World
{
    public class SummoningCircle : TriggerTileBase
    {
        private static Monster[] Monsters;
        
        protected override void Action()
        {
            Monsters ??= Resources.LoadAll<Monster>("Monsters");
            var idx = Random.Range(0, Monsters.Length);
            var monster = Monsters[idx];
            DialogueManager.Instance.Show(new DialogueModel
            {
                Name = monster.Name,
                Text = monster.PreBattleDialogue,
                Sprite = monster.Image,
                IsTrainer = false,
                Action = () => {
                    DialogueManager.Instance.IsTalking = false;
                    BattleManager.Instance.Show(new BattleEnemyModel
                    {
                        Name = monster.Name,
                        Party = new Party
                        {
                            Monsters = new[] {
                                new MonsterInstance
                                {
                                    Monster = monster,
                                    Level = (byte)Random.Range(1, 6),
                                    MaxHealth = 100,
                                    CurrentHealth = 100
                                }
                            }
                        }
                    });
                }
            });
        }
    }
}
