using System;
using System.Linq;
using Resources;
using UnityEngine;
using Utils;
using Wall;

namespace Player
{
    public class PlayerJumping : MonoBehaviour
    {
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _forbidDirectionChangingDistance;
        [SerializeField] private PlayerSounds _playerSounds;
        [SerializeField] private PlayerGravityHandler _gravityHandler;
        
        private bool _isJumpAxisWasIdle = true;
        private bool _directionWasChangedInJump;
        private PlayerWallCollisions _playerWallCollisions;
        private Rigidbody2D _rig;
        private bool _isSideAxisWasHeld;

        private static bool IsInputHorizontalNegative => Input.GetAxisRaw("Horizontal") < 0;
        private static bool IsInputHorizontalPositive => Input.GetAxisRaw("Horizontal") > 0;
        private static bool IsInputVerticalNegative => Input.GetAxisRaw("Vertical") < 0;
        private static bool IsInputVerticalPositive => Input.GetAxisRaw("Vertical") > 0;

        private bool IsDistanceToAnyFloorForbidsChange
        {
            get
            {
                return FindObjectsOfType<WallDanger>().Any(wall => transform.GetDistanceTo(wall.gameObject)  < _forbidDirectionChangingDistance);
            }
        }

        private void Start()
        {
            _rig = GetComponent<Rigidbody2D>();
            _playerWallCollisions = GetComponent<PlayerWallCollisions>();
            _gravityHandler.SwitchLocalGravity(Direction.Down);
        }

        private void Update()
        {
            HandleJumping();
            HandleInAirDirectionChanging();
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnPlayerGrounding(collision);
        }
        
        private void OnPlayerGrounding(Collision2D collision)
        {
            if (!collision.gameObject.tag.Contains(Tags.WallSuffix)) return;
            _rig.velocity = Vector2.zero;
            _directionWasChangedInJump = false;
            _playerSounds.Landing();
        }

        private void HandleJumping()
        {
            if (_isJumpAxisWasIdle && Input.GetAxisRaw("Jump") > 0)
            {
                if (_playerWallCollisions.IsTouchBottomWall)
                {
                    Jump(Direction.Up, true);
                }
                else if (_playerWallCollisions.IsTouchUpperWall)
                {
                    Jump(Direction.Down, true);
                }
                else if (_playerWallCollisions.IsTouchLeftWall)
                {
                    Jump(Direction.Right, true);
                }
                else if (_playerWallCollisions.IsTouchRightWall)
                {
                    Jump(Direction.Left, true);
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
            if (Vector2.Dot(_rig.velocity, Vector2.up) == 0)
            {
                _isSideAxisWasHeld = Input.GetAxisRaw("Vertical") != 0;
            }
            else
            {
                _isSideAxisWasHeld = Input.GetAxisRaw("Horizontal") != 0;
            }
        }

        private void Jump(Direction direction, bool fromGround)
        {
            var jumpVector = direction switch
            {
                Direction.Left => Vector2.left,
                Direction.Down => Vector2.down,
                Direction.Right => Vector2.right,
                Direction.Up => Vector2.up,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
            _gravityHandler.SwitchLocalGravity(direction);
            _rig.velocity = jumpVector * _jumpForce;
            _playerSounds.Jump(fromGround);
            SetIsSideAxisHeld();
        }

        private bool ForbidInAirTurning()
        {
            return
                _directionWasChangedInJump ||
                _playerWallCollisions.IsGrounded ||
                IsDistanceToAnyFloorForbidsChange;
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

                if (Input.GetAxisRaw("Horizontal") != 0 && Vector2.Dot(_rig.velocity, Vector2.left) == 0)
                {
                    _directionWasChangedInJump = true;

                    if (IsInputHorizontalNegative)
                    {
                        Jump(Direction.Left, false);
                    }
                    else if (IsInputHorizontalPositive)
                    {
                        Jump(Direction.Right, false);
                    }
                }
                else if (Input.GetAxisRaw("Vertical") != 0 && Vector2.Dot(_rig.velocity, Vector2.up) == 0)
                {
                    _directionWasChangedInJump = true;

                    if (IsInputVerticalNegative)
                    {
                        Jump(Direction.Down, false); 
                    }
                    else if (IsInputVerticalPositive)
                    {
                        Jump(Direction.Up, false);
                    }
                }
            }
        }
    }
}