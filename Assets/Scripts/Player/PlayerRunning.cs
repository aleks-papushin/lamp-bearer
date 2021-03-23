using UnityEngine;

namespace Player
{
    public class PlayerRunning : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Animator _animator;
        private PlayerFacing _facing;
        private PlayerWallCollisions _playerWallCollisions;
        private Rigidbody2D _rig;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Start()
        {
            _rig = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _facing = GetComponent<PlayerFacing>();
            _playerWallCollisions = GetComponent<PlayerWallCollisions>();
        }

        private void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            // TODO refactor conditions - extract method
            var currentPosition = transform.position;
            var deltaPos = _speed * Time.smoothDeltaTime;
            if (_playerWallCollisions.IsTouchHorizontalWall)
            {
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    _rig.transform.position = new Vector2(currentPosition.x - deltaPos, currentPosition.y);
                    _facing.Handle(Direction.Left);
                    _animator.SetFloat(Speed, _speed);
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    _rig.transform.position = new Vector2(currentPosition.x + deltaPos, currentPosition.y);
                    _facing.Handle(Direction.Right);
                    _animator.SetFloat(Speed, _speed);
                }
                else
                {
                    _animator.SetFloat(Speed, 0);
                }
            }
            else if (_playerWallCollisions.IsTouchVerticalWall)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    _rig.transform.position = new Vector2(currentPosition.x, currentPosition.y - deltaPos);
                    _facing.Handle(Direction.Up);
                    _animator.SetFloat(Speed, _speed);
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    _rig.transform.position = new Vector2(currentPosition.x, currentPosition.y + deltaPos);
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