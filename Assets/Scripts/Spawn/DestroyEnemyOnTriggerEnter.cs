using Assets.Scripts.Resources;
using UnityEngine;

public class DestroyEnemyOnTriggerEnter : MonoBehaviour
{
    void Update()
    {
                
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Enemy))
        {
            Destroy(collision.gameObject);
        }
    }
}
