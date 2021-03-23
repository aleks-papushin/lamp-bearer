using Resources;
using UnityEngine;

namespace Player
{
    public class PlayerWallCollisions : MonoBehaviour
    {
        private enum Wall 
        { 
            Down, 
            Left, 
            Up, 
            Right,
            None
        }
        
        private Wall _wallDirection;
        
        public bool IsTouchBottomWall => _wallDirection == Wall.Down;

        public bool IsTouchUpperWall => _wallDirection == Wall.Up;

        public bool IsTouchLeftWall => _wallDirection == Wall.Left;

        public bool IsTouchRightWall => _wallDirection == Wall.Right;

        public bool IsTouchHorizontalWall => IsTouchBottomWall || IsTouchUpperWall;
        public bool IsTouchVerticalWall => IsTouchLeftWall || IsTouchRightWall;

        public bool IsGrounded => _wallDirection != Wall.None;

        private void OnCollisionEnter2D(Collision2D otherCollider)
        {
            if (!otherCollider.gameObject.tag.Contains(Tags.WallSuffix)) return;
            _wallDirection = otherCollider.gameObject.tag switch
            {
                Tags.LeftWall => Wall.Left,
                Tags.RightWall => Wall.Right,
                Tags.BottomWall => Wall.Down,
                Tags.UpperWall => Wall.Up,
                _ => _wallDirection
            };
        }

        private void OnCollisionExit2D(Collision2D otherCollider)
        {
            if (!otherCollider.gameObject.tag.Contains(Tags.WallSuffix)) return;
            _wallDirection = Wall.None;
        }
    }
}
