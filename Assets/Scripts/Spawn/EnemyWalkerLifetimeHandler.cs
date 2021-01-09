using Assets.Scripts.Resources;
using System.Collections;
using UnityEngine;

public class EnemyWalkerLifetimeHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Enemy))
        {
            StartCoroutine(TemporaryDisable());

            if (!collision.GetComponent<EnemyWalkerCollisions>().IsTriggeredAtLeastOnce)
            {
                collision.GetComponent<EnemyWalkerCollisions>().IsTriggeredAtLeastOnce = true;
                StartCoroutine(collision.GetComponent<EnemyLifetimeScale>().IncreaseSizeRoutine());
            }
            else
            {
                bool isDestroyEnemy = Random.Range(1, 2) == 1;

                if (isDestroyEnemy)
                {
                    StartCoroutine(collision.GetComponent<EnemyLifetimeScale>().DecreaseSizeRoutine());
                }
                else
                {
                    collision.GetComponent<EnemyWalkerMovement>().ChangeDirection();
                }
            }
        }
    }

    private IEnumerator TemporaryDisable()
    {
        yield return null;

        var collider = GetComponent<EdgeCollider2D>();
        collider.enabled = false;

        yield return new WaitForSeconds(1);

        collider.enabled = true;
    }
}
