using Assets.Scripts.Resources;
using UnityEngine;

public class EnemyWalkerCollisions : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Enemy))
        {
            collision.gameObject.GetComponent<EnemyWalkerMovement>().ChangeDirection();
        }
    }
}
