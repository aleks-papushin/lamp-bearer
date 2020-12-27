using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    public class HandleObjectFacing : MonoBehaviour
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
                    if (_collisions.IsTouchBottomWall && transform.localScale.x < 0)
                    {
                        this.FlipScale();
                    }
                    else if (_collisions.IsTouchUpperWall && transform.localScale.x > 0)
                    {
                        this.FlipScale();
                    }
                    break;
                case Direction.Right:
                    if (_collisions.IsTouchBottomWall && transform.localScale.x > 0)
                    {
                        this.FlipScale();
                    }
                    else if (_collisions.IsTouchUpperWall && transform.localScale.x < 0)
                    {
                        this.FlipScale();
                    }
                    break;
                case Direction.Down:
                    if (_collisions.IsTouchLeftWall && transform.localScale.x < 0)
                    {
                        this.FlipScale();
                    }
                    else if (_collisions.IsTouchRightWall && transform.localScale.x > 0)
                    {
                        this.FlipScale();
                    }
                    break;
                case Direction.Up:
                    if (_collisions.IsTouchLeftWall && transform.localScale.x > 0)
                    {
                        this.FlipScale();
                    }
                    else if (_collisions.IsTouchRightWall && transform.localScale.x < 0)
                    {
                        this.FlipScale();
                    }
                    break;
            }
        }

        public void Handle(bool positive)
        {
            if ((positive && transform.localScale.x > 0) ||                 
                (!positive && transform.localScale.x < 0))
            {
                FlipScale();
            }
        }

        private void FlipScale()
        {
            var newScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            transform.localScale = newScale;
        }


    }
}
