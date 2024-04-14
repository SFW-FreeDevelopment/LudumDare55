using System;
using LD55.Behaviors;
using LD55.Managers;
using LD55.Models;
using UnityEngine;

namespace LD55.World
{
    public class Trainer : TriggerTileBase
    {
        [SerializeField] private TrainerData _data;
        public TrainerData Data => _data;

        [SerializeField] private bool _hasFought;
        public bool HasFought => _hasFought;
        
        private void Awake()
        {
            GetComponent<SpriteRenderer>().sprite = Data.Sprite;
        }

        protected override void Action()
        {
            Debug.Log("Trainer encountered!");
            DialogueManager.Instance.IsTalking = true;
            if (HasFought)
            {
                DialogueManager.Instance.Show(new DialogueModel
                {
                    Name = Data.Name,
                    Text = Data.PostBattleDialogue,
                    Sprite = Data.Image,
                    IsTrainer = true,
                    Action = () => {
                        DialogueManager.Instance.IsTalking = false;
                    }
                });
            }
            else
            {
                DialogueManager.Instance.Show(new DialogueModel
                {
                    Name = Data.Name,
                    Text = Data.PreBattleDialogue,
                    Sprite = Data.Image,
                    IsTrainer = true,
                    Action = () => {
                        DialogueManager.Instance.IsTalking = false;
                        BattleManager.Instance.Show(new BattleEnemyModel
                        {
                            Name = Data.Name,
                            Party = Data.Party
                        });
                    }
                });
            }
        }
    }
}
