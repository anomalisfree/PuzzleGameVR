using System;
using System.Collections.Generic;
using Main.Scripts.ApplicationCore.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Scripts.ApplicationCore.Controllers
{
    public enum AudioFXType
    {
        Click,
        FinalClick
    }
    
    public class AudioFXController : BaseController
    {
        [SerializeField] private AudioFXView audioFXView;
        [SerializeField] private List<AudioClip> clickSounds;
        [SerializeField] private List<AudioClip> finalClickSounds;

        public void AddAudioFX(Transform point, AudioFXType audioFXType)
        {
            var sounds = audioFXType switch
            {
                AudioFXType.Click => clickSounds,
                AudioFXType.FinalClick => finalClickSounds,
                _ => throw new ArgumentOutOfRangeException(nameof(audioFXType), audioFXType, null)
            };

            var audioFX = Instantiate(audioFXView, point.position, Quaternion.identity);
            audioFX.Init(sounds[Random.Range(0, sounds.Count)]);
        }
    }
}
