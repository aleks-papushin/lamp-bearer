using Assets.Scripts.Resources;
using UnityEngine;

// Currently is not used
public class EnemyZFreezeHandling : MonoBehaviour
{
    private Rigidbody2D _rig;

    private void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains(Tags.SpawnerSuffix))
        {
            _rig.freezeRotation = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains(Tags.SpawnerSuffix))
        {
            _rig.freezeRotation = true;
        }
    }
}
