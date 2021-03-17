using Enemy;
using Game;
using UnityEngine;

namespace Spawn
{
    public class SpawnEnemy : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private bool _isEnemyDirectionPositive;
        [SerializeField] private GameObject _pairSpawner;
        private GameObject _enemy;

        public int EnemyCount => _enemy != null ? 1 : 0;
        public bool IsFree => EnemyCount + _pairSpawner.GetComponent<SpawnEnemy>().EnemyCount == 0;
        
        public void Spawn(float speed)
        {
            var cachedTransform = transform;
            _enemy = Instantiate(_enemyPrefab, cachedTransform.position, cachedTransform.rotation);
            _enemy.GetComponent<EnemyWalkerMovement>().IsDirectionPositive = _isEnemyDirectionPositive;
            _enemy.GetComponent<EnemyWalkerMovement>().Speed = speed;
        }
    }
}
