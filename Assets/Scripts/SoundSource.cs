using UnityEngine;

namespace Assets.Scripts
{
    public abstract class SoundSource : MonoBehaviour
    {
        protected static void PlayLoop(AudioSource source, AudioClip clip)
        {
            source.clip = clip;
            source.loop = true;
            source.Play();
        }

        protected static void PlayOnce(AudioSource source, AudioClip clip)
        {
            source.PlayOneShot(clip);
        }
    }
}
