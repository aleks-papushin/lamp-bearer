using System;
using Resources;
using UnityEngine;
using Wall;

namespace Player
{
    public class PlayerCollisions : MonoBehaviour
    {
        private LightIntensityController _light;
        private bool _deathOfFire;

        public static event Action<bool> OnPlayerDeath;

        private void Start()
        {
            _light = GetComponent<LightIntensityController>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnDangerousWall(collision);  
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
            OnPlayerDeath?.Invoke(_deathOfFire);
        }
    }
}
