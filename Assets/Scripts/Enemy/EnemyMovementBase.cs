using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public abstract class EnemyMovementBase : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private ObjectWallCollisions _wallCollisions;

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public bool IsDirectionPositive { get; set; } = true;

        private Rigidbody2D _rig;

        void Awake()
        {
            _rig = GetComponent<Rigidbody2D>();
        }

        public void ChangeDirection() => IsDirectionPositive = !IsDirectionPositive;

        protected void Move()
        {
            if (!_wallCollisions.IsGrounded) return;

            var directionMod = IsDirectionPositive ? 1 : -1;

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
