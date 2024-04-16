using System;
using System.Collections;
using System.Linq;
using LD55.Enums;
using LD55.Models;
using LD55.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using Random=System.Random;

namespace LD55.Managers
{
    public class BattleManager : SceneSingleton<BattleManager>
    {
        public bool IsBattling { get; set; }
        private int runAttemptCount = 0;    
        
        [SerializeField] private GameObject _canvas, _mainMenu, _fightMenu, _partyMenu, _itemsMenu;

        [SerializeField] private GameObject _hitResultPane;
        [SerializeField] private TextMeshProUGUI _hitResultText;
        
        [SerializeField] private GameObject _battleResultPane;
        [SerializeField] private TextMeshProUGUI _battleResultText;

        [SerializeField] private AudioSource _musicSource;
        
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
                
                if (!IsBattling || State.InputLocked || State.AnimationInProgress) continue;

                // AI
                if (!State.WaitingForPlayerInput)
                {
                    State.InputLocked = true;
                    var moves = State.Enemy.CurrentMonster.Monster.LearnableMoves;
                    var idx = UnityEngine.Random.Range(0, moves.Length);
                    var move = moves[idx].Move;
                    ProcessEnemyMove(move);
                    State.InputLocked = false;
                }
                else
                {
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
                            SelectMove(0);
                        }
                        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                        {
                            SelectMove(1);
                        }
                        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                        {
                            SelectMove(2);
                        }
                        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                        {
                            SelectMove(3);
                        }
                    }
                    else if (State.CurrentMenu == SubMenu.Items)
                    {
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            SelectBack();
                        }
                        else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                        {
                            ProcessPlayerItem("potion");
                        }
                        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                        {
                            ProcessPlayerItem("shard");
                        }
                        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                        {
                            ProcessPlayerItem("stone");
                        }
                        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                        {
                            ProcessPlayerItem("hexBag");
                        }
                    }
                }
            }
        }

        public void SelectMove(int index)
        {
            ProcessPlayerMove(State.CurrentMonster.Monster.LearnableMoves[index].Move);
        }

        private void ProcessPlayerMove(BattleMove move)
        {
            PlayAttackSound(move.Category);
            var result = BattleEngine.TryAttack(State.CurrentMonster, State.Enemy.CurrentMonster, move);
            RefreshUI();
            DisplayHitResult(result, false);
        }

        private void ProcessPlayerItem(string item)
        {
            var result = BattleEngine.UseItem(State.CurrentMonster, State.Enemy.CurrentMonster, item);

            if (result.HitResult != null)
            {
                DisplayHitResult(result.HitResult, false);
            }
            else if (result.CaptureResult != null)
            {
                DisplayCaptureResult(result.CaptureResult, false);
            }
        }

        private void ProcessEnemyMove(BattleMove move)
        {
            PlayAttackSound(move.Category);
            var result = BattleEngine.TryAttack(State.Enemy.CurrentMonster, State.CurrentMonster, move);
            RefreshUI();
            DisplayHitResult(result, true);
        }

        private void PlayAttackSound(BattleMoveCategory category)
        {
            switch (category)
            {
                case BattleMoveCategory.Attack:
                    AudioManager.Instance.Play(SoundName.Attack);
                    break;
                case BattleMoveCategory.Status:
                    AudioManager.Instance.Play(SoundName.Growl);
                    break;
                case BattleMoveCategory.Restore:
                    AudioManager.Instance.Play(SoundName.Growl2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), category, null);
            }
        }

        public void Show(BattleEnemyModel model)
        {
            _musicSource.gameObject.SetActive(true);
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

        private void DisplayHitResult(HitResult hitResult, bool waitingForPlayerInput)
        {
            _mainMenu.SetActive(false);
            _fightMenu.SetActive(false);
            _partyMenu.SetActive(false);
            _itemsMenu.SetActive(false);
            
            _hitResultText.text = hitResult.Message;
            _hitResultPane.gameObject.SetActive(true);
            State.InputLocked = true;
            State.AnimationInProgress = true;
            StartCoroutine(CoroutineTemplate.DelayAndFireRoutine(2.0f, () => {
                _hitResultPane.gameObject.SetActive(false);
                _mainMenu.SetActive(true);
                State.CurrentMenu = null;
                State.InputLocked = false;
                CheckForBattleEnd();
                State.AnimationInProgress = false;
                State.WaitingForPlayerInput = waitingForPlayerInput;
            }));
        }

        private void DisplayCaptureResult(CaptureResult captureResult, bool waitingForPlayerInput)
        {
            _mainMenu.SetActive(false);
            _fightMenu.SetActive(false);
            _partyMenu.SetActive(false);
            _itemsMenu.SetActive(false);

            _hitResultText.text = captureResult.Message;
            _hitResultPane.gameObject.SetActive(true);
            State.InputLocked = true;
            State.AnimationInProgress = true;
            StartCoroutine(CoroutineTemplate.DelayAndFireRoutine(2.0f, () => {
                _hitResultPane.gameObject.SetActive(false);
                _mainMenu.SetActive(true);
                State.CurrentMenu = null;
                State.InputLocked = false;
                CheckForBattleEnd();
                State.AnimationInProgress = false;
                State.WaitingForPlayerInput = waitingForPlayerInput;
            }));
        }

        private void DisplayFleeResult(FleeResult fleeResult, bool waitingForPlayerInput)
        {
            _mainMenu.SetActive(false);
            _fightMenu.SetActive(false);
            _partyMenu.SetActive(false);
            _itemsMenu.SetActive(false);

            _hitResultText.text = fleeResult.FleeMessage;
            _hitResultPane.gameObject.SetActive(true);
            State.InputLocked = true;
            State.AnimationInProgress = true;
            StartCoroutine(CoroutineTemplate.DelayAndFireRoutine(2.0f, () => {
                _hitResultPane.gameObject.SetActive(false);
                _mainMenu.SetActive(true);
                State.CurrentMenu = null;
                State.InputLocked = false;
                CheckForBattleEnd();
                State.AnimationInProgress = false;
                State.WaitingForPlayerInput = waitingForPlayerInput;
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
            var battleHasEnded = enemyWiped || playerWiped;
            if (!battleHasEnded) return;
            
            AudioManager.Instance.Play(SoundName.Thud);

            IsBattling = false;
            _fightMenu.SetActive(false);
            _partyMenu.SetActive(false);
            _itemsMenu.SetActive(false);
            _mainMenu.SetActive(false);
            _hitResultPane.SetActive(false);
            
            _battleResultText.text = playerWiped
                ? $"{State.Enemy.Name} defeated you..."
                : $"You defeated {State.Enemy.Name}!";
            _battleResultPane.SetActive(true);
            State.AnimationInProgress = true;
            StartCoroutine(CoroutineTemplate.DelayAndFireRoutine(2.0f, () => {
                State.AnimationInProgress = false;
                Hide();
                // TODO: Dialogue manager call
            }));
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
            _musicSource.gameObject.SetActive(false);
            if (CurrentBattleRoutine != null)
            {
                StopCoroutine(CurrentBattleRoutine);
                CurrentBattleRoutine = null;
            }
            IsBattling = false;
            _battleResultPane.SetActive(false);
            _fightMenu.SetActive(false);
            _partyMenu.SetActive(false);
            _itemsMenu.SetActive(false);
            _mainMenu.SetActive(true);
            _canvas.SetActive(false);
        }

        public void SelectFight()
        {
            AudioManager.Instance.Play(SoundName.Click1);
            State.CurrentMenu = SubMenu.Fight;
            _fightMenu.SetActive(true);
            _mainMenu.SetActive(false);
        }

        public void SelectParty()
        {
            AudioManager.Instance.Play(SoundName.Click1);
            State.CurrentMenu = SubMenu.Party;
            _partyMenu.SetActive(true);
            _mainMenu.SetActive(false);
        }

        public void SelectItems()
        {
            AudioManager.Instance.Play(SoundName.Click1);
            State.CurrentMenu = SubMenu.Items;
            _itemsMenu.SetActive(true);
            _mainMenu.SetActive(false);
        }

        public void SelectBack()
        {
            AudioManager.Instance.Play(SoundName.Click1);
            State.CurrentMenu = null;
            _fightMenu.SetActive(false);
            _partyMenu.SetActive(false);
            _itemsMenu.SetActive(false);
            _mainMenu.SetActive(true);
        }

        public void SelectRun()
        {
            AudioManager.Instance.Play(SoundName.Click1);
            var playerMonster = State.CurrentMonster;
            var enemyMonster = State.Enemy.CurrentMonster;
            runAttemptCount++;
            FleeResult result = BattleEngine.TryFlee(playerMonster, enemyMonster, runAttemptCount);
            
            DisplayFleeResult(result, false);
            
            if (result.FleeSuccess)
            {
                StartCoroutine(CoroutineTemplate.DelayAndFireRoutine(2.0f, () =>
                {
                    State.CurrentMenu = null;
                    Hide();
                }));
            }

        }
    }
}
