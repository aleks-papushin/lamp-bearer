using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyGravityHandler : GravityHandler
    {
        public Rigidbody2D _rig;

        void Awake()
        {
            _rig = GetComponent<Rigidbody2D>();
            _rig.gravityScale = 0;
            SwitchGravity(Direction.Down);
        }

        void FixedUpdate()
        {
            this.ApplyGravity();
        }

        private void ApplyGravity()
        {
            switch (GravityVector)
            {
                case Direction.Down:
                default:
                    _rig.AddForce(new Vector2(0, -_gravity), ForceMode2D.Force);
                    break;
                case Direction.Up:
                    _rig.AddForce(new Vector2(0, _gravity), ForceMode2D.Force);
                    break;
                case Direction.Left:
                    _rig.AddForce(new Vector2(-_gravity, 0), ForceMode2D.Force);
                    break;
                case Direction.Right:
                    _rig.AddForce(new Vector2(_gravity, 0), ForceMode2D.Force);
                    break;
            }
        }

        public override void SwitchGravity(Direction direction)
        {
            GravityVector = direction;
        }
    }
}
