using Assets.Scripts.Interfaces;
using Assets.Scripts.Resources;
using Assets.Scripts.Utils;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class HandleSpriteRotation : MonoBehaviour
    {
        [SerializeField] private float _startRotationDistance;
        [SerializeField] private float _defaultRotationSpeed;
        private float _rotationSpeed;

        private GravityHandler _gravityHandler;
        private IGroundedStateHandler _groundedStateHandler;

        private GameObject Ground => DirectionUtils.GetFloorFor(_gravityHandler.GravityVector);

        private void Awake()
        {
            _rotationSpeed = _defaultRotationSpeed;
            _groundedStateHandler = GetComponent<IGroundedStateHandler>();
            _gravityHandler = GetComponent<GravityHandler>();
        }

        private void FixedUpdate()
        {
            Handle();
        }

        private void Handle()
        {
            if (_groundedStateHandler.IsGrounded) return;

            var ground = DirectionUtils.GetFloorFor(_gravityHandler.GravityVector);
            var distanceToGround = transform.GetDistanceTo(ground);
            if (distanceToGround < _startRotationDistance)
            {
                RotateTowards(_gravityHandler.GravityVector, true);
            }
            else
            {
                _rotationSpeed = _defaultRotationSpeed;
                RotateTowards(DirectionUtils.OppositeTo(_gravityHandler.GravityVector), false);
            }
        }

        private void RotateTowards(Direction gravityVector, bool accelerateRotation)
        {
            if (accelerateRotation)
            {
                // 4 is magic number which is should be bigger than any GetDistanceTo(Ground)
                var modifier = 4 / transform.GetDistanceTo(Ground);
                _rotationSpeed *= modifier;
            }

            var floorAnchor = DirectionUtils.GetFloorFor(gravityVector)
                .GetComponentsInChildren<Transform>()
                .SingleOrDefault(t => t.tag.Contains(Tags.AnchorSuffix));

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, floorAnchor.transform.rotation, Time.deltaTime * _rotationSpeed);
        }
    }
}

