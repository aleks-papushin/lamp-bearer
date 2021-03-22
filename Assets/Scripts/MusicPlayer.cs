using System.Collections;
using UI;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _intro;
    [SerializeField] private AudioClip[] _gameMusic;

    private AudioSource _source;
    private float _currentVolume;

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
    
    private static void PlayLoop(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.loop = true;
        source.Play();
    }
}
