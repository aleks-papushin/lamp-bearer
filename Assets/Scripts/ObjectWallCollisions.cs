using Assets.Scripts.Interfaces;
using Assets.Scripts.Resources;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectWallCollisions : MonoBehaviour, IWallCollisions, IGroundedStateHandler
    {
        public bool IsTouchBottomWall { get; set; }
        public bool IsTouchUpperWall { get; set; }
        public bool IsTouchLeftWall { get; set; }
        public bool IsTouchRightWall { get; set; }
        public bool IsTouchHorizontalWall => IsTouchBottomWall || IsTouchUpperWall;
        public bool IsTouchVerticalWall => IsTouchLeftWall || IsTouchRightWall;
        
        // TODO replace usings of this property to event handler model
        public bool IsGrounded => IsTouchHorizontalWall || IsTouchVerticalWall;

        void OnCollisionEnter2D(Collision2D otherCollider)
        {
            this.HandleCollisionState(otherCollider, true);
        }

        void OnCollisionExit2D(Collision2D otherCollider)
        {
            this.HandleCollisionState(otherCollider, false);
        }

        protected void HandleCollisionState(Collision2D otherCollider, bool isEntered)
        {
            if (otherCollider.gameObject.tag.Contains(Tags.WallSuffix))
            {
                switch (otherCollider.gameObject.tag)
                {
                    case Tags.LeftWall:
                        IsTouchLeftWall = isEntered;
                        break;
                    case Tags.RightWall:
                        IsTouchRightWall = isEntered;
                        break;
                    case Tags.BottomWall:
                        IsTouchBottomWall = isEntered;
                        break;
                    case Tags.UpperWall:
                        IsTouchUpperWall = isEntered;
                        break;
                }
            }
        }
    }
}
