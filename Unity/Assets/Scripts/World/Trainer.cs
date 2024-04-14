using System;
using LD55.Behaviors;
using LD55.Managers;
using LD55.Models;
using UnityEngine;

namespace LD55.World
{
    public class Trainer : TriggerTileBase
    {
        [SerializeField] private ScriptableObjects.Trainer _data;
        public ScriptableObjects.Trainer Data => _data;

        [SerializeField] private bool _hasFought;
        public bool HasFought => _hasFought;
        
        private void Awake()
        {
            GetComponent<SpriteRenderer>().sprite = Data.Sprite;
        }

        protected override void Action()
        {
            Debug.Log("Trainer encountered!");
            if (HasFought)
            {
                DialogueManager.Instance.Show(new DialogueModel
                {
                    Name = Data.Name,
                    Text = Data.PostBattleDialogue,
                    Sprite = Data.Image,
                    IsTrainer = true,
                    Action = () => {
                        // Do nothing since we fought them before
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
