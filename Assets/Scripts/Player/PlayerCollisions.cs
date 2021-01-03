using Assets.Scripts.Interfaces;
using Assets.Scripts.Resources;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerCollisions : MonoBehaviour, IGroundedStateHandler
    {
        [SerializeField] private PlayerSounds _playerSounds;
        private GameManager _gameManager;
        private PlayerController _playerController;
        private LightIntensityController _light;
        private Animator _animator;

        public bool IsTouchingBottom => this.IsTouching(Tags.BottomWall);
        public bool IsTouchingUpperWall => this.IsTouching(Tags.UpperWall);
        public bool IsTouchingLeftWall => this.IsTouching(Tags.LeftWall);
        public bool IsTouchingRightWall => this.IsTouching(Tags.RightWall);

        public bool IsGrounded { get; set; }

        public static event Action OnOilBottleTaken;

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
                Destroy(gameObject);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag.Contains(Tags.WallSuffix))
            {
                IsGrounded = false;
                this.HandleBeingOnWall(collision, isOnWall: false);
            }
        }

        private void HandleWalls(Collision2D collision)
        {
            if (collision.gameObject.tag.Contains(Tags.WallSuffix))
            {
                this.HandleDangerousWall(collision);
                IsGrounded = true;
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

        private void OilBottleTaken(Collider2D collision)
        {
            Destroy(collision.gameObject);
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
                Destroy(gameObject);
            }
        }

        private bool IsTouching(string tagName)
        {
            var otherCollider = GameObject.FindGameObjectWithTag(tagName).GetComponent<Collider2D>();
            return transform.GetChild(0).GetComponent<Collider2D>().IsTouching(otherCollider);
        }
    }
}
