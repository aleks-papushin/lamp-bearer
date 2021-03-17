using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Game
{
    public class GameTimer : MonoBehaviour
    {
        private Stopwatch _time;
        private TimeSpan _lastWaveIncrementTime = TimeSpan.FromSeconds(0);
        public static event Action OnWaveIncrementing;

        private float WaveIncrementIntervalSec { get; set; }

        private bool IsTimeToIncrementWave
        {
            get
            {
                if (_time.Elapsed <= _lastWaveIncrementTime + TimeSpan.FromSeconds(WaveIncrementIntervalSec))
                    return false;
                _lastWaveIncrementTime = _time.Elapsed;
                return true;

            }
        }            

        private void Start()
        {
            WaveIncrementIntervalSec = FindObjectOfType<GameWaveManager>().CurrentWave.waveDuration;

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
