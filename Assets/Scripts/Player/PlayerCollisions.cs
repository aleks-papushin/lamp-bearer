using Assets.Scripts.Enums;
using Assets.Scripts.Resources;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerCollisions : MonoBehaviour
    {
        private GameManager _gameManager;
        private PlayerController _playerController;
        private Animator _animator;

        public bool IsTouchingBottom => this.IsTouching(TagNames.BottomWallTag);
        public bool IsTouchingUpperWall => this.IsTouching(TagNames.UpperWallTag);
        public bool IsTouchingLeftWall => this.IsTouching(TagNames.LeftWallTag);
        public bool IsTouchingRightWall => this.IsTouching(TagNames.RightWallTag);

        public bool IsGrounded { get; internal set; }

        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _playerController = GetComponent<PlayerController>();
        }

        public void SetAnimator(Animator animator)
        {
            _animator = animator;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            this.HandleWalls(collision);        
            this.HandlePlayerGrounding(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag.Contains(TagNames.WallTagSuffix))
            {
                IsGrounded = false;
                this.HandleBeingOnWall(collision, isOnWall: false);
            }
        }

        private void HandleWalls(Collision2D collision)
        {
            if (collision.gameObject.tag.Contains(TagNames.WallTagSuffix))
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
            if (collision.gameObject.CompareTag(TagNames.OilBottle))
            {
                _gameManager.UpdateScore(10);
                Destroy(collision.gameObject);
            }

            this.HandleCorners(collision);
        }

        private void HandleCorners(Collider2D collision)
        {
            if (collision.gameObject.tag.Contains(TagNames.CornerTagSuffix))
            {
                var tag = collision.gameObject.tag;

                switch (tag)
                {
                    case TagNames.BottomLeftCornerTag:
                        _playerController.CurrentCorner = Corner.BottomLeft;
                        break;
                    case TagNames.BottomRightCornerTag:
                        _playerController.CurrentCorner = Corner.BottomRight;
                        break;
                    case TagNames.UpperLeftCornerTag:
                        _playerController.CurrentCorner = Corner.UpperLeft;
                        break;
                    case TagNames.UpperRightCornerTag:
                        _playerController.CurrentCorner = Corner.UpperRight;
                        break;
                }

                _playerController.CornerJump();
            }
        }

        private void HandlePlayerGrounding(Collision2D collision)
        {
            if (collision.gameObject.tag.Contains(TagNames.WallTagSuffix))
            {
                _playerController._isChangedDirectionInJump = false;
                _playerController.UnfreezeRig();
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
