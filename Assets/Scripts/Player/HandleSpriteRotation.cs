﻿using System.Linq;
using Resources;
using UnityEngine;
using Utils;
using Wall;

namespace Player
{
    public class HandleSpriteRotation : MonoBehaviour
    {
        [SerializeField] private float _startRotationDistance;
        [SerializeField] private float _defaultRotationSpeed;
        private float _rotationSpeed;

        private GravityHandler _gravityHandler;
        private ObjectWallCollisions _groundedStateHandler;

        private GameObject Ground => DirectionUtils.GetFloorFor(_gravityHandler.GravityVector);

        private void Awake()
        {
            _rotationSpeed = _defaultRotationSpeed;
            _groundedStateHandler = GetComponent<ObjectWallCollisions>();
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
                .Single(t => t.tag.Contains(Tags.AnchorSuffix));

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, floorAnchor.transform.rotation, Time.deltaTime * _rotationSpeed);
        }
    }
}