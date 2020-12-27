using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerRunning : PlayerAction
    {
        private readonly Animator _animator;
        private readonly HandleObjectFacing _facing;

        public PlayerRunning(
            Rigidbody2D rigidbody, 
            PlayerCollisions collisions, 
            HandleObjectFacing facing, 
            Animator animator) : base(rigidbody, collisions)
        {
            _rig = rigidbody;
            _collisions = collisions;
            _facing = facing;
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
                    _facing.Handle(Direction.Left);
                    _animator.SetFloat("Speed", speed);
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    this.RigidbodySetVelocity(new Vector2(1, 0), speed);
                    _facing.Handle(Direction.Right);
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
                    _facing.Handle(Direction.Up);
                    _animator.SetFloat("Speed", speed);
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    this.RigidbodySetVelocity(new Vector2(0, 1), speed);
                    _facing.Handle(Direction.Down);
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
    }
}
