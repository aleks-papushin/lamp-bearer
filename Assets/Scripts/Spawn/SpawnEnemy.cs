using Assets.Scripts;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private bool _isEnemyDirectionPositive;
    [SerializeField] private Direction _initGravity;
    [SerializeField] private float[] _speedInterval;

    public GameObject Spawn()
    {
        var enemy = Instantiate(_enemyPrefab, transform.position, transform.rotation);
        SetInitDirection(enemy);
        SetInitGravity(enemy);
        SetInitSpeed(enemy);

        return enemy;
    }

    private void SetInitSpeed(GameObject enemy)
    {
        enemy.GetComponent<EnemyWalkerMovement>().Speed = 
            UnityEngine.Random.Range(_speedInterval[0], _speedInterval[1]);
    }

    private void SetInitGravity(GameObject enemy)
    {
        enemy.GetComponent<GravityHandler>().SwitchLocalGravity(_initGravity);
    }

    private void SetInitDirection(GameObject enemy)
    {
        enemy.GetComponent<EnemyWalkerMovement>().IsDirectionPositive = _isEnemyDirectionPositive;
    }
}
