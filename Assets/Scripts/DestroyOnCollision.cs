using Assets.Scripts.Resources;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Enemy))
        {
            Destroy(collision.gameObject);
        }
    }
}
