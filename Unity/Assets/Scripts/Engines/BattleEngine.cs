using LD55.Models;
using LD55.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD55
{

    public class HitResult
    {
        public bool HitSuccess { get; set; }
        public decimal Damage { get; set; }
        public string Effectiveness { get; set; }
    }

    public interface IBattleEngine
    {
        bool TryCapture(MonsterInstance monsterInstance, string shardOrStone);
        HitResult TryAttack(MonsterInstance playerInstance, MonsterInstance monsterInstance, BattleMove battleMove)
    }

    public class BattleEngine : IBattleEngine
    {
        public bool TryCapture(MonsterInstance monsterInstance, string shardOrStone)
        {
            return DoesCapture(monsterInstance, shardOrStone);
        }


        /// <summary>
        /// Determine if the demon is captured
        ///  n  > ((CaptureRate * HpModifier * shardModifier) + levelModifier))
        /// </summary>
        /// <param name="monsterInstance"></param>
        /// <param name="shardModifer"></param>
        /// <returns></returns>
        private bool DoesCapture(MonsterInstance monsterInstance, string shardOrStone)
        {
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

            var captureDifficulty = captureRate * hpModifier * itemModifier;

            if (shardOrStone == "shard")
            {
                captureDifficulty += monsterLevel;
            }

            if (n > captureDifficulty)
            {
                return true;
            }

            return false;
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
            var levelModifier = ((2 * level)/5)+2;
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
        private decimal GetModifier(string type1, string type2)
        {
            const decimal effective = 1.3m;
            const decimal normal = 1.0m;
            const decimal notEffective = .85m;

            //logic for effectiveness

            return normal;
        }

        public HitResult TryAttack(MonsterInstance playerInstance, MonsterInstance monsterInstance, BattleMove battleMove)
        {
            var hitResult = new HitResult();
            var accuracyCheck = DoesMoveHit(battleMove.Accuracy);
            
            if(accuracyCheck == false) //Move Missed
            {
                hitResult.HitSuccess = false;
                hitResult.Damage = 0;
                return hitResult;
            }

            //Move Hit
            hitResult.HitSuccess = true;
            string monsterIntanceType = ""; //monsterIntance.Type
            var modifier = GetModifier(monsterIntanceType, battleMove.MoveType.ToString()); 
            
            switch(modifier)
            {
                case 1.3m:
                    hitResult.Effectiveness = "Effective";
                    break;
                case 1.0m:
                    hitResult.Effectiveness = "Normal";
                    break;
                case .85m:
                    hitResult.Effectiveness = "Not Effective";
                    break;
            }

            var playerIntanceAttack = 15;
            var monsterIntanceDefence = 10;
            var damage = CalculateDamage(playerInstance.Level, battleMove.Damage, playerIntanceAttack, monsterIntanceDefence, modifier);
            hitResult.Damage = damage;
            
            return hitResult;
        }


    }
    
}
