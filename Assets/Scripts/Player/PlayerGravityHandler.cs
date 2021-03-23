using System;
using UnityEngine;

namespace Player
{
    public class PlayerGravityHandler : MonoBehaviour
    {
        public Direction GravityVector { get; private set; }

        
        public bool IsGravityVectorVertical => GravityVector == Direction.Down || GravityVector == Direction.Up;
        public bool IsGravityVectorHorizontal => GravityVector == Direction.Left || GravityVector == Direction.Right;

        public void SwitchLocalGravity(Direction direction)
        {
            GravityVector = direction;
        }
    }
}
