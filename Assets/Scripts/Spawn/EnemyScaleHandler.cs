using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Enemy;
using Resources;
using UnityEngine;

namespace Spawn
{
    public class EnemyScaleHandler : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(Tags.Enemy)) return;
            StartCoroutine(TemporaryDisable());

            if (!collision.GetComponent<EnemyWalkerCollisions>().IsTriggeredAtLeastOnce)
            {
                collision.GetComponent<EnemyWalkerCollisions>().IsTriggeredAtLeastOnce = true;
                collision.GetComponent<EnemyScaling>().IsIncrease = true;
            }
            else
            {
                var isDestroyEnemy = Random.Range(1, 2) == 1;

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

        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess", Justification = "Can't be optimized")]
        private IEnumerator TemporaryDisable()
        {
            var edgeCollider = GetComponent<EdgeCollider2D>();
            edgeCollider.enabled = false;
            yield return new WaitForSeconds(1);
            edgeCollider.enabled = true;
        }
    }
}
