using LD55.Enums;
using LD55.Managers;
using UnityEngine;

namespace LD55.Behaviors
{
    public class PlaySoundOnStart : MonoBehaviour
    {
        [SerializeField] private SoundName _soundName;
        [SerializeField] private bool _loop;

        private void Start() => StartCoroutine(CoroutineTemplate.DelayAndFireRoutine(0.1f, () => AudioManager.Instance.Play(_soundName)));
    }
}
