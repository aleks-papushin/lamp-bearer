using Assets.Scripts;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private bool _isEnemyDirectionPositive;
    [SerializeField] private Direction _initGravity;
    [SerializeField] private GameObject _pairSpawner;
    private GameManager _gameManager;
    private const float MINSpeedMod = 0.5f;
    private const float MAXSpeedMod = 1.5f;

    private GameObject Enemy { get; set; }
    public int EnemyCount => Enemy != null ? 1 : 0;
    public int PairEnemyCount => EnemyCount + _pairSpawner.GetComponent<SpawnEnemy>().EnemyCount;


    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        Enemy = null;
    }

    public GameObject Spawn()
    {
        var cachedTransform = transform;
        Enemy = Instantiate(_enemyPrefab, cachedTransform.position, cachedTransform.rotation);
        SetInitDirection(Enemy);
        SetInitGravity(Enemy);
        SetSpeed(Enemy);

        return Enemy;
    }

    private void SetSpeedWithRandomness(GameObject enemy)
    {
        var speed = _gameManager.WaveManager.CurrentWave.enemySpeed;
        enemy.GetComponent<EnemyWalkerMovement>().Speed =
            Random.Range(speed * MINSpeedMod, speed * MAXSpeedMod);
    }

    private void SetSpeed(GameObject enemy)
    {
        var speed = _gameManager.WaveManager.CurrentWave.enemySpeed;
        enemy.GetComponent<EnemyWalkerMovement>().Speed = speed;
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
