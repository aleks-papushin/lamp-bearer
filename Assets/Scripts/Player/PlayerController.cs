using Assets.Scripts.Resources;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _gravity = 9.8f;
    private GravityDirection _gravityVector;
    private MovementMode _movement;
    private Rigidbody2D _rig;
    [SerializeField]
    private int _movementForce;
    [SerializeField]
    private int _jumpForce;

    private PlayerCollisions _playerCollisions;

    private bool _directionAlreadyChangedInJump = false;
    private float _closeToGroundDistance = 2f;

    public bool IsInputHorisontalNegative => Input.GetAxis("Horizontal") < 0;
    public bool IsInputHorizontalPositive => Input.GetAxis("Horizontal") > 0;
    public bool IsInputVerticalNegative => Input.GetAxis("Vertical") < 0;
    public bool IsInputVerticalPositive => Input.GetAxis("Vertical") > 0;

    public bool IsGravityVectorVertical => _gravityVector == GravityDirection.Down || _gravityVector == GravityDirection.Up;
    public bool IsGravityVectorHorizontal => _gravityVector == GravityDirection.Left || _gravityVector == GravityDirection.Right;


    void Start()
    {
        _gravityVector = GravityDirection.Down;
        _movement = MovementMode.Simple;
        _rig = GetComponent<Rigidbody2D>();
        _playerCollisions = GetComponent<PlayerCollisions>();

        this.SwitchGravityAndHandleRelatedStuff();
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
                // rotate face towards gravity direction side
                Debug.Log("Far from ground");
                this.RotateTowards(_gravityVector);
            }
            // until the ground is near
            else
            {
                // when it is, rotate in opposite side fastly (not instant)
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
        // find target floor transform to correct transform z component accordingly        
        var floor = GetFloorFor(gravityVector);
        Debug.Log($"Need to be rotated towards {floor.tag}");

        // get difference between z component of transforms and correct
        Debug.Log($"Floor rotation z is: {floor.transform.rotation.z}");
        Debug.Log($"Transform rotation z is: {transform.rotation.z}");        
        var dif = transform.rotation.z - floor.transform.rotation.z;
        Debug.Log($"Difference is: {dif}");

        // slowly rotate in this direction
        transform.Rotate(0, 0, dif);
        Debug.Log($"Transform rotation z after rotation: {transform.rotation.z}");
    }

    private GameObject GetFloorFor(GravityDirection gravityVector)
    {
        switch (gravityVector)
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

    private bool AmIFarFromGround()
    {
        // ground is floor which is _gravityVector pointed to
        GameObject ground = GetFloor(_gravityVector);

        // find nearest point on ground
        var closestPoint = ground.GetComponent<Collider2D>()
            .ClosestPoint(transform.position);

        // are we close enough?
        return Vector2.Distance(transform.position, closestPoint) > _closeToGroundDistance;

        GameObject GetFloor(GravityDirection gravityDirection)
        {
            switch (gravityDirection)
            {
                case GravityDirection.Down:
                default:
                    return GameObject.FindGameObjectWithTag(TagNames.BottomWallTag);
                case GravityDirection.Left:
                    return GameObject.FindGameObjectWithTag(TagNames.BottomWallTag);
                case GravityDirection.Up:
                    return GameObject.FindGameObjectWithTag(TagNames.UpperWallTag);
                case GravityDirection.Right:
                    return GameObject.FindGameObjectWithTag(TagNames.RightWallTag);
            }
        }
    }

    private void HandleJumping()
    {
        // if on the surface, add impulse force in the opposite side
        if (Input.GetAxis("Jump") > 0)
        {
            if (_playerCollisions.IsTouchingBottom)
            {
                this.StopRig();
                _gravityVector = GravityDirection.Up;
                this.SwitchGravityAndHandleRelatedStuff();
                _rig.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);                
            }
            else if (_playerCollisions.IsTouchingUpperWall)
            {
                this.StopRig();
                _gravityVector = GravityDirection.Down;
                this.SwitchGravityAndHandleRelatedStuff();
                _rig.AddForce(new Vector2(0, -_jumpForce), ForceMode2D.Impulse);                
            }
            else if (_playerCollisions.IsTouchingLeftWall)
            {
                this.StopRig();
                _gravityVector = GravityDirection.Right;
                this.SwitchGravityAndHandleRelatedStuff();
                _rig.AddForce(new Vector2(_jumpForce, 0), ForceMode2D.Impulse);
            }
            else if (_playerCollisions.IsTouchingRightWall)
            {
                this.StopRig();
                _gravityVector = GravityDirection.Left;
                this.SwitchGravityAndHandleRelatedStuff();
                _rig.AddForce(new Vector2(-_jumpForce, 0), ForceMode2D.Impulse);
            }
        }        
    }

    private void HandleInAirDirectionChanging()
    {
        if (!_directionAlreadyChangedInJump && !IsGrounded())
        {
            if (Input.GetAxis("Horizontal") != 0 && IsGravityVectorVertical)
            {
                _directionAlreadyChangedInJump = true;

                if (IsInputHorisontalNegative)
                {
                    this.StopRig();
                    _gravityVector = GravityDirection.Left;
                    this.SwitchGravityAndHandleRelatedStuff();
                    _rig.AddForce(new Vector2(-_jumpForce, 0), ForceMode2D.Impulse);
                }
                else if (IsInputHorizontalPositive)
                {
                    this.StopRig();
                    _gravityVector = GravityDirection.Right;
                    this.SwitchGravityAndHandleRelatedStuff();
                    _rig.AddForce(new Vector2(_jumpForce, 0), ForceMode2D.Impulse);
                }
            }
            else if (Input.GetAxis("Vertical") != 0 && IsGravityVectorHorizontal)
            {
                _directionAlreadyChangedInJump = true;

                if (IsInputVerticalNegative)
                {
                    this.StopRig();
                    _gravityVector = GravityDirection.Down;
                    this.SwitchGravityAndHandleRelatedStuff();
                    _rig.AddForce(new Vector2(0, -_jumpForce), ForceMode2D.Impulse);
                }
                else if (IsInputVerticalPositive)
                {
                    this.StopRig();
                    _gravityVector = GravityDirection.Up;
                    this.SwitchGravityAndHandleRelatedStuff();
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

    private void SwitchGravityAndHandleRelatedStuff()
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

        //this.RefinePlayerRotation();
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
