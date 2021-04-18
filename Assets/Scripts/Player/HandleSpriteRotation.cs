using System;
using System.Linq;
using Resources;
using UnityEngine;
using Utils;

namespace Player
{
    public class HandleSpriteRotation : MonoBehaviour
    {
        [SerializeField] private float _startRotationDistance;
        [SerializeField] private float _defaultRotationSpeed;
        private float _rotationSpeed;

        private PlayerGravityHandler _gravityHandler;
        private PlayerWallCollisions _groundedStateHandler;

        private void Awake()
        {
            _rotationSpeed = _defaultRotationSpeed;
            _groundedStateHandler = GetComponent<PlayerWallCollisions>();
            _gravityHandler = GetComponent<PlayerGravityHandler>();
        }

        private void Update()
        {
            if (_groundedStateHandler.IsGrounded) return;

            Rotate();
        }

        private void Rotate()
        {
            var ground = GetFloorFor(_gravityHandler.GravityVector);
            var distanceToGround = transform.GetDistanceTo(ground);
            Transform rotationAnchor;
            _rotationSpeed = _defaultRotationSpeed;

            if (distanceToGround < _startRotationDistance)
            {
                if (distanceToGround < _startRotationDistance * 0.5)
                {
                    _rotationSpeed *= 9999;
                }

                rotationAnchor = ground.GetComponentsInChildren<Transform>()
                    .Single(t => t.tag.Contains(Tags.AnchorSuffix));                
            }
            else
            {
                rotationAnchor = GetFloorFor(OppositeTo(_gravityHandler.GravityVector))
                    .GetComponentsInChildren<Transform>()
                    .Single(t => t.tag.Contains(Tags.AnchorSuffix));
            }

            transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, rotationAnchor.transform.rotation,
                        Time.deltaTime * _rotationSpeed);

        }
        
        private static GameObject GetFloorFor(Direction gravityDirection)
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

        private static Direction OppositeTo(Direction gravityVector)
        {
            return gravityVector switch
            {
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                Direction.Up => Direction.Down,
                _ => throw new NotImplementedException()
            };
        }
    }
}