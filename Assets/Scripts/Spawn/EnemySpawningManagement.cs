using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

namespace Spawn
{
    public class EnemySpawningManagement : MonoBehaviour
    {
        private GameWaveManager _waveManager;
        private List<SpawnEnemy> _enemySpawners;
        private int _waveEnemyCount;

        private int ActualEnemyCount => _enemySpawners.Sum(spawner => spawner.EnemyCount);

        private void Start()
        {
            _waveManager = FindObjectOfType<GameWaveManager>();
            _enemySpawners = GetComponentsInChildren<SpawnEnemy>().ToList();

            StartCoroutine(HandleEnemyExistenceRoutine());
        }
    
        private IEnumerator HandleEnemyExistenceRoutine()
        {
            while (true)
            {
                yield return null;

                _waveEnemyCount = _waveManager.CurrentWave.enemyCount;

                if (ActualEnemyCount >= _waveEnemyCount) continue;
                var spawner = PickFreeSpawner();
                yield return new WaitForSeconds(1);
                spawner.Spawn();
            }
        }

        private SpawnEnemy PickFreeSpawner()
        {
            var freeSpawners = _enemySpawners.Where(spawner => spawner.PairEnemyCount == 0).ToList();
            return freeSpawners.Count == 0 ? null : freeSpawners[Random.Range(0, freeSpawners.Count)];
        }
    }
}
