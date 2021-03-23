using UnityEngine;

namespace Player
{
    public class PlayerRunning : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Animator _animator;
        private PlayerFacing _facing;
        private PlayerGravityHandler _gravityHandler;
        private PlayerWallCollisions _playerWallCollisions;
        private Rigidbody2D _rig;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Start()
        {
            _rig = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _facing = GetComponent<PlayerFacing>();
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
                    _rig.velocity = Vector2.left * _speed;
                    _facing.Handle(Direction.Left);
                    _animator.SetFloat(Speed, _speed);
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    _rig.velocity = Vector2.right * _speed;
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
                    _rig.velocity = Vector2.down* _speed;
                    _facing.Handle(Direction.Up);
                    _animator.SetFloat(Speed, _speed);
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    _rig.velocity = Vector2.up * _speed;
                    _facing.Handle(Direction.Down);
                    _animator.SetFloat(Speed, _speed);
                }
                else
                {
                    _animator.SetFloat(Speed, 0);
                }
            }
        }
    }
}
