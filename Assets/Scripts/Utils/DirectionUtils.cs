using System;
using Resources;
using UnityEngine;

namespace Utils
{
    public static class DirectionUtils
    {
        public static GameObject GetFloorFor(Direction gravityDirection)
        {
            return gravityDirection switch
            {
                Direction.Down => GameObject.FindGameObjectWithTag(Tags.BottomWall),
                Direction.Left => GameObject.FindGameObjectWithTag(Tags.LeftWall),
                Direction.Up => GameObject.FindGameObjectWithTag(Tags.UpperWall),
                Direction.Right => GameObject.FindGameObjectWithTag(Tags.RightWall),
                _ => throw new ArgumentOutOfRangeException(nameof(gravityDirection), gravityDirection, null)
            };
        }
    }
}