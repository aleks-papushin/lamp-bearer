using System;
using Resources;
using Spawn;
using UnityEngine;
using Wall;

namespace Player
{
    public class PlayerCollisions : MonoBehaviour
    {
        [SerializeField] private PlayerSounds _playerSounds;
        private PlayerController _playerController;
        private LightIntensityController _light;
        
        public static event Action OnPlayerDied;

        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
            _light = GetComponent<LightIntensityController>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HandleWalls(collision);        
            HandlePlayerGrounding(collision);
            HandleEnemy(collision);
        }

        private void HandleEnemy(Collision2D collision)
        {
            if (collision.gameObject.tag.Contains(Tags.Enemy))
            {
                HandlePlayerDead();
            }
        }

        private void HandleWalls(Collision2D collision)
        {
            if (!collision.gameObject.tag.Contains(Tags.WallSuffix)) return;
            HandleDangerousWall(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {        
            HandleDangerousWall(collision);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.OilBottle))
            {
                _light.OilTaken();
            }
        }

        private void HandlePlayerGrounding(Collision2D collision)
        {
            if (!collision.gameObject.tag.Contains(Tags.WallSuffix)) return;
            _playerController.DirectionWasChangedInJump = false;
            _playerController.UnfreezeRig();
            _playerSounds.Landing();
        }

        private void HandleDangerousWall(Collision2D collision)
        {
            var wall = collision.gameObject.GetComponent<WallDanger>();
            if (wall != null && wall.IsDangerous)
            {
                HandlePlayerDead();
            }
        }

        private void HandlePlayerDead()
        {
            Destroy(gameObject);            
        }

        private void OnDestroy()
        {
            OnPlayerDied?.Invoke();
        }
    }
}
