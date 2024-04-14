using UnityEngine;

namespace LD55.Behaviors
{
    public abstract class TriggerTileBase : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Action();
            }
        }

        protected abstract void Action();
    }
}
