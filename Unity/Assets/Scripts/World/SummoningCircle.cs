using LD55.Behaviors;
using UnityEngine;

namespace LD55.World
{
    public class SummoningCircle : TriggerTileBase
    {
        protected override void Action()
        {
            Debug.Log("Interaction occurred!");
        }
    }
}
