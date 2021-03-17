using System.Linq;
using Resources;
using UnityEngine;
using Utils;
using Wall;

namespace Player
{
    public class HandleSpriteRotation : MonoBehaviour
    {
        [SerializeField] private float _startRotationDistance = 5f;
        [SerializeField] private float _defaultRotationSpeed = 1000f;
        private float _rotationSpeed;

        private PlayerGravityHandler _gravityHandler;
        private ObjectWallCollisions _groundedStateHandler;

        private void Awake()
        {
            _rotationSpeed = _defaultRotationSpeed;
            _groundedStateHandler = GetComponent<ObjectWallCollisions>();
            _gravityHandler = GetComponent<PlayerGravityHandler>();
        }

        private void FixedUpdate()
        {
            if (_groundedStateHandler.IsGrounded) return;

            RotateTowardTheWall();
        }

        private void RotateTowardTheWall()
        {
            var ground = DirectionUtils.GetFloorFor(_gravityHandler.GravityVector);
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
    }
}