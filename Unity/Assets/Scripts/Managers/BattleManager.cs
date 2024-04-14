using System.Collections;
using System.Linq;
using LD55.Models;
using LD55.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD55.Managers
{
    public class BattleManager : SceneSingleton<BattleManager>
    {
        public bool IsBattling { get; set; }
        
        [SerializeField] private GameObject _canvas, _mainMenu, _fightMenu, _partyMenu, _itemsMenu;

        [SerializeField] private GameObject _hitResultPane;
        [SerializeField] private TextMeshProUGUI _hitResultText;
        
        public BattleState State { get; set; } = new BattleState();

        private Coroutine CurrentBattleRoutine { get; set; }

        private BattleEngine BattleEngine { get; set; } = new BattleEngine();

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
                yield return null;
                
                if (!IsBattling || !State.WaitingForPlayerInput || State.InputLocked) continue;

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
                        var result = BattleEngine.TryAttack(State.CurrentMonster, State.Enemy.CurrentMonster, State.CurrentMonster.Monster.LearnableMoves[0].Move);
                        RefreshUI();
                        DisplayHitResult(result);
                        CheckForBattleEnd();
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                    {
                        var result = BattleEngine.TryAttack(State.CurrentMonster, State.Enemy.CurrentMonster, State.CurrentMonster.Monster.LearnableMoves[1].Move);
                        RefreshUI();
                        DisplayHitResult(result);
                        CheckForBattleEnd();
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                    {
                        var result = BattleEngine.TryAttack(State.CurrentMonster, State.Enemy.CurrentMonster, State.CurrentMonster.Monster.LearnableMoves[2].Move);
                        RefreshUI();
                        DisplayHitResult(result);
                        CheckForBattleEnd();
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                    {
                        var result = BattleEngine.TryAttack(State.CurrentMonster, State.Enemy.CurrentMonster, State.CurrentMonster.Monster.LearnableMoves[3].Move);
                        RefreshUI();
                        DisplayHitResult(result);
                        CheckForBattleEnd();
                    }
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
                Enemy = model,
                PlayerParty = PlayerDataManager.Instance.Party
            };
            
            SetupEnemyUI(State.Enemy.CurrentMonster);
            SetupPlayerUI(State.CurrentMonster);
            
            _canvas.SetActive(true);
            CurrentBattleRoutine = StartCoroutine(Routine());
        }

        private void DisplayHitResult(HitResult hitResult)
        {
            _mainMenu.SetActive(false);
            _fightMenu.SetActive(false);
            _partyMenu.SetActive(false);
            _itemsMenu.SetActive(false);
            
            _hitResultText.text = hitResult.Message;
            _hitResultPane.gameObject.SetActive(true);
            State.InputLocked = true;
            StartCoroutine(CoroutineTemplate.DelayAndFireRoutine(2.5f, () => {
                _hitResultPane.gameObject.SetActive(false);
                _mainMenu.SetActive(true);
                State.CurrentMenu = null;
                State.InputLocked = false;
            }));
        }
        
        private void RefreshUI()
        {
            SetupEnemyUI(State.Enemy.CurrentMonster);
            SetupPlayerUI(State.CurrentMonster);
        }

        private void CheckForBattleEnd()
        {
            var enemyWiped = State.Enemy.Party.Monsters.All(x => x.CurrentHealth == 0);
            var playerWiped = State.PlayerParty.Monsters.All(x => x.CurrentHealth == 0);
            if (enemyWiped || playerWiped)
            {
                if (playerWiped)
                {
                    Debug.Log("Oh nouuur");
                    // TODO: Handle end battle
                }
                else
                {
                    // TODO: Handle end battle
                }
            }
        }
        
        private void SetupEnemyUI(MonsterInstance monster)
        {
            _enemyName.text = monster.Monster.Name;
            _enemyLevelText.text = $"Level {monster.Level}";
            _enemyTypeText.text = $"{monster.Monster.Type} Type";
            _enemyHealthSlider.maxValue = monster.MaxHealth;
            _enemyHealthSlider.value = monster.CurrentHealth;
            _enemyImage.sprite = monster.Monster.Image;
        }

        private void SetupPlayerUI(MonsterInstance monster)
        {
            _playerName.text = monster.Monster.Name;
            _playerLevelText.text = $"Level {monster.Level}";
            _playerHealthSlider.maxValue = monster.MaxHealth;
            _playerHealthSlider.value = monster.CurrentHealth;
            _playerImage.sprite = monster.Monster.Image;
        }
        
        public void Hide()
        {
            StopCoroutine(CurrentBattleRoutine);
            CurrentBattleRoutine = null;
            IsBattling = false;
            _fightMenu.SetActive(false);
            _partyMenu.SetActive(false);
            _itemsMenu.SetActive(false);
            _mainMenu.SetActive(true);
            _canvas.SetActive(false);
        }

        public void SelectFight()
        {
            State.CurrentMenu = SubMenu.Fight;
            _fightMenu.SetActive(true);
            _mainMenu.SetActive(false);
        }

        public void SelectParty()
        {
            State.CurrentMenu = SubMenu.Party;
            _partyMenu.SetActive(true);
            _mainMenu.SetActive(false);
        }

        public void SelectItems()
        {
            State.CurrentMenu = SubMenu.Items;
            _itemsMenu.SetActive(true);
            _mainMenu.SetActive(false);
        }

        public void SelectBack()
        {
            State.CurrentMenu = null;
            _fightMenu.SetActive(false);
            _partyMenu.SetActive(false);
            _itemsMenu.SetActive(false);
            _mainMenu.SetActive(true);
        }

        public void SelectRun()
        {
            State.CurrentMenu = null;
            Hide();
        }
    }
}
