using System;
using UnityEngine;

public class HandleObjectFacing : MonoBehaviour
{
    private ObjectWallCollisions _collisions;

    private void Awake()
    {
        _collisions = GetComponent<ObjectWallCollisions>();
    }

    public void Handle(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                if (_collisions.IsTouchBottomWall && transform.localScale.x < 0)
                {
                    FlipScale();
                }
                else if (_collisions.IsTouchUpperWall && transform.localScale.x > 0)
                {
                    FlipScale();
                }

                break;
            case Direction.Right:
                if (_collisions.IsTouchBottomWall && transform.localScale.x > 0)
                {
                    FlipScale();
                }
                else if (_collisions.IsTouchUpperWall && transform.localScale.x < 0)
                {
                    FlipScale();
                }

                break;
            case Direction.Down:
                if (_collisions.IsTouchLeftWall && transform.localScale.x < 0)
                {
                    FlipScale();
                }
                else if (_collisions.IsTouchRightWall && transform.localScale.x > 0)
                {
                    FlipScale();
                }

                break;
            case Direction.Up:
                if (_collisions.IsTouchLeftWall && transform.localScale.x > 0)
                {
                    FlipScale();
                }
                else if (_collisions.IsTouchRightWall && transform.localScale.x < 0)
                {
                    FlipScale();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    public void Handle(bool positive)
    {
        if (positive && transform.localScale.x > 0 || !positive && transform.localScale.x < 0)
        {
            FlipScale();
        }
    }

    private void FlipScale()
    {
        var localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
    }
}