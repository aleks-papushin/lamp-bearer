using Assets.Scripts.Resources;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool directionAlreadyChangedInJump = false;

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
        _gravityVector = GravityDirection.Down;
        _rig = GetComponent<Rigidbody2D>();
        _playerCollisions = GetComponent<PlayerCollisions>();

        this.SwitchGravity();
    }

    void FixedUpdate()
    {
        this.HandleMovement();
        this.HandleJumping();
        this.HandleInAirDirectionChanging();
        this.HandlePlayerRotationInAir();
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

    // TODO refactor this method
    private void HandleJumping()
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

    // TODO refactor together with HandleJumping
    private void HandleInAirDirectionChanging()
    {
        if (!directionAlreadyChangedInJump && !IsGrounded())
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
