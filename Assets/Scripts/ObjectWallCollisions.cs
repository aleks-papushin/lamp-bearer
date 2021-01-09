using Assets.Scripts.Interfaces;
using Assets.Scripts.Resources;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectWallCollisions : MonoBehaviour, IWallCollisions, IGroundedStateHandler
    {
        private int _bWallCollisionEntered = 0;
        private int _uWallCollisionEntered = 0;
        private int _lWallCollisionEntered = 0;
        private int _rWallCollisionEntered = 0;

        public bool IsTouchBottomWall
        {
            get
            {
                return _bWallCollisionEntered > 0;
            }
        }

        public bool IsTouchUpperWall
        {
            get
            {
                return _uWallCollisionEntered > 0;
            }
        }

        public bool IsTouchLeftWall
        {
            get
            {
                return _lWallCollisionEntered > 0;
            }
        }

        public bool IsTouchRightWall
        {
            get
            {
                return _rWallCollisionEntered > 0;
            }
        }

        public bool IsTouchHorizontalWall => IsTouchBottomWall || IsTouchUpperWall;
        public bool IsTouchVerticalWall => IsTouchLeftWall || IsTouchRightWall;
        
        public bool IsGrounded => IsTouchHorizontalWall || IsTouchVerticalWall;

        void OnCollisionEnter2D(Collision2D otherCollider)
        {
            this.HandleCollisionState(otherCollider, 1);
        }

        void OnCollisionExit2D(Collision2D otherCollider)
        {
            this.HandleCollisionState(otherCollider, -1);
        }

        protected void HandleCollisionState(Collision2D otherCollider, int collisionIncrement)
        {
            if (otherCollider.gameObject.tag.Contains(Tags.WallSuffix))
            {
                switch (otherCollider.gameObject.tag)
                {
                    case Tags.LeftWall:
                        _lWallCollisionEntered += collisionIncrement;
                        break;
                    case Tags.RightWall:
                        _rWallCollisionEntered += collisionIncrement;
                        break;
                    case Tags.BottomWall:
                        _bWallCollisionEntered += collisionIncrement;
                        break;
                    case Tags.UpperWall:
                        _uWallCollisionEntered += collisionIncrement;
                        break;
                }
            }
        }
    }
}
