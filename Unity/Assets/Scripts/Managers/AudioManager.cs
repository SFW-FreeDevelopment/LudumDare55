﻿using System.Collections.Generic;
using System.Linq;
using LD55.Behaviors;
using LD55.Enums;
using UnityEngine;

namespace LD55.Managers
{
    public class AudioManager : GameSingleton<AudioManager>
    {
        [SerializeField] private GameObject _audioObjectPrefab;

        private readonly IDictionary<string, AudioClip> _audioClipDictionary = new Dictionary<string, AudioClip>();
        
        protected override void InitSingletonInstance()
        {
            var audioClips = Resources.LoadAll<AudioClip>("Audio");
            if (audioClips?.Any() ?? false)
            {
                foreach (var audioClip in audioClips)
                {
                    if (!_audioClipDictionary.TryGetValue(audioClip.name, out _))
                    {
                        _audioClipDictionary.Add(audioClip.name.ToLower(), audioClip);
                    }
                }
            }
        }

        public void Play(SoundName soundName) => Play(soundName.ToString());

        public void Play(string clipName, bool loop = false)
        {
            if (_audioClipDictionary.TryGetValue(clipName.ToLower(), out var clip))
            {
                var spawnedObj = Instantiate(_audioObjectPrefab);
                spawnedObj.name = $"Audio Object ({clipName})";
                var audioSource = spawnedObj.GetComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.loop = loop;
                if (loop)
                {
                    Destroy(spawnedObj.GetComponent<DestroyAfterTime>());
                }
                try
                {
                    audioSource.volume = SettingsManager.Instance.Settings.SfxVolume;
                }
                catch
                {
                    // do nothing
                }
                audioSource.Play();
            }
        }
    }
}
