using Resources;
using UnityEngine;

namespace Player
{
    public class PlayerWallCollisions : MonoBehaviour
    {
        private Direction _wallDirection;
        
        public bool IsTouchBottomWall => _wallDirection == Direction.Down;

        public bool IsTouchUpperWall => _wallDirection == Direction.Up;

        public bool IsTouchLeftWall => _wallDirection == Direction.Left;

        public bool IsTouchRightWall => _wallDirection == Direction.Right;

        public bool IsTouchHorizontalWall => IsTouchBottomWall || IsTouchUpperWall;
        public bool IsTouchVerticalWall => IsTouchLeftWall || IsTouchRightWall;
        
        public bool IsGrounded = true;

        private void OnCollisionEnter2D(Collision2D otherCollider)
        {
            if (!otherCollider.gameObject.tag.Contains(Tags.WallSuffix)) return;
            IsGrounded = true;
            _wallDirection = otherCollider.gameObject.tag switch
            {
                Tags.LeftWall => Direction.Left,
                Tags.RightWall => Direction.Right,
                Tags.BottomWall => Direction.Down,
                Tags.UpperWall => Direction.Up,
                _ => _wallDirection
            };
        }

        private void OnCollisionExit2D(Collision2D otherCollider)
        {
            if (!otherCollider.gameObject.tag.Contains(Tags.WallSuffix)) return;
            IsGrounded = false;
        }
    }
}
