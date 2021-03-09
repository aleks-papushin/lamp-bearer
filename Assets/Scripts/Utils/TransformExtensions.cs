using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace Utils
{
    public static class TransformExtensions
    {
        private static IEnumerable<GameObject> Floors => (from dir in (Direction[]) Enum.GetValues(typeof(Direction)) select DirectionUtils.GetFloorFor(dir)).ToList();

        public static float GetDistanceTo(this Transform transform, GameObject obj)
        {
            var position = transform.position;
            var closestPoint = obj.GetComponent<Collider2D>()
                .ClosestPoint(position);
            var distanceToClosestPoint = Vector2.Distance(position, closestPoint);
            return distanceToClosestPoint;
        }

        public static bool DistanceToAnyFloorLessThan(this Transform transform, float distance)
        {
            return Floors.Any(f => transform.GetDistanceTo(f) < distance);
        }
    }
}
