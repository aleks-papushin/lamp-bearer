using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyExistanceHandling : MonoBehaviour
{
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
                yield return new WaitForSeconds(3);
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
