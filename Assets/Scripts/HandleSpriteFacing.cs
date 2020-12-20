using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    public class HandleSpriteFacing : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;

        private IWallCollisions _collisions;

        void Awake()
        {
            _collisions = GetComponent<IWallCollisions>();
        }

        public void Handle(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                default:
                    if (_collisions.IsTouchBottomWall)
                    {
                        _sprite.flipX = false;
                    }
                    else if (_collisions.IsTouchUpperWall)
                    {
                        _sprite.flipX = true;
                    }
                    break;
                case Direction.Right:
                    if (_collisions.IsTouchBottomWall)
                    {
                        _sprite.flipX = true;
                    }
                    else if (_collisions.IsTouchUpperWall)
                    {
                        _sprite.flipX = false;
                    }
                    break;
                case Direction.Down:
                    if (_collisions.IsTouchLeftWall)
                    {
                        _sprite.flipX = false;
                    }
                    else if (_collisions.IsTouchRightWall)
                    {
                        _sprite.flipX = true;
                    }
                    break;
                case Direction.Up:
                    if (_collisions.IsTouchLeftWall)
                    {
                        _sprite.flipX = true;
                    }
                    else if (_collisions.IsTouchRightWall)
                    {
                        _sprite.flipX = false;
                    }
                    break;
            }
        }
    }
}
