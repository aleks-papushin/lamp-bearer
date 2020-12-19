using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _gravity;
        [SerializeField] private ObjectWallCollisions _wallCollisions;

        private Rigidbody2D _rig;

        // Start is called before the first frame update
        void Start()
        {
            _rig = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
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
