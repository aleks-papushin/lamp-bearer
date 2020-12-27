using Assets.Scripts;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private bool _isEnemyDirectionPositive;
    [SerializeField] private Direction _initGravity;
    [SerializeField] private float[] _speedInterval;
    [SerializeField] private GameObject _pairSpawner;

    public GameObject Enemy { get; set; }
    public int EnemyCount
    {
        get
        {
            if (Enemy != null) return 1;
            else return 0;
        }
    }
    public int PairEnemyCount => EnemyCount + _pairSpawner.GetComponent<SpawnEnemy>().EnemyCount;


    private void Awake()
    {
        Enemy = null;
    }

    public GameObject Spawn()
    {
        Enemy = Instantiate(_enemyPrefab, transform.position, transform.rotation);
        SetInitDirection(Enemy);
        SetInitGravity(Enemy);
        SetInitSpeed(Enemy);

        return Enemy;
    }

    private void SetInitSpeed(GameObject enemy)
    {
        enemy.GetComponent<EnemyWalkerMovement>().Speed =
            Random.Range(_speedInterval[0], _speedInterval[1]);
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
