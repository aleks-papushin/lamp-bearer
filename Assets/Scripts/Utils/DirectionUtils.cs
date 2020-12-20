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
                    return GameObject.FindGameObjectWithTag(Tags.BottomWall);
                case Direction.Left:
                    return GameObject.FindGameObjectWithTag(Tags.LeftWall);
                case Direction.Up:
                    return GameObject.FindGameObjectWithTag(Tags.UpperWall);
                case Direction.Right:
                    return GameObject.FindGameObjectWithTag(Tags.RightWall);
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
