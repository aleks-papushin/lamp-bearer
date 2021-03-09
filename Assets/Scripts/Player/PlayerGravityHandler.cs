using System;
using Enums;
using UnityEngine;

namespace Player
{
    public class PlayerGravityHandler : GravityHandler
    {
        public bool IsGravityVectorVertical => GravityVector == Direction.Down || GravityVector == Direction.Up;
        public bool IsGravityVectorHorizontal => GravityVector == Direction.Left || GravityVector == Direction.Right;

        public override void SwitchLocalGravity(Direction direction)
        {
            GravityVector = direction;

            switch (GravityVector)
            {
                case Direction.Down:
                    Physics2D.gravity = new Vector2(0, -_gravity);
                    break;
                case Direction.Up:
                    Physics2D.gravity = new Vector2(0, _gravity);
                    break;
                case Direction.Left:
                    Physics2D.gravity = new Vector2(-_gravity, 0);
                    break;
                case Direction.Right:
                    Physics2D.gravity = new Vector2(_gravity, 0);
                    break;
                default: 
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
