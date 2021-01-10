using System;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class SoundSource : MonoBehaviour, ISoundSource
    {
        public void PlayLoop(AudioSource source, AudioClip clip)
        {
            source.clip = clip;
            source.loop = true;
            source.Play();
        }

        public void PlayOnce(AudioSource source, AudioClip clip)
        {
            source.PlayOneShot(clip);
        }
    }
}
