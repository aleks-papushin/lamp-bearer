﻿using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        // animation
        [SerializeField] private Animator _animator;

        // jumping
        [SerializeField] private float _jumpForce;
        private bool _isJumpAxisWasIdle = true;
        internal bool _isChangedDirectionInJump = false;
        [SerializeField] private float _forbidDirectionChangingDistance;

        // collisions
        private PlayerCollisions _playerCollisions;
        private PlayerWallCollisions _playerWallCollisions;

        // scripts
        [SerializeField] private PlayerSounds _playerSounds;
        [SerializeField] private PlayerGravityHandler _gravityHandler;
        [SerializeField] private HandleObjectFacing _spriteFacing;
        [SerializeField] private HandleSpriteRotation _spriteRotation;

        // other
        private PlayerRunning _playerRunning;
        private Rigidbody2D _rig;
        private bool _isSideAxisWasHeld = false;

        public bool IsInputHorisontalNegative => Input.GetAxisRaw("Horizontal") < 0;
        public bool IsInputHorizontalPositive => Input.GetAxisRaw("Horizontal") > 0;
        public bool IsInputVerticalNegative => Input.GetAxisRaw("Vertical") < 0;
        public bool IsInputVerticalPositive => Input.GetAxisRaw("Vertical") > 0;

        public bool IsGrounded { get; set; }

        public static GameObject Player { get; private set; }

        private void Awake()
        {
            Player = gameObject;
        }

        void Start()
        {
            _rig = GetComponent<Rigidbody2D>();
            _playerRunning = GetComponent<PlayerRunning>();
            _playerCollisions = GetComponent<PlayerCollisions>();
            _playerWallCollisions = GetComponent<PlayerWallCollisions>();
            _playerCollisions.SetAnimator(_animator);
            _gravityHandler.SwitchLocalGravity(Direction.Down);
            PlayerWallCollisions.OnIsGroundedChanged += PlayerWallCollisions_OnIsGroundedChanged;
        }

        void FixedUpdate()
        {
            this.HandleJumping();
            this.HandleInAirDirectionChanging();
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
                    this.Jump(Direction.Up, new Vector2(0, _jumpForce));
                }
                else if (_playerWallCollisions.IsTouchUpperWall)
                {
                    this.Jump(Direction.Down, new Vector2(0, -_jumpForce));
                }
                else if (_playerWallCollisions.IsTouchLeftWall)
                {
                    this.Jump(Direction.Right, new Vector2(_jumpForce, 0));
                }
                else if (_playerWallCollisions.IsTouchRightWall)
                {
                    this.Jump(Direction.Left, new Vector2(-_jumpForce, 0));
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
            this.SetIsSideAxisHeld();

            // HACK to stop velocity changing immediately
            _playerRunning.IsGrounded = false;

            this.FreezePerpendicularAxis(gravity);
            _gravityHandler.SwitchLocalGravity(gravity);
            _rig.AddForce(jumpVector, ForceMode2D.Impulse);
            _animator.SetBool("IsJumping", true);
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
            }
        }

        private bool ForbidInAirTurning()
        {
            return
                _isChangedDirectionInJump ||
                IsGrounded ||
                transform.IsDistanceToAnyFloorLessThan(_forbidDirectionChangingDistance);
        }

        private void HandleInAirDirectionChanging()
        {
            // if side buttons were held before jump - 
            // must not change direction until button is up and pressed again
            if (_isSideAxisWasHeld) 
            {
                this.SetIsSideAxisHeld();
            }
            else
            {
                if (ForbidInAirTurning()) return;

                if (Input.GetAxisRaw("Horizontal") != 0 && _gravityHandler.IsGravityVectorVertical)
                {
                    _isChangedDirectionInJump = true;

                    if (IsInputHorisontalNegative)
                    {
                        this.Jump(Direction.Left, new Vector2(-_jumpForce, 0));
                    }
                    else if (IsInputHorizontalPositive)
                    {
                        this.Jump(Direction.Right, new Vector2(_jumpForce, 0));
                    }
                }
                else if (Input.GetAxisRaw("Vertical") != 0 && _gravityHandler.IsGravityVectorHorizontal)
                {
                    _isChangedDirectionInJump = true;

                    if (IsInputVerticalNegative)
                    {
                        this.Jump(Direction.Down, new Vector2(0, -_jumpForce));
                    }
                    else if (IsInputVerticalPositive)
                    {
                        this.Jump(Direction.Up, new Vector2(0, _jumpForce));
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

