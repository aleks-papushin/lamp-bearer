using UnityEngine;

namespace Player
{
    public class PlayerRunning : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Animator _animator;
        private HandleObjectFacing _facing;
        private PlayerGravityHandler _gravityHandler;
        private PlayerWallCollisions _playerWallCollisions;
        private Rigidbody2D _rig;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Start()
        {
            _rig = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _facing = GetComponent<HandleObjectFacing>();
            _gravityHandler = GetComponent<PlayerGravityHandler>();
            _playerWallCollisions = GetComponent<PlayerWallCollisions>();
        }

        private void Update()
        {
            if (_playerWallCollisions.IsGrounded)
            {
                HandleMovement();
            }
        }

        private void HandleMovement()
        {          
            // TODO refactor conditions - extract method
            if (_playerWallCollisions.IsTouchHorizontalWall && _gravityHandler.IsGravityVectorVertical)
            {
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    RigidbodySetVelocity(new Vector2(-1, 0), _speed);
                    _facing.Handle(Direction.Left);
                    _animator.SetFloat(Speed, _speed);
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    RigidbodySetVelocity(new Vector2(1, 0), _speed);
                    _facing.Handle(Direction.Right);
                    _animator.SetFloat(Speed, _speed);
                }
                else
                {
                    _animator.SetFloat(Speed, 0);
                }
            }
            else if (_playerWallCollisions.IsTouchVerticalWall && !_gravityHandler.IsGravityVectorVertical)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    RigidbodySetVelocity(new Vector2(0, -1), _speed);
                    _facing.Handle(Direction.Up);
                    _animator.SetFloat(Speed, _speed);
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    RigidbodySetVelocity(new Vector2(0, 1), _speed);
                    _facing.Handle(Direction.Down);
                    _animator.SetFloat(Speed, _speed);
                }
                else
                {
                    _animator.SetFloat(Speed, 0);
                }
            }
        }

        private void RigidbodySetVelocity(Vector2 vector, float speed)
        {
            if (_playerWallCollisions.IsGrounded)
            {
                _rig.velocity = vector * speed;
            }
        }
    }
}
