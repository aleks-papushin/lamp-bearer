using UnityEngine;

public class WallSound : MonoBehaviour
{
    [SerializeField] private AudioClip _dangerSound;

    private readonly float _defaultVolume = 0.2f;
    private AudioSource _source;

    private void Start()
    {
        _source = gameObject.AddComponent<AudioSource>();
    }

    public void PlayDangerSound()
    {
        _source.PlayOneShot(_dangerSound, _defaultVolume);
    }
}
