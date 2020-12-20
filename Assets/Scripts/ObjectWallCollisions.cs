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
        public bool IsGrounded => IsTouchHorizontalWall || IsTouchVerticalWall;

        void OnCollisionEnter2D(Collision2D otherCollider)
        {
            switch (otherCollider.gameObject.tag)
            {
                case Tags.LeftWall:
                    IsTouchLeftWall = true;
                    break;
                case Tags.RightWall:
                    IsTouchRightWall = true;
                    break;
                case Tags.BottomWall:
                    IsTouchBottomWall = true;
                    break;
                case Tags.UpperWall:
                    IsTouchUpperWall = true;
                    break;
            }
        }

        void OnCollisionExit2D(Collision2D otherCollider)
        {
            switch (otherCollider.gameObject.tag)
            {
                case Tags.LeftWall:
                    IsTouchLeftWall = false;
                    break;
                case Tags.RightWall:
                    IsTouchRightWall = false;
                    break;
                case Tags.BottomWall:
                    IsTouchBottomWall = false;
                    break;
                case Tags.UpperWall:
                    IsTouchUpperWall = false;
                    break;
            }
        }
    }
}
