using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Resources;
using UnityEngine;

namespace Assets.Scripts
{
    public class JumpOverCorner : MonoBehaviour
    {
        [SerializeField] private float _cornerJumpForce;
        [SerializeField] private float _cornerJumpModifier;

        private Rigidbody2D _rig;

        private IWallCollisions _wallCollisions;
        private IGravitySwitcher _gravitySwitcher;
        private ICornerJumpSoundSource _sound;

        public bool IsCornerReached { get; set; }

        void Awake()
        {
            _rig = GetComponent<Rigidbody2D>();
            _wallCollisions = GetComponent<IWallCollisions>();
            _gravitySwitcher = GetComponent<IGravitySwitcher>();
            _sound = GetComponent<ICornerJumpSoundSource>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Contains(TagNames.CornerTagSuffix))
            {
                IsCornerReached = true;
                var tag = collision.gameObject.tag;

                Corner currentCorner;
                switch (tag)
                {
                    case TagNames.BottomLeftCornerTag:
                    default:
                        currentCorner = Corner.BottomLeft;
                        break;
                    case TagNames.BottomRightCornerTag:
                        currentCorner = Corner.BottomRight;
                        break;
                    case TagNames.UpperLeftCornerTag:
                        currentCorner = Corner.UpperLeft;
                        break;
                    case TagNames.UpperRightCornerTag:
                        currentCorner = Corner.UpperRight;
                        break;
                }

                CornerJump(currentCorner);
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Contains(TagNames.CornerTagSuffix))
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
                        this.PerformCornerJump(
                            new Vector2(-_cornerJumpForce * _cornerJumpModifier, _cornerJumpForce), Direction.Left);
                    }
                    else if (_wallCollisions.IsTouchVerticalWall)
                    {
                        this.PerformCornerJump(
                            new Vector2(_cornerJumpForce, -_cornerJumpForce * _cornerJumpModifier), Direction.Down);
                    }
                    break;
                case Corner.BottomRight:
                    if (_wallCollisions.IsTouchHorizontalWall)
                    {
                        this.PerformCornerJump(
                            new Vector2(_cornerJumpForce * _cornerJumpModifier, _cornerJumpForce), Direction.Right);
                    }
                    else if (_wallCollisions.IsTouchVerticalWall)
                    {
                        this.PerformCornerJump(
                            new Vector2(-_cornerJumpForce, -_cornerJumpForce * _cornerJumpModifier), Direction.Down);
                    }
                    break;
                case Corner.UpperLeft:
                    if (_wallCollisions.IsTouchHorizontalWall)
                    {
                        this.PerformCornerJump(
                            new Vector2(-_cornerJumpForce * _cornerJumpModifier, -_cornerJumpForce), Direction.Left);
                    }
                    else if (_wallCollisions.IsTouchVerticalWall)
                    {
                        this.PerformCornerJump(
                            new Vector2(_cornerJumpForce, _cornerJumpForce * _cornerJumpModifier), Direction.Up);
                    }
                    break;
                case Corner.UpperRight:
                    if (_wallCollisions.IsTouchHorizontalWall)
                    {
                        this.PerformCornerJump(
                            new Vector2(_cornerJumpForce * _cornerJumpModifier, -_cornerJumpForce), Direction.Right);
                    }
                    else if (_wallCollisions.IsTouchVerticalWall)
                    {
                        this.PerformCornerJump(
                            new Vector2(-_cornerJumpForce, _cornerJumpForce * _cornerJumpModifier), Direction.Up);
                    }
                    break;
            }
        }

        private void PerformCornerJump(Vector2 force, Direction newGravity)
        {
            _rig.velocity = Vector2.zero;
            _rig.AddForce(force, ForceMode2D.Impulse);
            _gravitySwitcher.SwitchGravity(newGravity);
            _sound?.CornerJump();
        }
    }
}
