using System.Collections;
using LD55.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD55.Managers
{
    public class BattleManager : SceneSingleton<BattleManager>
    {
        public bool IsBattling { get; set; }
        
        [SerializeField] private GameObject _canvas;

        public BattleState State { get; set; } = new BattleState();

        private Coroutine CurrentBattleRoutine { get; set; }

        [Header("Enemy")]
        [SerializeField] private Image _enemyImage;
        [SerializeField] private TextMeshProUGUI _enemyName, _enemyLevelText, _enemyTypeText;
        [SerializeField] private Slider _enemyHealthSlider;
        
        [Header("Player")]
        [SerializeField] private Image _playerImage;
        [SerializeField] private TextMeshProUGUI _playerName, _playerLevelText;
        [SerializeField] private Slider _playerHealthSlider;
        
        protected override void InitSingletonInstance()
        {
        }

        private IEnumerator Routine()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                if (State.WaitingForPlayerInput) continue;
                
                // TODO: Battle things
            }
        }

        private void Update()
        {
            if (!IsBattling || !State.WaitingForPlayerInput) return;

            if (State.CurrentMenu == null)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                {
                    SelectFight();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                {
                    SelectParty();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                {
                    SelectItems();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                {
                    SelectRun();
                }
            }
            else if (State.CurrentMenu == SubMenu.Fight)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SelectBack();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                {
                    // TODO: Lock in move
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                {
                    // TODO: Lock in move
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                {
                    // TODO: Lock in move
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                {
                    // TODO: Lock in move
                }
            }
        }

        public void SelectMove()
        {
            
        }

        public void Show(BattleEnemyModel model)
        {
            IsBattling = true;
            State = new BattleState
            {
                Enemy = model
            };
            
            SetupEnemyUI(State.Enemy.Party.Monsters[0]);
            SetupPlayerUI(State.Enemy.Party.Monsters[0]);
            
            _canvas.SetActive(true);
            CurrentBattleRoutine = StartCoroutine(Routine());
        }

        private void SetupEnemyUI(MonsterInstance monster)
        {
            _enemyName.text = monster.Monster.Name;
            _enemyLevelText.text = $"Level {monster.Level}";
            _enemyTypeText.text = $"{monster.Monster.Type} Type";
            _enemyHealthSlider.maxValue = monster.MaxHealth;
            _enemyHealthSlider.value = monster.MaxHealth;
            _enemyImage.sprite = monster.Monster.Image;
        }

        private void SetupPlayerUI(MonsterInstance monster)
        {
            _playerName.text = monster.Monster.Name;
            _playerLevelText.text = $"Level {monster.Level}";
            _playerHealthSlider.maxValue = monster.MaxHealth;
            _playerHealthSlider.value = monster.MaxHealth;
            _playerImage.sprite = monster.Monster.Image;
        }
        
        public void Hide()
        {
            StopCoroutine(CurrentBattleRoutine);
            CurrentBattleRoutine = null;
            IsBattling = false;
            _canvas.SetActive(false);
        }

        public void SelectFight()
        {
            State.CurrentMenu = SubMenu.Fight;
            // TODO: Open move select UI
        }

        public void SelectParty()
        {
            State.CurrentMenu = SubMenu.Party;
            // TODO: Open party select UI
        }

        public void SelectItems()
        {
            State.CurrentMenu = SubMenu.Items;
            // TODO: Open item select UI
        }

        public void SelectBack()
        {
            State.CurrentMenu = null;
            // TODO: Close all menus
        }

        public void SelectRun()
        {
            State.CurrentMenu = null;
            Hide();
        }
    }
}
