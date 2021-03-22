using Player;
using System.Collections;
using UI;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _intro;
    [SerializeField] private AudioClip[] _gameMusic;
    [SerializeField] private AudioClip _deathOfFire;
    [SerializeField] private AudioClip _deathOfEnemy;

    private readonly float _defaultMusicVolume = 0.25f;
    private float _currentVolume;
    private AudioSource _musicSource;
    private AudioSource _soundSource;

    private void Awake()
    {
        if (FindObjectsOfType<MusicPlayer>().Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _musicSource = gameObject.AddComponent<AudioSource>();
        _soundSource = gameObject.AddComponent<AudioSource>();
        _musicSource.volume = _defaultMusicVolume;
        _currentVolume = _musicSource.volume;
        PlayLoop(_musicSource, _intro);
        StartButton.OnGameStarted += StartButton_OnGameStarted;
        PlayerCollisions.OnPlayerDeath += PlayerCollisions_OnPlayerDeath;
    }

    private void StartButton_OnGameStarted()
    {
        var clipNumber = Random.Range(0, _gameMusic.Length);
        StartCoroutine(FadeOutCurrentAndStartNewClip(_gameMusic[clipNumber]));
    }

    private void PlayerCollisions_OnPlayerDeath(bool deathOfFire)
    {
        if (deathOfFire)
        {
            _soundSource.PlayOneShot(_deathOfFire);
        }
        else
        {
            _soundSource.PlayOneShot(_deathOfEnemy);
        }
    }

    private IEnumerator FadeOutCurrentAndStartNewClip(AudioClip clip)
    {
        while (_musicSource.volume > 0.01)
        {
            _musicSource.volume -= 0.01f;
            yield return new WaitForSeconds(0.02f);
        }

        _musicSource.Stop();
        _musicSource.volume = _currentVolume;
        PlayLoop(_musicSource, clip);
    }
    
    private static void PlayLoop(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.loop = true;
        source.Play();
    }
}
