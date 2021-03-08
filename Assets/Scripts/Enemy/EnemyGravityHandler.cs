using System;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyGravityHandler : GravityHandler
    {
        public Rigidbody2D _rig;

        private void Awake()
        {
            _rig = GetComponent<Rigidbody2D>();
            _rig.gravityScale = 0;
            SwitchLocalGravity(Direction.Down);
        }

        private void FixedUpdate()
        {
            ApplyGravity();
        }

        private void ApplyGravity()
        {
            switch (GravityVector)
            {
                case Direction.Up:
                    _rig.AddForce(new Vector2(0, _gravity), ForceMode2D.Force);
                    break;
                case Direction.Left:
                    _rig.AddForce(new Vector2(-_gravity, 0), ForceMode2D.Force);
                    break;
                case Direction.Right:
                    _rig.AddForce(new Vector2(_gravity, 0), ForceMode2D.Force);
                    break;
                case Direction.Down:
                    _rig.AddForce(new Vector2(0, -_gravity), ForceMode2D.Force);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void SwitchLocalGravity(Direction direction)
        {
            GravityVector = direction;
        }
    }
}
