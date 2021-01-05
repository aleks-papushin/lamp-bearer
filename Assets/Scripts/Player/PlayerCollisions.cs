using Assets.Scripts.Resources;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerCollisions : MonoBehaviour
    {
        [SerializeField] private PlayerSounds _playerSounds;
        private GameManager _gameManager;
        private PlayerController _playerController;
        private LightIntensityController _light;
        private Animator _animator;

        public static event Action OnOilBottleTaken;
        public static event Action OnPlayerDied;

        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _playerController = GetComponent<PlayerController>();
            _light = GetComponent<LightIntensityController>();
        }

        public void SetAnimator(Animator animator)
        {
            _animator = animator;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            this.HandleWalls(collision);        
            this.HandlePlayerGrounding(collision);
            this.HandleEnemy(collision);
        }

        private void HandleEnemy(Collision2D collision)
        {
            if (collision.gameObject.tag.Contains(Tags.Enemy))
            {
                this.HandlePlayerDead();
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag.Contains(Tags.WallSuffix))
            {
                this.HandleBeingOnWall(collision, isOnWall: false);
            }
        }

        private void HandleWalls(Collision2D collision)
        {
            if (collision.gameObject.tag.Contains(Tags.WallSuffix))
            {
                this.HandleDangerousWall(collision);
                _animator.SetBool("IsJumping", false);
                this.HandleBeingOnWall(collision, isOnWall: true);
            }
        }

        private void HandleBeingOnWall(Collision2D collision, bool isOnWall)
        {
            collision.transform.GetComponent<WallDanger>().IsPlayerStandsOnMe = isOnWall;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {        
            this.HandleDangerousWall(collision);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.OilBottle))
            {
                OilBottleTaken(collision);
            }
        }

        private void OilBottleTaken(Collider2D oilBottle)
        {
            Destroy(oilBottle.gameObject);
            _gameManager.UpdateScore(1);
            _playerSounds.OilTaken();
            _light.OilTaken();
            OnOilBottleTaken?.Invoke();
        }

        private void HandlePlayerGrounding(Collision2D collision)
        {
            if (collision.gameObject.tag.Contains(Tags.WallSuffix))
            {
                _playerController._isChangedDirectionInJump = false;
                _playerController.UnfreezeRig();
                _playerSounds.Landing();
            }
        }

        private void HandleDangerousWall(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<WallDanger>().IsDangerous)
            {
                this.HandlePlayerDead();
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
