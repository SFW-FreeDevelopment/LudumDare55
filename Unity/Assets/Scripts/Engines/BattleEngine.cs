using LD55.Enums;
using LD55.Models;
using LD55.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LD55
{
    public interface IBattleEngine
    {
        HitResult TryAttack(MonsterInstance attackerInstance, MonsterInstance defenderIntance, BattleMove battleMove);
        ItemResult UseItem(MonsterInstance attackerInstance, MonsterInstance defenderIntance, string item);
        FleeResult TryFlee(MonsterInstance attackerInstance, MonsterInstance defenderIntance, int attempt = 0);


    }

    public class HitResult
    {
        public bool HitSuccess { get; set; }
        public decimal Damage { get; set; }
        public Effectiveness Effectiveness { get; set; }
        public string Message { get; set; }
        public BattleMove moveUsed { get; set; }
    }

    public class CaptureResult
    {
        public bool CaptureSuccess { get; set; }
        public int Roll { get; set; }
        public int CaptureCheck { get; set; }
        public string ShardOrStone { get; set; }
        public string Message { get; set; }
    }

    public class ItemResult
    {
        public bool ItemSuccess { get; set; }
        public CaptureResult CaptureResult { get; set; }
        public HitResult HitResult { get; set; }
    }

    public class FleeResult
    {
        public bool FleeSuccess { get; set; }
        public string FleeMessage { get; set; }
        public int FleeAttempts { get; set; }
    }

    public enum Effectiveness
    {
        Normal,
        Effective,
        Ineffective
    }


    public class BattleEngine : IBattleEngine
    {
        public FleeResult TryFlee(MonsterInstance attackerInstance, MonsterInstance defenderInstance, int attempt)
        {
            FleeResult result = new FleeResult();
            var fleeSuccess = CheckFleeSuccess(attackerInstance, defenderInstance, attempt);

            if (fleeSuccess)
            {
                result.FleeSuccess = true;
                result.FleeMessage = $"{attackerInstance.Monster.Name} has successfully fled";
            }
            else
            {
                result.FleeSuccess = false;
                result.FleeMessage = $"{attackerInstance.Monster.Name} was too slow to run away!";
            }

            return result;
        }


        /// <summary>
        /// Checks if flee succedded.
        /// </summary>
        /// <param name="attackerInstance"></param>
        /// <param name="defenderInstance"></param>
        /// <returns></returns>
        private bool CheckFleeSuccess(MonsterInstance playerInstance, MonsterInstance monsterInstance, int attempt = 0)
        {
            const int baseEscapeChance = 40; //Assuming full hp, equal speed, and same level, this is the base chance for escape as a percent.
            const int additionalChancePerAttempt = 5; //Each attempt adds this much % chance to escape.

            //variables used
            System.Random rand = new System.Random();
            var defendingHpRatioRemaining = (monsterInstance.CurrentHealth / monsterInstance.MaxHealth);
            int playerSpeedScore = 2 * playerInstance.Monster.Speed;
            int monsterSpeedScore =  monsterInstance.Monster.Speed + (monsterInstance.Monster.Speed * defendingHpRatioRemaining);
            int speedRatio = playerSpeedScore / monsterSpeedScore;
            var levelFactor = (playerInstance.Level - monsterInstance.Level);

            //This is the roll that will be used against the check to see if the flee succeedd
            var playerRoll = (speedRatio * baseEscapeChance) + levelFactor + ((attempt - 1) * additionalChancePerAttempt);

            //Generate a random number between 0 and 256
            var checkToBeat = rand.Next(0, 100);

            if (playerRoll > checkToBeat)
            {
                return true;
            }

            return false;
        }

        public ItemResult UseItem(MonsterInstance playerInstance, MonsterInstance monsterInstance, string item)
        {
            ItemResult result = new ItemResult();

            switch (item)
            {
                case "shard":
                case "stone":
                    result.CaptureResult = TryCapture(monsterInstance, item);

                    if (result.CaptureResult.CaptureSuccess == true)
                    {
                        result.ItemSuccess = true;
                    }
                    else
                    {
                        result.ItemSuccess = false;
                    }
                    break;

                case "hexBag":
                    BattleMove move = new BattleMove();
                    var hexBag = move.CreateBattleMoveFromItem("name", "description", Enums.BattleMoveCategory.Attack, Enums.MonsterType.Fire, 10, 1);
                    result.HitResult = TryAttack(playerInstance, monsterInstance, hexBag);

                    if (result.HitResult.HitSuccess == true)
                    {
                        result.ItemSuccess = true;
                    }
                    else
                    {
                        result.ItemSuccess = false;
                    }
                    break;
                case "potion":
                    result.HitResult = TryHeal(playerInstance, item);
                    playerInstance.Heal((int)result.HitResult.Damage);
                    result.ItemSuccess = true;
                    break;
            }

            ConsumeItem();

            return result;
        }

        private HitResult TryHeal(MonsterInstance attackerInstance, string item)
        {
            attackerInstance.Heal(10);//item.Damage);
            var hitResult = new HitResult();
            hitResult.HitSuccess = true;
            hitResult.Damage = 10;//item.Damage;
            hitResult.Message = $"{attackerInstance.Monster.Name} used {item} to restore {hitResult.Damage} health";

            return hitResult;
        }

        private CaptureResult TryCapture(MonsterInstance monsterInstance, string shardOrStone)
        {
            CaptureResult result = DoesCapture(monsterInstance, shardOrStone);
            if (result.CaptureSuccess == true)
            {
                //Do Capture;
            }

            return result;
        }

        /// <summary>
        /// Determine if the demon is captured
        ///  n  > ((CaptureRate * HpModifier * shardModifier) + levelModifier))
        /// </summary>
        /// <param name="monsterInstance"></param>
        /// <param name="shardModifer"></param>
        /// <returns></returns>
        private CaptureResult DoesCapture(MonsterInstance monsterInstance, string shardOrStone)
        {
            CaptureResult captureResult = new CaptureResult();
            captureResult.ShardOrStone = shardOrStone;
            const decimal shard = 1.1m;
            const decimal stone = .9m;
            const int shardMaxN = 75;
            const int stoneMaxN = 90;

            var monsterCurrentHp = monsterInstance.CurrentHealth;
            var monsterMaxHp = monsterInstance.MaxHealth;
            var monsterLevel = monsterInstance.Level;
            var captureRate = 30;//monsterInstance.CaptureRate;
            var maxN = 75;

            decimal itemModifier = 1m;
            itemModifier = shard;  //Logic to get shard modifier goes here
            maxN = shardMaxN; //apply shard modifier

            decimal hpModifier = (3 * monsterCurrentHp) / (2 * monsterMaxHp);

            System.Random rand = new System.Random();
            var n = rand.Next(1, maxN);
            captureResult.Roll = n;

            var captureDifficulty = captureRate * hpModifier * itemModifier;

            if (shardOrStone == "shard")
            {
                captureDifficulty += monsterLevel;
            }
            captureResult.CaptureCheck = (int)captureDifficulty;

            if (n > captureDifficulty)
            {
                captureResult.CaptureSuccess = true;
                captureResult.Message = $"{monsterInstance.Id} was captured";
                return captureResult;
            }

            captureResult.CaptureSuccess = false;
            captureResult.Message = $"{monsterInstance.Id} resisted capture!";
            return captureResult;
        }



        /// <summary>
        /// Generate random number between 1 and 100. If random number is less than the move accuracy, the move hits (true). Else the move missed (false).
        /// </summary>
        /// <param name="moveAccuracy"></param>
        /// <returns></returns>
        private bool DoesMoveHit(int moveAccuracy)
        {
            System.Random rand = new System.Random();
            var n = rand.Next(0, 100);

            if (n < moveAccuracy)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// (((((2 * Level)/5)+2)*Power*(Attack/Defence)/50)+2)*Modifier
        /// </summary>
        /// <returns></returns>
        private decimal CalculateDamage(int level, int power, int attack, int defence, decimal modifier)
        {
            var random = new System.Random();
            var levelModifier = ((2 * level)/5) + random.Next(1, 3);
            var attackDefenceRatio = attack/defence;
            var calculateNumerator = levelModifier * power * attackDefenceRatio;
            var resultBeforeModifier = (calculateNumerator / 50) + 2;
            var result = resultBeforeModifier * modifier;

            return result;
        }

        /// <summary>
        /// Gets the modifier to use for damage. 1.3 for super, 1 for regular, .85 for not effective
        /// </summary>
        /// <returns></returns>
        private Effectiveness GetModifier(MonsterType defenderType, MonsterType moveType)
        {
            Dictionary<(MonsterType, MonsterType), Effectiveness> effectivenessMap = new Dictionary<(MonsterType, MonsterType), Effectiveness>
    {
        // Fire type effectiveness
        {(MonsterType.Fire, MonsterType.Fire), Effectiveness.Ineffective},
        {(MonsterType.Fire, MonsterType.Rock), Effectiveness.Ineffective},
        {(MonsterType.Fire, MonsterType.Dark), Effectiveness.Effective},
        
        // Dark type effectiveness
        {(MonsterType.Dark, MonsterType.Dark), Effectiveness.Ineffective},
        {(MonsterType.Dark, MonsterType.Fire), Effectiveness.Ineffective},
        {(MonsterType.Dark, MonsterType.Fel), Effectiveness.Effective},
        
        // Rock type effectiveness
        {(MonsterType.Rock, MonsterType.Rock), Effectiveness.Ineffective},
        {(MonsterType.Rock, MonsterType.Fire), Effectiveness.Ineffective},
        {(MonsterType.Rock, MonsterType.Poison), Effectiveness.Effective},
        
        // Poison type effectiveness
        {(MonsterType.Poison, MonsterType.Poison), Effectiveness.Ineffective},
        {(MonsterType.Poison, MonsterType.Rock), Effectiveness.Ineffective},
        {(MonsterType.Poison, MonsterType.Fel), Effectiveness.Effective},
        
        // Fel type effectiveness
        {(MonsterType.Fel, MonsterType.Fel), Effectiveness.Ineffective},
        {(MonsterType.Fel, MonsterType.Dark), Effectiveness.Ineffective},
        {(MonsterType.Fel, MonsterType.Fire), Effectiveness.Ineffective}
    };

            if (effectivenessMap.TryGetValue((moveType, defenderType), out Effectiveness effectiveness))
            {
                return effectiveness;
            }

            return Effectiveness.Normal;
        }

        public HitResult TryAttack(MonsterInstance attackerInstance, MonsterInstance defenderInstance, BattleMove battleMove)
        {
            var hitResult = new HitResult();
            var accuracyCheck = DoesMoveHit(battleMove.Accuracy);

            if (accuracyCheck == false) //Move Missed
            {
                hitResult.HitSuccess = false;
                hitResult.Damage = 0;
                hitResult.Message = GenerateMessage(battleMove, hitResult, attackerInstance, defenderInstance);
                return hitResult;
            }

            //Move Hit
            hitResult.HitSuccess = true;
            var modifier = GetModifier(defenderInstance.Monster.Type, battleMove.MoveType);
            var modifierValue = 1m;
            switch (modifier)
            {
                case Effectiveness.Effective:
                    hitResult.Effectiveness = Effectiveness.Effective;
                    modifierValue = 1.3m;
                    break;
                case Effectiveness.Normal:
                    hitResult.Effectiveness = Effectiveness.Normal;
                    modifierValue = 1m;
                    break;
                case Effectiveness.Ineffective:
                    hitResult.Effectiveness = Effectiveness.Ineffective;
                    modifierValue = .85m;
                    break;
            }

            var playerIntanceAttack = 15; //need to add to monsterinstance
            var monsterIntanceDefence = 10; //need to add to monsterinstance

            if (battleMove.Category == Enums.BattleMoveCategory.Attack)
            {
                var damage = CalculateDamage(attackerInstance.Level, battleMove.Damage, playerIntanceAttack, monsterIntanceDefence, modifierValue);
                defenderInstance.TakeDamage((int)damage);
                hitResult.Damage = (int)damage;
                hitResult.moveUsed = battleMove;
                hitResult.Message = GenerateMessage(battleMove, hitResult, attackerInstance, defenderInstance);
                return hitResult;
            }

            if (battleMove.Category == Enums.BattleMoveCategory.Status)
            {
                ProcessStatus(battleMove, attackerInstance, defenderInstance);
                hitResult.Damage = battleMove.Damage;
                hitResult.moveUsed = battleMove;
                hitResult.Message = GenerateMessage(battleMove, hitResult, attackerInstance, defenderInstance);
                return hitResult;
            }

            //Should not reach here
            hitResult.HitSuccess = false;
            hitResult.moveUsed = battleMove;
            hitResult.Damage = 0;
            hitResult.Message = $"Something went wrong, your move missed. {battleMove.Name} was a terrible attack";
            return hitResult;
        }

        private bool ProcessStatus(BattleMove battleMove, MonsterInstance attackerInstance, MonsterInstance defenderInstance)
        {
            switch (battleMove.Name)
            {
                case "Attack Up":
                    //attackerInstance.AttakModifier += battleMove.Value;
                    break;
                case "Attack Down":
                    //defenderInstance.AttakModifier -= battleMove.Value;
                    break;
                case "Defence Up":
                    //attackerInstance.DefenceModifier += battleMove.Value;
                    break;
                case "Defence Down":
                    //defenderInstance.AttakModifier -= battleMove.Value;
                    break;
            }
            return true;
        }


        private string GenerateMessage(BattleMove battleMove, HitResult hitResult, MonsterInstance attackerInstance, MonsterInstance defenderInstance = null)
        {
            string result = string.Empty;
            if (hitResult.HitSuccess == false)
            {
                result = $"{attackerInstance.Monster.Name} used {battleMove.Name}... it missed! ";
            }

            if (battleMove.Category == Enums.BattleMoveCategory.Attack && hitResult.HitSuccess == true)
            {
                result = $"{attackerInstance.Monster.Name} used {battleMove.Name} on {defenderInstance.Monster.Name}.";
                if (hitResult.Effectiveness != Effectiveness.Normal)
                {
                    result += $"{Environment.NewLine}It was {hitResult.Effectiveness.ToString()}.";
                }

                result += $"{Environment.NewLine}It took {hitResult.Damage} damage!";
            }

            if (battleMove.Category == Enums.BattleMoveCategory.Status && hitResult.HitSuccess == true)
            {
                result = $"{attackerInstance.Monster.Name} used {battleMove.Name} on {defenderInstance.Monster.Name}.{Environment.NewLine}It took {hitResult.Damage} damage!";
            }

            if (battleMove.Category == Enums.BattleMoveCategory.Restore && hitResult.HitSuccess == true)
            {
                result = $"{attackerInstance.Monster.Name} healed for {hitResult.Damage}";
            }

            return result;
        }

        /// <summary>
        /// Player consumed an item. Remove it from their inventory.
        /// </summary>
        /// <returns></returns>
        private bool ConsumeItem()//(MonsterInstance playerInstance, string item)
        {
            //consume Item
            return true;
        }

    }

}
