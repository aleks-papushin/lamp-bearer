using UnityEngine;
using Wall;

namespace Enemy
{
    public class EnemyWalkerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private ObjectWallCollisions _wallCollisions;
        private HandleObjectFacing _facing;
        private Rigidbody2D _rig;
        
        public bool IsDirectionPositive { get; set; } = true;
        
        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }
        
        private void Start()
        {
            _rig = GetComponent<Rigidbody2D>();
            _rig.gravityScale = 0;
            _facing = GetComponent<HandleObjectFacing>();
        }
        
        private void Update()
        {
            Move();
        }

        private void Move()
        {
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
