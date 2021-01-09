using Assets.Scripts.Resources;
using UnityEngine;

public class EnemyWalkerCollisions : MonoBehaviour
{
    public bool IsTriggeredAtLeastOnce { get; set; } = false; // is uisng to handle existance of enemy by spawner triggers

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Enemy))
        {
            collision.gameObject.GetComponent<EnemyWalkerMovement>().ChangeDirection();
        }
    }
}
