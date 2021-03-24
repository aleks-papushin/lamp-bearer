using System.Linq;
using Resources;
using UnityEngine;
using Utils;

namespace Player
{
    public class HandleSpriteRotation : MonoBehaviour
    {
        [SerializeField] private float _startRotationDistance = 5f;
        [SerializeField] private float _defaultRotationSpeed = 1000f;
        private float _rotationSpeed;

        private Rigidbody2D _rig;
        private PlayerWallCollisions _groundedStateHandler;

        private void Awake()
        {
            _rotationSpeed = _defaultRotationSpeed;
            _groundedStateHandler = GetComponent<PlayerWallCollisions>();
            _rig = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_groundedStateHandler.IsGrounded) return;

            RotateTowardTheWall();
        }

        private void RotateTowardTheWall()
        {
            var ground = GetFloorFor(_rig.velocity);
            var distanceToGround = transform.GetDistanceTo(ground);
            if (distanceToGround < _startRotationDistance)
            {
                // 4 is magic number which is should be bigger than any GetDistanceTo(Ground)
                var modifier = 4 / distanceToGround;
                _rotationSpeed *= modifier;
            }
            else
            {
                _rotationSpeed = _defaultRotationSpeed;
            }

            var floorAnchor = ground.GetComponentsInChildren<Transform>()
                .Single(t => t.tag.Contains(Tags.AnchorSuffix));

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, floorAnchor.transform.rotation,
                    Time.deltaTime * _rotationSpeed);
        }
        
        private static GameObject GetFloorFor(Vector2 velocity)
        {
            if (velocity.x > 0) return GameObject.FindGameObjectWithTag(Tags.RightWall);
            if (velocity.x < 0) return GameObject.FindGameObjectWithTag(Tags.LeftWall);
            if (velocity.y < 0) return GameObject.FindGameObjectWithTag(Tags.BottomWall);
            if (velocity.y > 0) return GameObject.FindGameObjectWithTag(Tags.UpperWall);
            return null;
        }
    }
}