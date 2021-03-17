using Enemy;
using Game;
using UnityEngine;

namespace Spawn
{
    public class SpawnEnemy : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private bool _isEnemyDirectionPositive;
        [SerializeField] private Direction _initGravity;
        [SerializeField] private GameObject _pairSpawner;
        private GameWaveManager _gameWaveManager;

        private GameObject Enemy { get; set; }
        public int EnemyCount => Enemy != null ? 1 : 0;
        public int PairEnemyCount => EnemyCount + _pairSpawner.GetComponent<SpawnEnemy>().EnemyCount;


        private void Awake()
        {
            _gameWaveManager = FindObjectOfType<GameWaveManager>();
            Enemy = null;
        }

        public void Spawn()
        {
            var cachedTransform = transform;
            Enemy = Instantiate(_enemyPrefab, cachedTransform.position, cachedTransform.rotation);
            SetInitDirection(Enemy);
            SetSpeed(Enemy);
        }

        private void SetSpeed(GameObject enemy)
        {
            var speed = _gameWaveManager.CurrentWave.enemySpeed;
            enemy.GetComponent<EnemyWalkerMovement>().Speed = speed;
        }

        private void SetInitDirection(GameObject enemy)
        {
            enemy.GetComponent<EnemyWalkerMovement>().IsDirectionPositive = _isEnemyDirectionPositive;
        }
    }
}
