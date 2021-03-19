using System;
using UnityEngine;

namespace Player
{
    public class PlayerGravityHandler : MonoBehaviour
    {
        private readonly float _gravity = Physics.gravity.magnitude;
        public Direction GravityVector { get; private set; }

        
        public bool IsGravityVectorVertical => GravityVector == Direction.Down || GravityVector == Direction.Up;
        public bool IsGravityVectorHorizontal => GravityVector == Direction.Left || GravityVector == Direction.Right;

        public void SwitchLocalGravity(Direction direction)
        {
            GravityVector = direction;

            Physics2D.gravity = GravityVector switch
            {
                Direction.Down => new Vector2(0, -_gravity),
                Direction.Up => new Vector2(0, _gravity),
                Direction.Left => new Vector2(-_gravity, 0),
                Direction.Right => new Vector2(_gravity, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
