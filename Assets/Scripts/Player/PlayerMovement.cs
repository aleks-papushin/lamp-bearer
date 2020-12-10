using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMovement
    {
        private Rigidbody2D _rig;
        private PlayerCollisions _collisions;
        private SpriteRenderer _sprite;

        public PlayerMovement(
            Rigidbody2D rigidbody, 
            PlayerCollisions playerCollisions, 
            SpriteRenderer playerSprite)
        {
            _rig = rigidbody;
            _collisions = playerCollisions;
            _sprite = playerSprite;
        }

        public void HandleMovement(
            float speed,
            bool isGravityVectorVertical, 
            bool isTouchingHorizontalWall, 
            bool isTouchingVerticalWall)
        {
            if (isTouchingHorizontalWall && isGravityVectorVertical)
            {
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    this.RigidbodySetVelocity(new Vector2(-1, 0), speed);
                    this.HandleSpriteFacing(Direction.Left);
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    this.RigidbodySetVelocity(new Vector2(1, 0), speed);
                    this.HandleSpriteFacing(Direction.Right);
                }
            }
            else if (isTouchingVerticalWall && !isGravityVectorVertical)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    this.RigidbodySetVelocity(new Vector2(0, -1), speed);
                    this.HandleSpriteFacing(Direction.Up);
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    this.RigidbodySetVelocity(new Vector2(0, 1), speed);
                    this.HandleSpriteFacing(Direction.Down);
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
