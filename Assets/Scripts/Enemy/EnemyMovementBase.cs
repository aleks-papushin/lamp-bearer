using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public abstract class EnemyMovementBase : MonoBehaviour, IMovement
    {
        public bool IsDirectionPositive { get; set; } = true;
        [SerializeField] private float _speed;
        [SerializeField] private ObjectWallCollisions _wallCollisions;
        private HandleObjectFacing _facing;
        private Rigidbody2D _rig;

        private void Start()
        {
            _facing = GetComponent<HandleObjectFacing>();
        }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        private void Awake()
        {
            _rig = GetComponent<Rigidbody2D>();
        }

        public void ChangeDirection() => IsDirectionPositive = !IsDirectionPositive;

        public void Move()
        {
            if (!_wallCollisions.IsGrounded) return;

            var directionMod = IsDirectionPositive ? 1 : -1;

            _facing.Handle(IsDirectionPositive);

            if (_wallCollisions.IsTouchBottomWall)
            {
                _rig.velocity = new Vector2(directionMod, 0) * _speed;
            }
            else if (_wallCollisions.IsTouchLeftWall)
            {
                _rig.velocity = new Vector2(0, -directionMod) * _speed;
            }
            else if (_wallCollisions.IsTouchUpperWall)
            {
                _rig.velocity = new Vector2(-directionMod, 0) * _speed;
            }
            else if (_wallCollisions.IsTouchRightWall)
            {
                _rig.velocity = new Vector2(0, directionMod) * _speed;
            }
        }
    }
}