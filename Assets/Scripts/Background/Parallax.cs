using Player;
using Resources;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _degree;
    private GameObject _player;
    private static bool _playerDied;
    

    private void Start()
    {
        var cachedTransform = transform;
        cachedTransform.position = new Vector3(0, 0, cachedTransform.position.z);
        PlayerCollisions.OnPlayerDeath += PlayerCollisions_OnPlayerDeath;
        _playerDied = false;
        _player = GameObject.FindGameObjectWithTag(Tags.Player);
    }

    private void Update()
    {
        if (_playerDied) return;
        var position = _player.transform.position;
        var newPosX = position.x * _degree;
        var newPosY = position.y * _degree;

        var cachedTransform = transform;
        cachedTransform.position = new Vector3(newPosX, newPosY, cachedTransform.position.z);
    }

    private static void PlayerCollisions_OnPlayerDeath(bool deathOfFire)
    {
        _playerDied = true;
    }
}