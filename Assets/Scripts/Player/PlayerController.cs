﻿using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    // Debug variables:
    private bool infiniteInAir = false;
    private void SwitchInfiniteInAir()
    {
        infiniteInAir = !infiniteInAir;
    }
    //

    private float _gravity = 9.8f;
    private GravityDirection _gravityVector;
    private MovementMode _movement;
    private Rigidbody2D _rig;
    [SerializeField]
    private int _movementForce;
    [SerializeField]
    private int _jumpForce;

    private PlayerCollisions _playerCollisions;
    private SpriteRenderer _playerSprite;

    public bool directionAlreadyChangedInJump = false;

    public bool IsTouchingAnyUpperCorner =>
        this.IsTouchingCorner(CornerEnum.UpperLeft) || this.IsTouchingCorner(CornerEnum.UpperRight);
    public bool IsTouchingAnyBottomCorner =>
        this.IsTouchingCorner(CornerEnum.BottomLeft) || this.IsTouchingCorner(CornerEnum.BottomRight);
    public bool IsTouchingAnyLeftCorner =>
        this.IsTouchingCorner(CornerEnum.BottomLeft) || this.IsTouchingCorner(CornerEnum.BottomRight);
    public bool IsTouchingAnyRightCorner =>
        this.IsTouchingCorner(CornerEnum.BottomRight) || this.IsTouchingCorner(CornerEnum.UpperRight);

    public bool IsInputHorisontalNegative => Input.GetAxis("Horizontal") < 0;
    public bool IsInputHorizontalPositive => Input.GetAxis("Horizontal") > 0;
    public bool IsInputVerticalNegative => Input.GetAxis("Vertical") < 0;
    public bool IsInputVerticalPositive => Input.GetAxis("Vertical") > 0;

    public bool IsGravityVectorVertical => _gravityVector == GravityDirection.Down || _gravityVector == GravityDirection.Up;
    public bool IsGravityVectorHorizontal => _gravityVector == GravityDirection.Left || _gravityVector == GravityDirection.Right;


    void Start()
    {
        _playerSprite = GetComponent<SpriteRenderer>();

        _gravityVector = GravityDirection.Down;
        _movement = MovementMode.Simple;
        _rig = GetComponent<Rigidbody2D>();
        _playerCollisions = GetComponent<PlayerCollisions>();

        this.SwitchGravity();
    }

    void FixedUpdate()
    {
        this.HandleMovementUsual();
        this.HandleJump();
        this.HandleInAirDirectionChanging();

        // Debug code
        if (Input.GetKeyDown(KeyCode.I))
        {
            this.SwitchInfiniteInAir();
        }
        //
    }

    private void HandleJump()
    {
        // if on the surface, add impulse force in the opposite side
        if (Input.GetAxis("Jump") > 0)
        {
            if (_playerCollisions.IsTouchingBottom)
            {
                this.StopRig();
                _gravityVector = GravityDirection.Up;
                this.SwitchGravity();
                _rig.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);                
            }
            else if (_playerCollisions.IsTouchingUpperWall)
            {
                this.StopRig();
                _gravityVector = GravityDirection.Down;
                this.SwitchGravity();
                _rig.AddForce(new Vector2(0, -_jumpForce), ForceMode2D.Impulse);                
            }
            else if (_playerCollisions.IsTouchingLeftWall)
            {
                this.StopRig();
                _gravityVector = GravityDirection.Right;
                this.SwitchGravity();
                _rig.AddForce(new Vector2(_jumpForce, 0), ForceMode2D.Impulse);
            }
            else if (_playerCollisions.IsTouchingRightWall)
            {
                this.StopRig();
                _gravityVector = GravityDirection.Left;
                this.SwitchGravity();
                _rig.AddForce(new Vector2(-_jumpForce, 0), ForceMode2D.Impulse);
            }
        }        
    }

    private void HandleInAirDirectionChanging()
    {
        if (!directionAlreadyChangedInJump && !IsTouchingAnything())
        {
            if (Input.GetAxis("Horizontal") != 0 && IsGravityVectorVertical)
            {
                directionAlreadyChangedInJump = true;

                if (IsInputHorisontalNegative)
                {
                    this.StopRig();
                    _gravityVector = GravityDirection.Left;
                    this.SwitchGravity();
                    _rig.AddForce(new Vector2(-_jumpForce, 0), ForceMode2D.Impulse);
                }
                else if (IsInputHorizontalPositive)
                {
                    this.StopRig();
                    _gravityVector = GravityDirection.Right;
                    this.SwitchGravity();
                    _rig.AddForce(new Vector2(_jumpForce, 0), ForceMode2D.Impulse);
                }
            }
            else if (Input.GetAxis("Vertical") != 0 && IsGravityVectorHorizontal)
            {
                directionAlreadyChangedInJump = true;

                if (IsInputVerticalNegative)
                {
                    this.StopRig();
                    _gravityVector = GravityDirection.Down;
                    this.SwitchGravity();
                    _rig.AddForce(new Vector2(0, -_jumpForce), ForceMode2D.Impulse);
                }
                else if (IsInputVerticalPositive)
                {
                    this.StopRig();
                    _gravityVector = GravityDirection.Up;
                    this.SwitchGravity();
                    _rig.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
                }
            }
        }
    }

    private bool IsTouchingAnything()
    {
        return 
            _playerCollisions.IsTouchingBottom ||
            _playerCollisions.IsTouchingUpperWall ||
            _playerCollisions.IsTouchingLeftWall ||
            _playerCollisions.IsTouchingRightWall;
    }

    private void HandleMovementAlternate()
    {
        if (_playerCollisions.IsTouchingBottom)
        {
            if (IsInputHorisontalNegative)
            {
                _rig.AddForce(new Vector2(-_movementForce, 0));
            }
            else if (IsInputHorizontalPositive)
            {
                _rig.AddForce(new Vector2(_movementForce, 0));
            }
        }
        if (_playerCollisions.IsTouchingLeftWall)
        {
            if (IsInputHorisontalNegative)
            {
                _rig.AddForce(new Vector2(0, _movementForce));
            }
            else if (IsInputHorizontalPositive)
            {
                _rig.AddForce(new Vector2(0, -_movementForce));
            }
        }
        if (_playerCollisions.IsTouchingUpperWall)
        {
            if (IsInputHorisontalNegative)
            {
                _rig.AddForce(new Vector2(_movementForce, 0));
            }
            else if (IsInputHorizontalPositive)
            {
                _rig.AddForce(new Vector2(-_movementForce, 0));
            }
        }
        if (_playerCollisions.IsTouchingRightWall)
        {
            if (IsInputHorisontalNegative)
            {
                _rig.AddForce(new Vector2(0, -_movementForce));
            }
            else if (IsInputHorizontalPositive)
            {
                _rig.AddForce(new Vector2(0, _movementForce));
            }
        }
    }

    private void HandleMovementUsual()
    {
        if (this.IsTouchingHorizontalWall() && IsGravityVectorVertical)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                _rig.AddForce(new Vector2(-_movementForce, 0));
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                _rig.AddForce(new Vector2(_movementForce, 0));
            }
        }
        if (this.IsTouchingVerticalWall() && IsGravityVectorHorizontal)
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                _rig.AddForce(new Vector2(0, -_movementForce));
            }
            else if (Input.GetAxis("Vertical") > 0)
            {
                _rig.AddForce(new Vector2(0, _movementForce));
            }
        }        
    }

    private void HandleNearCornersMovement()
    {
        // handle direction changing on corners
        if (this.IsTouchingAnyUpperCorner && Input.GetAxis("Vertical") < 0)
        {
            _gravityVector = GravityDirection.Down;
        }
        else if (this.IsTouchingAnyBottomCorner && Input.GetAxis("Vertical") > 0)
        {
            _gravityVector = GravityDirection.Up;
        }
        else if (this.IsTouchingAnyRightCorner && Input.GetAxis("Horizontal") < 0)
        {
            _gravityVector = GravityDirection.Left;
        }
        else if (this.IsTouchingAnyLeftCorner && Input.GetAxis("Horizontal") > 0)
        {
            _gravityVector = GravityDirection.Right;
        }

        this.SwitchGravity();
    }

    private void HandleJumpingAlternate()
    {
        if (Input.GetAxis("Vertical") < 0)
        {
            _gravityVector = GravityDirection.Down;
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            _gravityVector = GravityDirection.Up;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            _gravityVector = GravityDirection.Left;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            _gravityVector = GravityDirection.Right;
        }

        this.SwitchGravity();
    }

    private void SwitchGravity()
    {
        switch (_gravityVector)
        {
            case GravityDirection.Down:
            default:
                Physics2D.gravity = new Vector2(0, -_gravity);
                break;
            case GravityDirection.Up:
                Physics2D.gravity = new Vector2(0, _gravity);
                break;
            case GravityDirection.Left:
                Physics2D.gravity = new Vector2(-_gravity, 0);
                break;
            case GravityDirection.Right:
                Physics2D.gravity = new Vector2(_gravity, 0);
                break;
        }

        this.RefinePlayerRotation();
    }

    private void RefinePlayerRotation()
    {
        switch (_gravityVector)
        {
            case GravityDirection.Down:
            default:
                transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, 0);                
                break;
            case GravityDirection.Up:
                transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, 180);
                break;
            case GravityDirection.Left:
                transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, -90);
                break;
            case GravityDirection.Right:
                transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, 90);
                break;
        }
    }

    private bool IsTouchingCorner(CornerEnum corner)
    {
        switch (corner)
        {
            case CornerEnum.BottomLeft:
                return _playerCollisions.IsTouchingBottomLeftCorner;
            case CornerEnum.UpperLeft:
                return _playerCollisions.IsTouchingUpperLeftCorner;
            case CornerEnum.UpperRight:
                return _playerCollisions.IsTouchingUpperRightCorner;
            case CornerEnum.BottomRight:
                return _playerCollisions.IsTouchingBottomRightCorner;
            default:
                return false;
        }
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
