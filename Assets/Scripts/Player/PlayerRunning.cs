using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerRunning : PlayerAction
    {
        [SerializeField] private float _speed;

        private Animator _animator;
        private HandleObjectFacing _facing;
        private PlayerGravityHandler _gravityHandler;
        private PlayerWallCollisions _playerWallCollisions;

        public bool IsGrounded { get; set; }

        private void Start()
        {
            _rig = GetComponent<Rigidbody2D>();
            _collisions = GetComponent<PlayerCollisions>();
            _animator = GetComponent<Animator>();
            _facing = GetComponent<HandleObjectFacing>();
            _gravityHandler = GetComponent<PlayerGravityHandler>();
            _playerWallCollisions = GetComponent<PlayerWallCollisions>();
            PlayerWallCollisions.OnIsGroundedChanged += PlayerWallCollisions_OnIsGroundedChanged;
        }

        private void Update()
        {
            if (IsGrounded)
            {
                HandleMovement();
            }
        }

        public void HandleMovement()
        {          
            // TODO refactor conditions - extract method
            if (_playerWallCollisions.IsTouchHorizontalWall && _gravityHandler.IsGravityVectorVertical)
            {
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    this.RigidbodySetVelocity(new Vector2(-1, 0), _speed);
                    _facing.Handle(Direction.Left);
                    _animator.SetFloat("Speed", _speed);
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    this.RigidbodySetVelocity(new Vector2(1, 0), _speed);
                    _facing.Handle(Direction.Right);
                    _animator.SetFloat("Speed", _speed);
                }
                else
                {
                    _animator.SetFloat("Speed", 0);
                }
            }
            else if (_playerWallCollisions.IsTouchVerticalWall && !_gravityHandler.IsGravityVectorVertical)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    this.RigidbodySetVelocity(new Vector2(0, -1), _speed);
                    _facing.Handle(Direction.Up);
                    _animator.SetFloat("Speed", _speed);
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    this.RigidbodySetVelocity(new Vector2(0, 1), _speed);
                    _facing.Handle(Direction.Down);
                    _animator.SetFloat("Speed", _speed);
                }
                else
                {
                    _animator.SetFloat("Speed", 0);
                }
            }
        }

        private void RigidbodySetVelocity(Vector2 vector, float speed)
        {
            if (IsGrounded)
            {
                _rig.velocity = vector * speed;
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
