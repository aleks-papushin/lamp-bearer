using Assets.Scripts.Resources;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool directionAlreadyChangedInJump = false;

    private Rigidbody2D _rig;
        
    private float _gravity = 9.8f;
    private Direction _gravityVector;

    [SerializeField]
    private float _movementSpeed;
    private bool _isSideAxisWasHeld = false;

    [SerializeField]
    private float _jumpForce;
    private bool _isJumpAxisWasIdle = true;

    private PlayerCollisions _playerCollisions;
    [SerializeField]
    private float _startRotatingAt = 2f;
    [SerializeField]
    private float _rotationSpeed = 10f;

    public bool IsInputHorisontalNegative => Input.GetAxisRaw("Horizontal") < 0;
    public bool IsInputHorizontalPositive => Input.GetAxisRaw("Horizontal") > 0;
    public bool IsInputVerticalNegative => Input.GetAxisRaw("Vertical") < 0;
    public bool IsInputVerticalPositive => Input.GetAxisRaw("Vertical") > 0;

    public bool IsGravityVectorVertical => _gravityVector == Direction.Down || _gravityVector == Direction.Up;
    public bool IsGravityVectorHorizontal => _gravityVector == Direction.Left || _gravityVector == Direction.Right;

    public bool IsGrounded => _playerCollisions.IsGrounded;        

    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        _playerCollisions = GetComponent<PlayerCollisions>();

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
        this.HandlePlayerRotationInAir();
    }

    public void UnfreezeRig()
    {
        _rig.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void HandlePlayerRotationInAir()
    {
        if (!IsGrounded)
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

    private void RotateTowards(Direction gravityVector)
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
            if (!directionAlreadyChangedInJump && !IsGrounded)
            {
                if (Input.GetAxisRaw("Horizontal") != 0 && IsGravityVectorVertical)
                {
                    directionAlreadyChangedInJump = true;

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
                    directionAlreadyChangedInJump = true;

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
    }

    private void HandleMovement()
    {
        if (this.IsTouchingHorizontalWall() && IsGravityVectorVertical)
        {           
            if (Input.GetAxisRaw("Horizontal") < 0)
            {             
                this.RigidbodySetVelocity(new Vector2(-1, 0));
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                this.RigidbodySetVelocity(new Vector2(1, 0));
            }
        }
        else if (this.IsTouchingVerticalWall() && IsGravityVectorHorizontal)
        {            
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                this.RigidbodySetVelocity(new Vector2(0, -1));
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                this.RigidbodySetVelocity(new Vector2(0, 1));
            }
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
