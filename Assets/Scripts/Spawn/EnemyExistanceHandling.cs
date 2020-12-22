using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyExistanceHandling : MonoBehaviour
{
    [SerializeField] private float[] _enemyAliveCheckInterval; 

    private List<SpawnEnemy> _enemySpawners;
    private GameObject _enemy = null;

    void Start()
    {
        _enemySpawners = GetComponentsInChildren<SpawnEnemy>().ToList();
        StartCoroutine(HandleEnemyExistanceRoutine());
    }

    private IEnumerator HandleEnemyExistanceRoutine()
    {
        while (true)
        {
            if (IsEnemyAlive())
            {
                float time = Random.Range(_enemyAliveCheckInterval[0], _enemyAliveCheckInterval[1]);
                yield return new WaitForSeconds(time);
            }
            else
            {
                var spawner = _enemySpawners[Random.Range(0, 2)];
                _enemy = spawner.Spawn();
            }
        }
    }

    private bool IsEnemyAlive()
    {
        return _enemy != null;
    }
}
