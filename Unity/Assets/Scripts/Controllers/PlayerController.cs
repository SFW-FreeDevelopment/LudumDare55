using LD55.Managers;
using LD55.Models;
using LD55.ScriptableObjects;
using UnityEngine;

namespace LD55.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public Transform movePoint;
        private int count = 0;

        public LayerMask whatStopsMovement;
        public LayerMask encounterZone;

        public SpriteRenderer SpriteRenderer;

        void Start()
        {
            // Remove movePoint from parent so that movePoint position is not relative to the player
            movePoint.parent = null;
        }

        void Update()
        {
            if (BattleManager.Instance?.IsBattling ?? false) return;
            if (DialogueManager.Instance?.IsTalking ?? false) return;

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SpriteRenderer.flipX = true;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                SpriteRenderer.flipX = false;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, whatStopsMovement))
                    {
                        movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                        CheckForEncounters(); // Check for encounters right after updating movePoint
                    }
                }
                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, whatStopsMovement))
                    {
                        movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                        CheckForEncounters(); // Check for encounters right after updating movePoint
                    }
                }
            }
        }

        private void CheckForEncounters()
        {
            if (Physics2D.OverlapCircle(movePoint.position, 0.2f, encounterZone) != null)
            {
                if (Random.Range(1, 101) <= 101)
                {
                    Debug.Log($"Encountered pokemon {count}");
                    count++;
                    var monsters = Resources.LoadAll<Monster>("Monsters");
                    var idx = Random.Range(0, monsters.Length);
                    var monster = monsters[idx];
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
    }
}
