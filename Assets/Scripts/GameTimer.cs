using System;
using System.Diagnostics;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameTimer : MonoBehaviour
    {
        private Stopwatch _time;
        private TimeSpan _lastWaveIncrementTime = TimeSpan.FromSeconds(0);
        [SerializeField] private int _waveIncrementIntervalSec;

        public bool IsTimeToIncrementWave
        {
            get
            {
                if (_time.Elapsed > (_lastWaveIncrementTime + TimeSpan.FromSeconds(_waveIncrementIntervalSec)))
                {
                    _lastWaveIncrementTime = _time.Elapsed;
                    return true;
                }
                else return false;
            }
        }            

        private void Start()
        {
            _time = new Stopwatch();
            _time.Start();
        }
    }
}
