using System;
using Resources;
using UnityEngine;
using Wall;

namespace Player
{
    public class PlayerCollisions : MonoBehaviour
    {
        [SerializeField] private PlayerSounds _playerSounds;
        private PlayerController _playerController;
        private LightIntensityController _light;
        private bool _deathOfFire;
        private Rigidbody2D _rig;
        
        public static event Action<bool> OnPlayerDeath;

        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
            _light = GetComponent<LightIntensityController>();
            _rig = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnDangerousWall(collision);        
            OnPlayerGrounding(collision);
            OnEnemyCollision(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {        
            OnDangerousWall(collision);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.OilBottle))
            {
                _light.OilTaken();
            }
        }
        
        private void OnEnemyCollision(Collision2D collision)
        {
            if (collision.gameObject.tag.Contains(Tags.Enemy))
            {
                PlayerDie();
            }
        }

        private void OnPlayerGrounding(Collision2D collision)
        {
            if (!collision.gameObject.tag.Contains(Tags.WallSuffix)) return;
            _rig.velocity = Vector2.zero;
            _playerController.DirectionWasChangedInJump = false;
            _playerSounds.Landing();
        }

        private void OnDangerousWall(Collision2D collision)
        {
            if (!collision.gameObject.tag.Contains(Tags.WallSuffix)) return;
            var wall = collision.gameObject.GetComponent<WallDanger>();
            if (wall.IsDangerous)
            {
                _deathOfFire = true;
                PlayerDie();
            }
        }

        private void PlayerDie()
        {
            Destroy(gameObject);            
        }

        private void OnDestroy()
        {
            OnPlayerDeath(_deathOfFire);
        }
    }
}
