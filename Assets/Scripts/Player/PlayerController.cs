using Assets.Scripts.Enums;
using Assets.Scripts.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rig;
    private SpriteRenderer _sprite;
        
    private float _gravity = 9.8f;
    private Direction _gravityVector;

    [SerializeField]
    private float _movementSpeed;
    private bool _isSideAxisWasHeld = false;

    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _cornerJumpForce;
    [SerializeField]
    private float _cornerJumpModifier;
    private bool _isJumpAxisWasIdle = true;
    internal bool _isChangedDirectionInJump = false;
    [SerializeField]
    private float _forbidDirectionChangingDistance;

    private PlayerCollisions _playerCollisions;
    [SerializeField]
    private float _startRotationDistance;
    [SerializeField]
    private float _rotationSpeed;
    private float _rotSpeedAccelerated;
    [SerializeField]
    private float _rotationSpeedMod;

    public bool IsInputHorisontalNegative => Input.GetAxisRaw("Horizontal") < 0;
    public bool IsInputHorizontalPositive => Input.GetAxisRaw("Horizontal") > 0;
    public bool IsInputVerticalNegative => Input.GetAxisRaw("Vertical") < 0;
    public bool IsInputVerticalPositive => Input.GetAxisRaw("Vertical") > 0;

    public bool IsGravityVectorVertical => _gravityVector == Direction.Down || _gravityVector == Direction.Up;
    public bool IsGravityVectorHorizontal => _gravityVector == Direction.Left || _gravityVector == Direction.Right;

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
        _sprite = GetComponent<SpriteRenderer>();
        _playerCollisions = GetComponent<PlayerCollisions>();

        _rotSpeedAccelerated = _rotationSpeed;

        this.SwitchGravity(Direction.Down);
    }

    void FixedUpdate()
    {
        if (IsGrounded)
        {
            this.HandleMovement();
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
            var ground = this.GetFloorFor(_gravityVector);
            if (this.IsDistanceToObjectLessThan(_startRotationDistance, ground))
            {          
                this.RotateTowards(_gravityVector, accelerateRotation: true);
            }
            else
            {
                this.RotateTowards(this.OppositeTo(_gravityVector), accelerateRotation: false);
            }
        }
    }

    public void CornerJump()
    {
        switch (CurrentCorner)
        {
            case Corner.BottomLeft:
                if (IsTouchingHorizontalWall())
                {
                    PerformCornerJump(new Vector2(-_cornerJumpForce * _cornerJumpModifier, _cornerJumpForce), Direction.Left);
                }
                else if (IsTouchingVerticalWall())
                {
                    PerformCornerJump(new Vector2(_cornerJumpForce, -_cornerJumpForce * _cornerJumpModifier), Direction.Down);
                }
                break;
            case Corner.BottomRight:
                if (IsTouchingHorizontalWall())
                {
                    PerformCornerJump(new Vector2(_cornerJumpForce * _cornerJumpModifier, _cornerJumpForce), Direction.Right);
                }
                else if (IsTouchingVerticalWall())
                {
                    PerformCornerJump(new Vector2(-_cornerJumpForce, -_cornerJumpForce * _cornerJumpModifier), Direction.Down);
                }
                break;
            case Corner.UpperLeft:
                if (IsTouchingHorizontalWall())
                {
                    PerformCornerJump(new Vector2(-_cornerJumpForce * _cornerJumpModifier, -_cornerJumpForce), Direction.Left);
                }
                else if (IsTouchingVerticalWall())
                {
                    PerformCornerJump(new Vector2(_cornerJumpForce, _cornerJumpForce * _cornerJumpModifier), Direction.Up);
                }
                break;
            case Corner.UpperRight:
                if (IsTouchingHorizontalWall())
                {
                    PerformCornerJump(new Vector2(_cornerJumpForce * _cornerJumpModifier, -_cornerJumpForce), Direction.Right);
                }
                else if (IsTouchingVerticalWall())
                {
                    PerformCornerJump(new Vector2(-_cornerJumpForce, _cornerJumpForce * _cornerJumpModifier), Direction.Up);
                }
                break;
        }

        void PerformCornerJump(Vector2 force, Direction newGravity)
        {            
            this.StopRig();
            _isChangedDirectionInJump = true;
            _rig.AddForce(force, ForceMode2D.Impulse);
            this.SwitchGravity(newGravity);
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
            Debug.Log($"Accelerate rotation");
            Debug.Log($"Rot speed acc before mod: {_rotSpeedAccelerated}");
            _rotSpeedAccelerated *= _rotationSpeedMod;
            Debug.Log($"Rot speed acc after mod: {_rotSpeedAccelerated}");
        }

        if (!IsGrounded)
        {
            Debug.Log($"Rotation speed accelerated: {_rotSpeedAccelerated}");
        }        

        var floor = GetFloorFor(gravityVector);
        transform.rotation = 
            Quaternion.RotateTowards(transform.rotation, floor.transform.rotation, Time.deltaTime * _rotSpeedAccelerated);
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
        if (IsGravityVectorHorizontal)
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
        _rotSpeedAccelerated = _rotationSpeed;

        this.SetIsSideAxisHeld();
        this.StopRig();        
        this.FreezePerpendicularAxis(gravity);
        this.SwitchGravity(gravity);
        _rig.AddForce(jumpVector, ForceMode2D.Impulse);
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

    private void SwitchGravity(Direction gravity)
    {
        _gravityVector = gravity;

        switch (_gravityVector)
        {
            case Direction.Down:
            default:
                Physics2D.gravity = new Vector2(0, -_gravity);
                break;
            case Direction.Up:
                Physics2D.gravity = new Vector2(0, _gravity);
                break;
            case Direction.Left:
                Physics2D.gravity = new Vector2(-_gravity, 0);
                break;
            case Direction.Right:
                Physics2D.gravity = new Vector2(_gravity, 0);
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

            if (Input.GetAxisRaw("Horizontal") != 0 && IsGravityVectorVertical)
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
            else if (Input.GetAxisRaw("Vertical") != 0 && IsGravityVectorHorizontal)
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

    private void HandleMovement()
    {
        if (this.IsTouchingHorizontalWall() && IsGravityVectorVertical)
        {           
            if (Input.GetAxisRaw("Horizontal") < 0)
            {             
                this.RigidbodySetVelocity(new Vector2(-1, 0));
                this.HandleSpriteFacing(Direction.Left);
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                this.RigidbodySetVelocity(new Vector2(1, 0));
                this.HandleSpriteFacing(Direction.Right);
            }
        }
        else if (this.IsTouchingVerticalWall() && IsGravityVectorHorizontal)
        {            
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                this.RigidbodySetVelocity(new Vector2(0, -1));
                this.HandleSpriteFacing(Direction.Up);
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                this.RigidbodySetVelocity(new Vector2(0, 1));
                this.HandleSpriteFacing(Direction.Down);
            }
        }
    }

    private void HandleSpriteFacing(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
            default:
                if (_playerCollisions.IsTouchingBottom)
                {
                    _sprite.flipX = false;
                }
                else if (_playerCollisions.IsTouchingUpperWall)
                {
                    _sprite.flipX = true;
                }
                break;
            case Direction.Right:
                if (_playerCollisions.IsTouchingBottom)
                {
                    _sprite.flipX = true;
                }
                else if (_playerCollisions.IsTouchingUpperWall)
                {
                    _sprite.flipX = false;
                }
                break;
            case Direction.Down:
                if (_playerCollisions.IsTouchingLeftWall)
                {
                    _sprite.flipX = false;
                }
                else if (_playerCollisions.IsTouchingRightWall)
                {
                    _sprite.flipX = true;
                }
                break;
            case Direction.Up:
                if (_playerCollisions.IsTouchingLeftWall)
                {
                    _sprite.flipX = true;
                }
                else if (_playerCollisions.IsTouchingRightWall)
                {
                    _sprite.flipX = false;
                }
                break;
        }
    }

    private void RigidbodySetVelocity(Vector2 vector)
    {
        _rig.velocity = vector * _movementSpeed;
    }

    private bool IsTouchingHorizontalWall()
    {
        return _playerCollisions.IsTouchingBottom || _playerCollisions.IsTouchingUpperWall;
    }

    private bool IsTouchingVerticalWall()
    {
        return _playerCollisions.IsTouchingLeftWall || _playerCollisions.IsTouchingRightWall;
    }

    private void StopRig()
    {
        _rig.velocity = Vector2.zero;
    }    
}
