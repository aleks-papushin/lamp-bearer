using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class PlayMusic : SoundSource
{
    [SerializeField] private AudioClip _intro;
    [SerializeField] private AudioClip[] _gameMusic;

    private AudioSource _source;
    private float _currentVolume;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _source = GetComponent<AudioSource>();        
        _currentVolume = _source.volume;
        PlayLoop(_source, _intro);
        StartButton.OnGameStarted += StartButton_OnGameStarted;
    }

    private void StartButton_OnGameStarted()
    {
        var clipNumber = Random.Range(0, _gameMusic.Length);
        StartCoroutine(FadeOutCurrentAndStartNewClip(_gameMusic[clipNumber]));
    }

    private IEnumerator FadeOutCurrentAndStartNewClip(AudioClip clip)
    {
        while (_source.volume > 0.01)
        {
            _source.volume -= 0.01f;
            yield return new WaitForSeconds(0.02f);
        }

        _source.Stop();
        _source.volume = _currentVolume;
        PlayLoop(_source, clip);
    }
}
