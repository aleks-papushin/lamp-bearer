using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class TransformExtensions
    {
        private static List<GameObject> Floors
        {
            get
            {
                List<GameObject> floors = new List<GameObject>();

                foreach (var dir in (Direction[])Enum.GetValues(typeof(Direction)))
                {
                    floors.Add(DirectionUtils.GetFloorFor(dir));
                }

                return floors;
            }
        }

        public static float GetDistanceTo(this Transform transform, GameObject obj)
        {
            var closestPoint = obj.GetComponent<Collider2D>()
                .ClosestPoint(transform.position);
            var distanceToClosestPoint = Vector2.Distance(transform.position, closestPoint);
            return distanceToClosestPoint;
        }

        public static bool IsDistanceToAnyFloorLessThan(this Transform transform, float distance)
        {
            return Floors.Any(f => transform.GetDistanceTo(f) < distance);
        }
    }
}
