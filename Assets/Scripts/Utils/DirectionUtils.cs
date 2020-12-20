using Assets.Scripts.Resources;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class DirectionUtils
    {
        public static GameObject GetFloorFor(Direction gravityDirection)
        {
            switch (gravityDirection)
            {
                case Direction.Down:
                default:
                    return GameObject.FindGameObjectWithTag(TagNames.BottomWallTag);
                case Direction.Left:
                    return GameObject.FindGameObjectWithTag(TagNames.LeftWallTag);
                case Direction.Up:
                    return GameObject.FindGameObjectWithTag(TagNames.UpperWallTag);
                case Direction.Right:
                    return GameObject.FindGameObjectWithTag(TagNames.RightWallTag);
            }
        }

        public static Direction OppositeTo(Direction gravityVector)
        {
            switch (gravityVector)
            {
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Up:
                default:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Left;
            }
        }
    }
}
