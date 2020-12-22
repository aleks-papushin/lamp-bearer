using Assets.Scripts.Resources;
using UnityEngine;

public class EnemyWalkerCollisions : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Enemy))
        {
            bool isDestroyEnemy = Random.Range(0, 2) == 1;

            if (isDestroyEnemy) Destroy(collision.gameObject);
            else
            {
                collision.GetComponent<EnemyWalkerMovement>().IsDirectionPositive = 
                    !collision.GetComponent<EnemyWalkerMovement>().IsDirectionPositive;
            }
        }
    }
}
