using UnityEngine;

namespace Assets.Scripts
{
    public interface ISoundSource
    {
        void PlayOnce(AudioSource source, AudioClip clip);
    }
}
