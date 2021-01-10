using Assets.Scripts;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private bool _isEnemyDirectionPositive;
    [SerializeField] private Direction _initGravity;
    [SerializeField] private GameObject _pairSpawner;
    private GameManager _gameManager;
    private readonly float _minSpeedMod = 0.5f;
    private readonly float _maxSpeedMod = 1.5f;

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
        _gameManager = FindObjectOfType<GameManager>();
        Enemy = null;
    }

    public GameObject Spawn()
    {
        Enemy = Instantiate(_enemyPrefab, transform.position, transform.rotation);
        SetInitDirection(Enemy);
        SetInitGravity(Enemy);
        SetSpeed(Enemy);

        return Enemy;
    }

    private void SetSpeed(GameObject enemy)
    {
        var speed = _gameManager.WaveManager.CurrentWave.enemySpeed;
        enemy.GetComponent<EnemyWalkerMovement>().Speed =
            Random.Range(speed * _minSpeedMod, speed * _maxSpeedMod);
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
