using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Game
{
    public class GameTimer : MonoBehaviour
    {
        private GameWaveManager _gameWaveManager;
        public static event Action OnWaveIncrementing;
        
        private void Start()
        {
            _gameWaveManager = FindObjectOfType<GameWaveManager>();
            StartCoroutine(HandleWaveIncrementing(_gameWaveManager.CurrentWave.waveDuration));
        }

        private IEnumerator HandleWaveIncrementing(float waveIncrementIntervalSec)
        {
            yield return new WaitForSeconds(waveIncrementIntervalSec);
            OnWaveIncrementing?.Invoke();
            StartCoroutine(HandleWaveIncrementing(_gameWaveManager.CurrentWave.waveDuration));
        }
    }
}