using Assets.Scripts.Resources;
using System.Collections;
using UnityEngine;

public class EnemyScaleHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Enemy))
        {
            StartCoroutine(TemporaryDisable());

            if (!collision.GetComponent<EnemyWalkerCollisions>().IsTriggeredAtLeastOnce)
            {
                collision.GetComponent<EnemyWalkerCollisions>().IsTriggeredAtLeastOnce = true;
                collision.GetComponent<EnemyScaling>().IsIncrease = true;
            }
            else
            {
                bool isDestroyEnemy = Random.Range(1, 2) == 1;

                if (isDestroyEnemy)
                {
                    collision.GetComponent<EnemyScaling>().IsDestroy = true;
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
        var collider = GetComponent<EdgeCollider2D>();
        collider.enabled = false;

        yield return new WaitForSeconds(1);

        collider.enabled = true;
    }
}
