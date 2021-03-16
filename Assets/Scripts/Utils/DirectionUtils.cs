using System;
using Resources;
using UnityEngine;

namespace Utils
{
    public static class DirectionUtils
    {
        public static GameObject GetFloorFor(Direction gravityDirection)
        {
            switch (gravityDirection)
            {
                case Direction.Down:
                    return GameObject.FindGameObjectWithTag(Tags.BottomWall);
                case Direction.Left:
                    return GameObject.FindGameObjectWithTag(Tags.LeftWall);
                case Direction.Up:
                    return GameObject.FindGameObjectWithTag(Tags.UpperWall);
                case Direction.Right:
                    return GameObject.FindGameObjectWithTag(Tags.RightWall);
                default:
                    throw new ArgumentOutOfRangeException(nameof(gravityDirection), gravityDirection, null);
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
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Left;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gravityVector), gravityVector, null);
            }
        }
    }
}