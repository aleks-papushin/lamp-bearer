using Assets.Scripts.Resources;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool directionAlreadyChangedInJump = false;

    private bool _isSideAxisWasHeld = false;

    private float _gravity = 9.8f;
    private GravityDirection _gravityVector;
    private Rigidbody2D _rig;
    [SerializeField]
    private float _movementForce;
    [SerializeField]
    private float _initialAccelerationForce;
    private float _currentAccelerationForce;
    [SerializeField]
    private float _accelerationForceReductionModifier;
    [SerializeField]
    private float _jumpForce;
    private bool _isJumpAxisWasIdle = true;

    private PlayerCollisions _playerCollisions;
    [SerializeField]
    private float _startRotatingAt = 2f;
    [SerializeField]
    private float _rotationSpeed = 10f;

    public bool IsInputHorisontalNegative => Input.GetAxis("Horizontal") < 0;
    public bool IsInputHorizontalPositive => Input.GetAxis("Horizontal") > 0;
    public bool IsInputVerticalNegative => Input.GetAxis("Vertical") < 0;
    public bool IsInputVerticalPositive => Input.GetAxis("Vertical") > 0;

    public bool IsGravityVectorVertical => _gravityVector == GravityDirection.Down || _gravityVector == GravityDirection.Up;
    public bool IsGravityVectorHorizontal => _gravityVector == GravityDirection.Left || _gravityVector == GravityDirection.Right;

    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        _playerCollisions = GetComponent<PlayerCollisions>();

        this.SwitchGravity(GravityDirection.Down);
    }

    void FixedUpdate()
    {
        this.HandleMovement();        
        this.HandleJumping();
        this.HandleInAirDirectionChanging();
        this.HandlePlayerRotationInAir();
    }
    public void UnfreezeRig()
    {
        _rig.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void HandlePlayerRotationInAir()
    {
        if (!IsGrounded())
        {
            if (this.AmIFarFromGround())
            {          
                this.RotateTowards(_gravityVector);
            }
            else
            {
                this.RotateTowards(this.OppositeTo(_gravityVector));
            }
        }
    }

    private GravityDirection OppositeTo(GravityDirection gravityVector)
    {
        switch (gravityVector)
        {
            case GravityDirection.Down:            
                return GravityDirection.Up;
            case GravityDirection.Left:
                return GravityDirection.Right;
            case GravityDirection.Up:
            default:
                return GravityDirection.Down;
            case GravityDirection.Right:
                return GravityDirection.Left;
        }
    }

    private void RotateTowards(GravityDirection gravityVector)
    {
        var floor = GetFloorFor(gravityVector);
        transform.rotation = 
            Quaternion.RotateTowards(transform.rotation, floor.transform.rotation, Time.deltaTime * _rotationSpeed);
    }

    private bool AmIFarFromGround()
    {
        // ground is floor which is _gravityVector pointed to
        var ground = this.GetFloorFor(_gravityVector);

        // find nearest point on ground
        var closestPoint = ground.GetComponent<Collider2D>()
            .ClosestPoint(transform.position);

        // are we close enough?
        var distanceToClosestGroundPoint = Vector2.Distance(transform.position, closestPoint);
        var isCloseToGround = distanceToClosestGroundPoint < _startRotatingAt;

        return isCloseToGround;
    }

    private GameObject GetFloorFor(GravityDirection gravityDirection)
    {
        switch (gravityDirection)
        {
            case GravityDirection.Down:
            default:
                return GameObject.FindGameObjectWithTag(TagNames.BottomWallTag);
            case GravityDirection.Left:
                return GameObject.FindGameObjectWithTag(TagNames.LeftWallTag);
            case GravityDirection.Up:
                return GameObject.FindGameObjectWithTag(TagNames.UpperWallTag);
            case GravityDirection.Right:
                return GameObject.FindGameObjectWithTag(TagNames.RightWallTag);
        }
    }
    
    private void HandleJumping()
    {
        // if on the surface, add impulse force in the opposite side
        if (_isJumpAxisWasIdle && Input.GetAxis("Jump") > 0)
        {            
            if (_playerCollisions.IsTouchingBottom)
            {
                this.Jump(GravityDirection.Up, new Vector2(0, _jumpForce));
            }
            else if (_playerCollisions.IsTouchingUpperWall)
            {
                this.Jump(GravityDirection.Down, new Vector2(0, -_jumpForce));
            }
            else if (_playerCollisions.IsTouchingLeftWall)
            {
                this.Jump(GravityDirection.Right, new Vector2(_jumpForce, 0));
            }
            else if (_playerCollisions.IsTouchingRightWall)
            {
                this.Jump(GravityDirection.Left, new Vector2(-_jumpForce, 0));
            }

            _isJumpAxisWasIdle = false;
        }
        else if (Input.GetAxis("Jump") == 0)
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
            _isSideAxisWasHeld = Input.GetAxis("Vertical") != 0;
        }
        else
        {
            _isSideAxisWasHeld = Input.GetAxis("Horizontal") != 0;
        }
    }

    private void Jump(GravityDirection gravity, Vector2 jumpVector)
    {        
        this.SetIsSideAxisHeld();
        this.StopRig();        
        this.FreezePerpendicularAxis(gravity);
        this.SwitchGravity(gravity);
        _rig.AddForce(jumpVector, ForceMode2D.Impulse);
    }

    // This method was added because of slight side movement
    // observed in case if player pressed jump and side movement buttons simultaneously
    private void FreezePerpendicularAxis(GravityDirection gravity)
    {
        switch (gravity)
        {
            case GravityDirection.Down:
            case GravityDirection.Up:
                _rig.constraints = RigidbodyConstraints2D.FreezePositionX;
                break;
            case GravityDirection.Left:
            case GravityDirection.Right:
                _rig.constraints = RigidbodyConstraints2D.FreezePositionY;
                break;
        }
    }

    private void SwitchGravity(GravityDirection gravity)
    {
        _gravityVector = gravity;

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
            if (!directionAlreadyChangedInJump && !IsGrounded())
            {
                if (Input.GetAxis("Horizontal") != 0 && IsGravityVectorVertical)
                {
                    directionAlreadyChangedInJump = true;

                    if (IsInputHorisontalNegative)
                    {
                        this.Jump(GravityDirection.Left, new Vector2(-_jumpForce, 0));
                    }
                    else if (IsInputHorizontalPositive)
                    {
                        this.Jump(GravityDirection.Right, new Vector2(_jumpForce, 0));
                    }
                }
                else if (Input.GetAxis("Vertical") != 0 && IsGravityVectorHorizontal)
                {
                    directionAlreadyChangedInJump = true;

                    if (IsInputVerticalNegative)
                    {
                        this.Jump(GravityDirection.Down, new Vector2(0, -_jumpForce));
                    }
                    else if (IsInputVerticalPositive)
                    {
                        this.Jump(GravityDirection.Up, new Vector2(0, _jumpForce));
                    }
                }
            }
        }
    }

    private bool IsGrounded()
    {
        return 
            _playerCollisions.IsTouchingBottom ||
            _playerCollisions.IsTouchingUpperWall ||
            _playerCollisions.IsTouchingLeftWall ||
            _playerCollisions.IsTouchingRightWall;
    }

    private void HandleMovement()
    {
        var totalMovementForce = _movementForce + _currentAccelerationForce;

        if (this.IsTouchingHorizontalWall() && IsGravityVectorVertical)
        {
            CorrectCurrentAccelerationForce("Horizontal");

            if (Input.GetAxis("Horizontal") < 0)
            {
                _rig.AddForce(new Vector2(-totalMovementForce, 0));
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                _rig.AddForce(new Vector2(totalMovementForce, 0));
            }
        }
        else if (this.IsTouchingVerticalWall() && IsGravityVectorHorizontal)
        {
            CorrectCurrentAccelerationForce("Vertical");

            if (Input.GetAxis("Vertical") < 0)
            {
                _rig.AddForce(new Vector2(0, -totalMovementForce));
            }
            else if (Input.GetAxis("Vertical") > 0)
            {
                _rig.AddForce(new Vector2(0, totalMovementForce));
            }
        }

        void CorrectCurrentAccelerationForce(string axisName)
        {
            if (_currentAccelerationForce > 0)
            {
                _currentAccelerationForce -= _accelerationForceReductionModifier;
            }

            if (Input.GetAxis(axisName) == 0)
            {
                _currentAccelerationForce = _initialAccelerationForce;
            }
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
