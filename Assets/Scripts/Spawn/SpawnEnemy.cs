using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private bool _isEnemyDirectionPositive;

    public GameObject Spawn()
    {
        _enemy.GetComponent<EnemyWalkerMovement>().IsDirectionPositive = _isEnemyDirectionPositive;

        return Instantiate(_enemy, transform.position, transform.rotation);
    }
}
