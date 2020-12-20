using Assets.Scripts.Enums;
using Assets.Scripts.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerRunning _playerMovement;
        [SerializeField] private float _movementSpeed;

        [SerializeField] private Animator _animator;

        private Rigidbody2D _rig;    
        
        private bool _isSideAxisWasHeld = false;

        [SerializeField] private float _jumpForce;
        private bool _isJumpAxisWasIdle = true;
        internal bool _isChangedDirectionInJump = false;
        [SerializeField] private float _forbidDirectionChangingDistance;

        private PlayerCollisions _playerCollisions;
        [SerializeField] private float _startRotationDistance;
        [SerializeField] private float _defaultRotationSpeed;
        private float _rotationSpeed;
        [SerializeField] private float _rotationSpeedMod;

        [SerializeField] private PlayerSounds _playerSounds;
        [SerializeField] private PlayerGravityHandler _gravity;
        [SerializeField] private HandlingSpriteFacing _spriteFacing;

        public bool IsInputHorisontalNegative => Input.GetAxisRaw("Horizontal") < 0;
        public bool IsInputHorizontalPositive => Input.GetAxisRaw("Horizontal") > 0;
        public bool IsInputVerticalNegative => Input.GetAxisRaw("Vertical") < 0;
        public bool IsInputVerticalPositive => Input.GetAxisRaw("Vertical") > 0;
        public bool IsTouchingHorizontalWall => _playerCollisions.IsTouchingBottom || _playerCollisions.IsTouchingUpperWall;
        public bool IsTouchingVerticalWall => _playerCollisions.IsTouchingLeftWall || _playerCollisions.IsTouchingRightWall;

        public GameObject Ground => this.GetFloorFor(_gravity.GravityVector);

        public bool IsGrounded => _playerCollisions.IsGrounded;

        public Corner CurrentCorner { get; internal set; }

        public List<GameObject> Floors
        { 
            get
            {
                List<GameObject> floors = new List<GameObject>();

                foreach (var dir in (Direction[]) Enum.GetValues(typeof(Direction)))
                {
                    floors.Add(GetFloorFor(dir));
                }

                return floors;
            }
        }

        void Start()
        {
            _rig = GetComponent<Rigidbody2D>();
            _playerCollisions = GetComponent<PlayerCollisions>();
            _playerCollisions.SetAnimator(_animator);
            _rotationSpeed = _defaultRotationSpeed;
            _gravity.SwitchGravity(Direction.Down);

            _playerMovement = new PlayerRunning(_rig, _playerCollisions, _spriteFacing, _animator);
        }

        void FixedUpdate()
        {
            if (IsGrounded)
            {
                _playerMovement.HandleMovement(
                    _movementSpeed, _gravity.IsGravityVectorVertical, IsTouchingHorizontalWall, IsTouchingVerticalWall);
            }

            this.HandleJumping();
            this.HandleInAirDirectionChanging();
            this.HandlePlayerRotation();
        }

        public void UnfreezeRig()
        {
            _rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void HandlePlayerRotation()
        {
            if (!IsGrounded)
            {
                var ground = this.GetFloorFor(_gravity.GravityVector);
                if (this.IsDistanceToObjectLessThan(_startRotationDistance, ground))
                {          
                    this.RotateTowards(_gravity.GravityVector, accelerateRotation: true);
                }
                else
                {
                    this.RotateTowards(this.OppositeTo(_gravity.GravityVector), accelerateRotation: false);
                }
            }
        }

        private Direction OppositeTo(Direction gravityVector)
        {
            switch (gravityVector)
            {
                case Direction.Down:            
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Up:
                default:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Left;
            }
        }

        private void RotateTowards(Direction gravityVector, bool accelerateRotation)
        {
            if (accelerateRotation)
            {
                // 4 is magic number which is should be bigger than any GetDistanceTo(Ground)
                var modifier = 4 / GetDistanceTo(Ground); 
                _rotationSpeed *= modifier;
            }     

            var floor = GetFloorFor(gravityVector);
            transform.rotation = 
                Quaternion.RotateTowards(transform.rotation, floor.transform.rotation, Time.deltaTime * _rotationSpeed);
        }

        private bool IsDistanceToObjectLessThan(float distance, GameObject obj) => this.GetDistanceTo(obj) < distance;

        private float GetDistanceTo(GameObject obj)
        {
            var closestPoint = obj.GetComponent<Collider2D>()
                .ClosestPoint(transform.position);
            var distanceToClosestPoint = Vector2.Distance(transform.position, closestPoint);
            return distanceToClosestPoint;
        }

        private bool IsDistanceToAnyFloorLessThan(float distance)
        {
            return Floors.Any(f => IsDistanceToObjectLessThan(distance, f));
        }

        private GameObject GetFloorFor(Direction gravityDirection)
        {
            switch (gravityDirection)
            {
                case Direction.Down:
                default:
                    return GameObject.FindGameObjectWithTag(TagNames.BottomWallTag);
                case Direction.Left:
                    return GameObject.FindGameObjectWithTag(TagNames.LeftWallTag);
                case Direction.Up:
                    return GameObject.FindGameObjectWithTag(TagNames.UpperWallTag);
                case Direction.Right:
                    return GameObject.FindGameObjectWithTag(TagNames.RightWallTag);
            }
        }
    
        private void HandleJumping()
        {
            // if on the surface, add impulse force in the opposite side
            if (_isJumpAxisWasIdle && Input.GetAxisRaw("Jump") > 0)
            {            
                if (_playerCollisions.IsTouchingBottom)
                {
                    this.Jump(Direction.Up, new Vector2(0, _jumpForce));
                }
                else if (_playerCollisions.IsTouchingUpperWall)
                {
                    this.Jump(Direction.Down, new Vector2(0, -_jumpForce));
                }
                else if (_playerCollisions.IsTouchingLeftWall)
                {
                    this.Jump(Direction.Right, new Vector2(_jumpForce, 0));
                }
                else if (_playerCollisions.IsTouchingRightWall)
                {
                    this.Jump(Direction.Left, new Vector2(-_jumpForce, 0));
                }

                _isJumpAxisWasIdle = false;
                _playerCollisions.IsGrounded = false;                
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
            if (_gravity.IsGravityVectorHorizontal)
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
            _rotationSpeed = _defaultRotationSpeed;

            this.SetIsSideAxisHeld();
            this.StopRig();        
            this.FreezePerpendicularAxis(gravity);
            _gravity.SwitchGravity(gravity);
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
                IsDistanceToAnyFloorLessThan(_forbidDirectionChangingDistance);
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

                if (Input.GetAxisRaw("Horizontal") != 0 && _gravity.IsGravityVectorVertical)
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
                else if (Input.GetAxisRaw("Vertical") != 0 && _gravity.IsGravityVectorHorizontal)
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

        private void StopRig()
        {
            _rig.velocity = Vector2.zero;
        }
    }
}

