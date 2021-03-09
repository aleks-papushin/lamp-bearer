using System;
using Enums;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        // animation
        [SerializeField] private Animator _animator;

        // jumping
        [SerializeField] private float _jumpForce;
        private bool _isJumpAxisWasIdle = true;
        internal bool _isChangedDirectionInJump;
        [SerializeField] private float _forbidDirectionChangingDistance;

        // collisions
        private PlayerCollisions _playerCollisions;
        private PlayerWallCollisions _playerWallCollisions;

        // scripts
        [SerializeField] private PlayerSounds _playerSounds;
        [SerializeField] private PlayerGravityHandler _gravityHandler;

        // other
        private PlayerRunning _playerRunning;
        private Rigidbody2D _rig;
        private bool _isSideAxisWasHeld;
        private static readonly int Jumping = Animator.StringToHash("IsJumping");

        private static bool IsInputHorizontalNegative => Input.GetAxisRaw("Horizontal") < 0;
        private static bool IsInputHorizontalPositive => Input.GetAxisRaw("Horizontal") > 0;
        private static bool IsInputVerticalNegative => Input.GetAxisRaw("Vertical") < 0;
        private static bool IsInputVerticalPositive => Input.GetAxisRaw("Vertical") > 0;

        private bool IsGrounded { get; set; }

        public static GameObject Player { get; private set; }

        private void Awake()
        {
            Player = gameObject;
        }

        private void Start()
        {
            _rig = GetComponent<Rigidbody2D>();
            _playerRunning = GetComponent<PlayerRunning>();
            _playerCollisions = GetComponent<PlayerCollisions>();
            _playerWallCollisions = GetComponent<PlayerWallCollisions>();
            _playerCollisions.SetAnimator(_animator);
            _gravityHandler.SwitchLocalGravity(Direction.Down);
            PlayerWallCollisions.OnIsGroundedChanged += PlayerWallCollisions_OnIsGroundedChanged;
        }

        private void FixedUpdate()
        {
            HandleJumping();
            HandleInAirDirectionChanging();
        }

        public void UnfreezeRig()
        {
            _rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    
        private void HandleJumping()
        {
            // if on the surface, add impulse force in the opposite side
            if (_isJumpAxisWasIdle && Input.GetAxisRaw("Jump") > 0)
            {
                if (_playerWallCollisions.IsTouchBottomWall)
                {
                    Jump(Direction.Up, new Vector2(0, _jumpForce));
                }
                else if (_playerWallCollisions.IsTouchUpperWall)
                {
                    Jump(Direction.Down, new Vector2(0, -_jumpForce));
                }
                else if (_playerWallCollisions.IsTouchLeftWall)
                {
                    Jump(Direction.Right, new Vector2(_jumpForce, 0));
                }
                else if (_playerWallCollisions.IsTouchRightWall)
                {
                    Jump(Direction.Left, new Vector2(-_jumpForce, 0));
                }

                _isJumpAxisWasIdle = false;
                
            }
            else if (Input.GetAxisRaw("Jump") == 0)
            {
                _isJumpAxisWasIdle = true;                
            }
        }

        // This method prevents player from turning aside in jump
        // in case if direction button was held in moment when player jumps
        private void SetIsSideAxisHeld()
        {
            if (_gravityHandler.IsGravityVectorHorizontal)
            {
                _isSideAxisWasHeld = Input.GetAxisRaw("Vertical") != 0;
            }
            else
            {
                _isSideAxisWasHeld = Input.GetAxisRaw("Horizontal") != 0;
            }
        }

        private void Jump(Direction gravity, Vector2 jumpVector)
        {
            SetIsSideAxisHeld();

            // HACK to stop velocity changing immediately
            _playerRunning.IsGrounded = false;

            FreezePerpendicularAxis(gravity);
            _gravityHandler.SwitchLocalGravity(gravity);
            _rig.AddForce(jumpVector, ForceMode2D.Impulse);
            _animator.SetBool(Jumping, true);
            _playerSounds.Jump();
        }

        // This method was added because of slight side movement
        // observed in case if player pressed jump and side movement buttons simultaneously
        private void FreezePerpendicularAxis(Direction gravity)
        {
            switch (gravity)
            {
                case Direction.Down:
                case Direction.Up:
                    _rig.constraints = RigidbodyConstraints2D.FreezePositionX;
                    break;
                case Direction.Left:
                case Direction.Right:
                    _rig.constraints = RigidbodyConstraints2D.FreezePositionY;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gravity), gravity, null);
            }
        }

        private bool ForbidInAirTurning()
        {
            return
                _isChangedDirectionInJump ||
                IsGrounded ||
                transform.DistanceToAnyFloorLessThan(_forbidDirectionChangingDistance);
        }

        private void HandleInAirDirectionChanging()
        {
            // if side buttons were held before jump - 
            // must not change direction until button is up and pressed again
            if (_isSideAxisWasHeld) 
            {
                SetIsSideAxisHeld();
            }
            else
            {
                if (ForbidInAirTurning()) return;

                if (Input.GetAxisRaw("Horizontal") != 0 && _gravityHandler.IsGravityVectorVertical)
                {
                    _isChangedDirectionInJump = true;

                    if (IsInputHorizontalNegative)
                    {
                        Jump(Direction.Left, new Vector2(-_jumpForce, 0));
                    }
                    else if (IsInputHorizontalPositive)
                    {
                        Jump(Direction.Right, new Vector2(_jumpForce, 0));
                    }
                }
                else if (Input.GetAxisRaw("Vertical") != 0 && _gravityHandler.IsGravityVectorHorizontal)
                {
                    _isChangedDirectionInJump = true;

                    if (IsInputVerticalNegative)
                    {
                        Jump(Direction.Down, new Vector2(0, -_jumpForce));
                    }
                    else if (IsInputVerticalPositive)
                    {
                        Jump(Direction.Up, new Vector2(0, _jumpForce));
                    }
                }
            }
        }

        private void PlayerWallCollisions_OnIsGroundedChanged(bool isGrounded)
        {
            IsGrounded = isGrounded;
        }

        private void OnDestroy()
        {
            PlayerWallCollisions.OnIsGroundedChanged -= PlayerWallCollisions_OnIsGroundedChanged;
        }
    }
}

