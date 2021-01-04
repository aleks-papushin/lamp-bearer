using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameTimer : MonoBehaviour
    {
        private Stopwatch _time;
        private TimeSpan _lastWaveIncrementTime = TimeSpan.FromSeconds(0);
        private GameManager _gameManager;
        public static event Action OnWaveIncrementing;

        public float WaveIncrementIntervalSec { get; set; }

        public bool IsTimeToIncrementWave
        {
            get
            {
                if (_time.Elapsed > (_lastWaveIncrementTime + TimeSpan.FromSeconds(WaveIncrementIntervalSec)))
                {
                    _lastWaveIncrementTime = _time.Elapsed;
                    return true;
                }
                else return false;
            }
        }            

        private void Start()
        {
            _gameManager = GetComponent<GameManager>();
            WaveIncrementIntervalSec = _gameManager.WaveManager.CurrentWave.waveDuration;

            _time = new Stopwatch();
            _time.Start();

            StartCoroutine(HandleWaveIncrementing());
        }

        private IEnumerator HandleWaveIncrementing()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                if (IsTimeToIncrementWave)
                {

                    OnWaveIncrementing?.Invoke();
                }
            }
        }
    }
}
