using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerRunning : PlayerAction
    {
        private SpriteRenderer _sprite;
        private Animator _animator;

        public PlayerRunning(
            Rigidbody2D rigidbody, 
            PlayerCollisions collisions, 
            SpriteRenderer playerSprite, 
            Animator animator) : base(rigidbody, collisions)
        {
            _rig = rigidbody;
            _collisions = collisions;
            _sprite = playerSprite;
            _animator = animator;
        }

        public void HandleMovement(
            float speed,
            bool isGravityVectorVertical, 
            bool isTouchingHorizontalWall, 
            bool isTouchingVerticalWall)
        {          
            // TODO refactor conditions - extract method
            if (isTouchingHorizontalWall && isGravityVectorVertical)
            {
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    this.RigidbodySetVelocity(new Vector2(-1, 0), speed);
                    this.HandleSpriteFacing(Direction.Left);
                    _animator.SetFloat("Speed", speed);
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    this.RigidbodySetVelocity(new Vector2(1, 0), speed);
                    this.HandleSpriteFacing(Direction.Right);
                    _animator.SetFloat("Speed", speed);
                }
                else
                {
                    _animator.SetFloat("Speed", 0);
                }
            }
            else if (isTouchingVerticalWall && !isGravityVectorVertical)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    this.RigidbodySetVelocity(new Vector2(0, -1), speed);
                    this.HandleSpriteFacing(Direction.Up);
                    _animator.SetFloat("Speed", speed);
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    this.RigidbodySetVelocity(new Vector2(0, 1), speed);
                    this.HandleSpriteFacing(Direction.Down);
                    _animator.SetFloat("Speed", speed);
                }
                else
                {
                    _animator.SetFloat("Speed", 0);
                }
            }
        }

        private void RigidbodySetVelocity(Vector2 vector, float speed)
        {
            _rig.velocity = vector * speed;
        }

        private void HandleSpriteFacing(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                default:
                    if (_collisions.IsTouchingBottom)
                    {
                        _sprite.flipX = false;
                    }
                    else if (_collisions.IsTouchingUpperWall)
                    {
                        _sprite.flipX = true;
                    }
                    break;
                case Direction.Right:
                    if (_collisions.IsTouchingBottom)
                    {
                        _sprite.flipX = true;
                    }
                    else if (_collisions.IsTouchingUpperWall)
                    {
                        _sprite.flipX = false;
                    }
                    break;
                case Direction.Down:
                    if (_collisions.IsTouchingLeftWall)
                    {
                        _sprite.flipX = false;
                    }
                    else if (_collisions.IsTouchingRightWall)
                    {
                        _sprite.flipX = true;
                    }
                    break;
                case Direction.Up:
                    if (_collisions.IsTouchingLeftWall)
                    {
                        _sprite.flipX = true;
                    }
                    else if (_collisions.IsTouchingRightWall)
                    {
                        _sprite.flipX = false;
                    }
                    break;
            }
        }
    }
}
