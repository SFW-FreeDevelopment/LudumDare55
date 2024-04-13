using UnityEngine;

namespace LD55.Behaviors
{
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] private float _time = 3;
        
        private void Start() => Destroy(gameObject, _time);
    }
}
