using Assets.Scripts.Player;
using Assets.Scripts.Resources;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _degree;
    private GameObject player;
    private static bool playerDied;
    

    private void Start()
    {
        var cachedTransform = transform;
        cachedTransform.position = new Vector3(0, 0, cachedTransform.position.z);
        PlayerCollisions.OnPlayerDied += PlayerDied;
        playerDied = false;
        player = GameObject.FindGameObjectWithTag(Tags.Player);
    }

    private void Update()
    {
        if (playerDied) return;
        var position = player.transform.position;
        var newPosX = position.x * _degree;
        var newPosY = position.y * _degree;

        var cachedTransform = transform;
        cachedTransform.position = new Vector3(newPosX, newPosY, cachedTransform.position.z);
    }

    private static void PlayerDied()
    {
        playerDied = true;
    }
}