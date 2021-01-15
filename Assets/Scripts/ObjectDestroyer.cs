using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    private void OnCollisionExit2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
    }
}
