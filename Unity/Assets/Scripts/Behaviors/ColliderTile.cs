using UnityEngine;

namespace LD55.Behaviors
{
    public abstract class ColliderTileBase : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("Collision entered!");
            if (other.gameObject.CompareTag("Player"))
            {
                Action();
            }
        }
        
        protected abstract void Action();
    }
}
