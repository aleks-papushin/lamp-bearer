using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private bool _isEnemyDirectionPositive;

    public GameObject Spawn()
    {
        var enemy = Instantiate(_enemyPrefab, transform.position, transform.rotation);
        enemy.GetComponent<EnemyWalkerMovement>().IsDirectionPositive = _isEnemyDirectionPositive;

        return enemy;
    }
}
