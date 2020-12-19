using Assets.Scripts.Interfaces;
using Assets.Scripts.Resources;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectWallCollisions : MonoBehaviour, IWallCollisions
    {
        public bool IsTouchBottomWall { get; set; }
        public bool IsTouchUpperWall { get; set; }
        public bool IsTouchLeftWall { get; set; }
        public bool IsTouchRightWall { get; set; }

        void OnCollisionEnter2D(Collision2D otherCollider)
        {
            switch (otherCollider.gameObject.tag)
            {
                case TagNames.LeftWallTag:
                    IsTouchLeftWall = true;
                    break;
                case TagNames.RightWallTag:
                    IsTouchRightWall = true;
                    break;
                case TagNames.BottomWallTag:
                    IsTouchBottomWall = true;
                    break;
                case TagNames.UpperWallTag:
                    IsTouchUpperWall = true;
                    break;
            }
        }

        void OnCollisionExit2D(Collision2D otherCollider)
        {
            switch (otherCollider.gameObject.tag)
            {
                case TagNames.LeftWallTag:
                    IsTouchLeftWall = false;
                    break;
                case TagNames.RightWallTag:
                    IsTouchRightWall = false;
                    break;
                case TagNames.BottomWallTag:
                    IsTouchBottomWall = false;
                    break;
                case TagNames.UpperWallTag:
                    IsTouchUpperWall = false;
                    break;
            }
        }
    }
}
