using LD55.Behaviors;
using LD55.Managers;
using LD55.Models;
using UnityEngine;

namespace LD55.World
{
    public class RoamingMonster : TriggerTileBase
    {
        [SerializeField] private MonsterInstance _data;
        public MonsterInstance Data => _data;

        [SerializeField] private bool _hasFought;
        public bool HasFought => _hasFought;
        
        private void Awake()
        {
            GetComponent<SpriteRenderer>().sprite = Data.Monster.Sprite;
        }
        
        protected override void Action()
        {
            Debug.Log("Trainer encountered!");
            DialogueManager.Instance.IsTalking = true;
            if (HasFought)
            {
                DialogueManager.Instance.Show(new DialogueModel
                {
                    Name = Data.Monster.Name,
                    Text = Data.Monster.PostBattleDialogue,
                    Sprite = Data.Monster.Image,
                    IsTrainer = false,
                    Action = () => {
                        DialogueManager.Instance.IsTalking = false;
                    }
                });
            }
            else
            {
                DialogueManager.Instance.Show(new DialogueModel
                {
                    Name = Data.Monster.Name,
                    Text = Data.Monster.PreBattleDialogue,
                    Sprite = Data.Monster.Image,
                    IsTrainer = false,
                    Action = () => {
                        DialogueManager.Instance.IsTalking = false;
                        BattleManager.Instance.Show(new BattleEnemyModel
                        {
                            Name = Data.Monster.Name,
                            Party = new Party
                            {
                                Monsters = new[] { _data }
                            }
                        });
                    }
                });
            }
        }
    }
}
