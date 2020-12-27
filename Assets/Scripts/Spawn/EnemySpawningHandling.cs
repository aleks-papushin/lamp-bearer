using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawningHandling : MonoBehaviour
{
    private GameManager _gameManager;
    private List<SpawnEnemy> _enemySpawners;
    private int _waveEnemyCount;

    public int ActualEnemyCount
    {
        get
        {
            int count = 0;
            foreach (var spawner in _enemySpawners)
            {
                count += spawner.EnemyCount;
            }
            return count;
        }
    }

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _enemySpawners = GetComponentsInChildren<SpawnEnemy>().ToList();

        StartCoroutine(HandleEnemyExistanceRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator HandleEnemyExistanceRoutine()
    {
        while (true)
        {
            yield return null;

            _waveEnemyCount = _gameManager.WaveManager.CurrentWave.enemyCount;

            if (ActualEnemyCount < _waveEnemyCount)
            {
                SpawnEnemy spawner = this.PickFreeSpawner();
                spawner.Spawn();
            }
        }
    }

    private SpawnEnemy PickFreeSpawner()
    {
        var freeSpawners = new List<SpawnEnemy>();

        foreach (var spawner in _enemySpawners)
        {
            if (spawner.PairEnemyCount == 0) freeSpawners.Add(spawner);
        }

        if (freeSpawners.Count == 0) return null;

        return freeSpawners[UnityEngine.Random.Range(0, freeSpawners.Count)];
    }
}
