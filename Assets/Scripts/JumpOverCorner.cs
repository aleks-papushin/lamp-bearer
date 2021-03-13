using System;
using Enums;
using Interfaces;
using Resources;
using UnityEngine;

public class JumpOverCorner : MonoBehaviour
{
    [SerializeField] private float _cornerJumpForce;
    [SerializeField] private float _cornerJumpModifier;

    private Animator _animator;
    private Rigidbody2D _rig;

    private IWallCollisions _wallCollisions;
    private IGravitySwitcher _gravitySwitcher;
    private ICornerJumpSoundSource _sound;

    public bool IsCornerReached { get; private set; }

    private void Awake()
    {
        _rig = GetComponent<Rigidbody2D>();
        _wallCollisions = GetComponent<IWallCollisions>();
        _gravitySwitcher = GetComponent<IGravitySwitcher>();
        _sound = GetComponent<ICornerJumpSoundSource>();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.tag.Contains(Tags.CornerSuffix)) return;
        IsCornerReached = true;
        var objectTag = collision.gameObject.tag;

        Corner currentCorner;
        switch (objectTag)
        {
            case Tags.BottomLeftCorner:
                currentCorner = Corner.BottomLeft;
                break;
            case Tags.BottomRightCorner:
                currentCorner = Corner.BottomRight;
                break;
            case Tags.UpperLeftCorner:
                currentCorner = Corner.UpperLeft;
                break;
            case Tags.UpperRightCorner:
                currentCorner = Corner.UpperRight;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        CornerJump(currentCorner);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains(Tags.CornerSuffix))
        {
            IsCornerReached = false;
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

    private void PerformCornerJump(Vector2 force, Direction newGravity)
    {
        _rig.velocity = Vector2.zero;
        _rig.AddForce(force, ForceMode2D.Impulse);
        _gravitySwitcher.SwitchLocalGravity(newGravity);
        _sound?.CornerJump();
    }
}