using System;
using Resources;
using UnityEngine;
using Wall;

namespace Player
{
    public class JumpOverCorner : MonoBehaviour
    {
        private enum Corner
        {
            BottomLeft,
            BottomRight,
            UpperLeft,
            UpperRight
        }
    
        [SerializeField] private float _cornerJumpForce;
        [SerializeField] private float _cornerJumpModifier;

        private Rigidbody2D _rig;

        private PlayerWallCollisions _wallCollisions;
        private PlayerGravityHandler _gravitySwitcher;
        private PlayerSounds _sound;

        private void Awake()
        {
            _rig = GetComponent<Rigidbody2D>();
            _wallCollisions = GetComponent<PlayerWallCollisions>();
            _gravitySwitcher = GetComponent<PlayerGravityHandler>();
            _sound = GetComponent<PlayerSounds>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.tag.Contains(Tags.CornerSuffix)) return;
            switch (collision.gameObject.tag)
            {
                case Tags.BottomLeftCorner:
                    CornerJump(Corner.BottomLeft);
                    break;
                case Tags.BottomRightCorner:
                    CornerJump(Corner.BottomRight);
                    break;
                case Tags.UpperLeftCorner:
                    CornerJump(Corner.UpperLeft);
                    break;
                case Tags.UpperRightCorner:
                    CornerJump(Corner.UpperRight);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void CornerJump(Corner currentCorner)
        {
            switch (currentCorner)
            {
                case Corner.BottomLeft:
                    if (_wallCollisions.IsTouchHorizontalWall)
                    {
                        PerformCornerJump(
                            new Vector2(-_cornerJumpForce * _cornerJumpModifier, _cornerJumpForce), Direction.Left);
                    }
                    else if (_wallCollisions.IsTouchVerticalWall)
                    {
                        PerformCornerJump(
                            new Vector2(_cornerJumpForce, -_cornerJumpForce * _cornerJumpModifier), Direction.Down);
                    }
                    break;
                case Corner.BottomRight:
                    if (_wallCollisions.IsTouchHorizontalWall)
                    {
                        PerformCornerJump(
                            new Vector2(_cornerJumpForce * _cornerJumpModifier, _cornerJumpForce), Direction.Right);
                    }
                    else if (_wallCollisions.IsTouchVerticalWall)
                    {
                        PerformCornerJump(
                            new Vector2(-_cornerJumpForce, -_cornerJumpForce * _cornerJumpModifier), Direction.Down);
                    }
                    break;
                case Corner.UpperLeft:
                    if (_wallCollisions.IsTouchHorizontalWall)
                    {
                        PerformCornerJump(
                            new Vector2(-_cornerJumpForce * _cornerJumpModifier, -_cornerJumpForce), Direction.Left);
                    }
                    else if (_wallCollisions.IsTouchVerticalWall)
                    {
                        PerformCornerJump(
                            new Vector2(_cornerJumpForce, _cornerJumpForce * _cornerJumpModifier), Direction.Up);
                    }
                    break;
                case Corner.UpperRight:
                    if (_wallCollisions.IsTouchHorizontalWall)
                    {
                        PerformCornerJump(
                            new Vector2(_cornerJumpForce * _cornerJumpModifier, -_cornerJumpForce), Direction.Right);
                    }
                    else if (_wallCollisions.IsTouchVerticalWall)
                    {
                        PerformCornerJump(
                            new Vector2(-_cornerJumpForce, _cornerJumpForce * _cornerJumpModifier), Direction.Up);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentCorner), currentCorner, null);
            }
        }

        private void PerformCornerJump(Vector2 speed, Direction newGravity)
        {
            _rig.velocity = speed;
            _gravitySwitcher.SwitchLocalGravity(newGravity);
            _sound.CornerJump();
        }
    }
}

