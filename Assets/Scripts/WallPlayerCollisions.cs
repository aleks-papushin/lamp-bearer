using Assets.Scripts.Resources;
using UnityEngine;

public class WallPlayerCollisions : MonoBehaviour
{
    public bool IsPlayerStandsOnMe { get; set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Player))
        {
            IsPlayerStandsOnMe = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Player))
        {
            IsPlayerStandsOnMe = false;
        }
    }
}
