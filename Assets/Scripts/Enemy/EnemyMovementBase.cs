using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public abstract class EnemyMovementBase : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private ObjectWallCollisions _wallCollisions;

        private Rigidbody2D _rig;

        void Awake()
        {
            _rig = GetComponent<Rigidbody2D>();
        }

        protected void Move()
        {
            if (!_wallCollisions.IsGrounded) return;

            if (_wallCollisions.IsTouchBottomWall)
            {
                this.RigidbodySetVelocity(new Vector2(-1, 0), _speed);
            }
            else if (_wallCollisions.IsTouchLeftWall)
            {
                this.RigidbodySetVelocity(new Vector2(0, 1), _speed);
            }
            else if (_wallCollisions.IsTouchUpperWall)
            {
                this.RigidbodySetVelocity(new Vector2(1, 0), _speed);
            }
            else if (_wallCollisions.IsTouchRightWall)
            {
                this.RigidbodySetVelocity(new Vector2(0, -1), _speed);
            }
        }

        private void RigidbodySetVelocity(Vector2 vector, float speed)
        {
            _rig.velocity = vector * speed;
        }
    }
}
