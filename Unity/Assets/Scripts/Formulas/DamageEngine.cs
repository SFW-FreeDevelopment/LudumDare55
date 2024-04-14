using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LD55
{
    public class DamageEngine
    {
        /// <summary>
        /// Generate random number between 1 and 100. If random number is less than the move accuracy, the move hits (true). Else the move missed (false).
        /// </summary>
        /// <param name="moveAccuracy"></param>
        /// <returns></returns>
        public bool DoesMoveHit(int moveAccuracy)
        {
            System.Random rand = new System.Random();
            var n = rand.Next(0,100);

            if( n < moveAccuracy) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// (((((2 * Level)/5)+2)*Power*(Attack/Defence)/50)+2)*Modifier
        /// </summary>
        /// <returns></returns>
        public decimal CalculateDamage(int level, int power, int attack, int defence, decimal modifier)
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
        public decimal GetModifier(string type1, string type2)
        {
            const decimal effective = 1.3m;
            const decimal normal = 1.3m;
            const decimal notEffective = .85m;

            //logic for effectiveness

            return normal;
        }
    }
}
