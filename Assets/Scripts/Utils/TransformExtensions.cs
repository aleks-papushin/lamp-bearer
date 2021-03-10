using UnityEngine;

namespace Utils
{
    public static class TransformExtensions
    {
        public static float GetDistanceTo(this Transform transform, GameObject obj)
        {
            var position = transform.position;
            var closestPoint = obj.GetComponent<Collider2D>()
                .ClosestPoint(position);
            var distanceToClosestPoint = Vector2.Distance(position, closestPoint);
            return distanceToClosestPoint;
        }
    }
}
