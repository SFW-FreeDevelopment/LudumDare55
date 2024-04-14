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
        
        public BattleEnemyModel BattleEnemyModel { get; set; }

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

        public void Show(BattleEnemyModel model)
        {
            IsBattling = true;
            State = new BattleState
            {
                Enemy = model
            };
            
            SetupEnemyUI(State.Enemy.Party.Monsters[0]);
            SetupPlayerUI(State.Enemy.Party.Monsters[1]);
            
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
            // TODO: Open move select UI
        }

        public void SelectParty()
        {
            // TODO: Open party select UI
        }

        public void SelectItems()
        {
            // TODO: Open item select UI
        }

        public void SelectRun()
        {
            Hide();
        }
    }
}
