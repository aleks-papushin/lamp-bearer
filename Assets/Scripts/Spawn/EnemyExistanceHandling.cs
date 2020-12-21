using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyExistanceHandling : MonoBehaviour
{
    private List<SpawnEnemy> _enemySpawners;
    private GameObject _enemy;

    // Start is called before the first frame update
    void Start()
    {
        _enemySpawners = GetComponentsInChildren<SpawnEnemy>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(HandleEnemyExistanceRoutine());
    }

    private IEnumerator HandleEnemyExistanceRoutine()
    {
        while (true)
        {
            if (IsEnemyAlive())
            {
                yield return new WaitForSeconds(1);
            }
            else
            {
                // TODO pick random enemy spawner                
                // spawn new enemy
                _enemy = _enemySpawners.First().Spawn();
            }
        }
    }

    private bool IsEnemyAlive()
    {
        return _enemy != null;
    }
}
