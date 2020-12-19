using System;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class SoundSource : MonoBehaviour, ISoundSource
    {
        public void PlayOnce(AudioSource source, AudioClip clip)
        {
            source.PlayOneShot(clip);
        }
    }
}
