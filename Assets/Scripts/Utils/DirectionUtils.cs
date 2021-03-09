using System;
using Assets.Scripts.Resources;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class DirectionUtils
    {
        private static readonly GameObject bottomWall = GameObject.FindGameObjectWithTag(Tags.BottomWall);
        private static readonly GameObject leftWall = GameObject.FindGameObjectWithTag(Tags.LeftWall);
        private static readonly GameObject upWall = GameObject.FindGameObjectWithTag(Tags.UpperWall);
        private static readonly GameObject rightWall = GameObject.FindGameObjectWithTag(Tags.RightWall);

        public static GameObject GetFloorFor(Direction gravityDirection)
        {
            switch (gravityDirection)
            {
                case Direction.Down:
                    return bottomWall;
                case Direction.Left:
                    return leftWall;
                case Direction.Up:
                    return upWall;
                case Direction.Right:
                    return rightWall;
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