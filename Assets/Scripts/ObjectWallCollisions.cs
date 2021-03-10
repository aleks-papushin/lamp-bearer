using Interfaces;
using Resources;
using UnityEngine;

public class ObjectWallCollisions : MonoBehaviour, IWallCollisions, IGroundedStateHandler
{
    private int _bWallCollisionEntered;
    private int _uWallCollisionEntered;
    private int _lWallCollisionEntered;
    private int _rWallCollisionEntered;
    
    public bool IsTouchBottomWall => _bWallCollisionEntered > 0;

    public bool IsTouchUpperWall => _uWallCollisionEntered > 0;

    public bool IsTouchLeftWall => _lWallCollisionEntered > 0;

    public bool IsTouchRightWall => _rWallCollisionEntered > 0;

    public bool IsTouchHorizontalWall => IsTouchBottomWall || IsTouchUpperWall;
    public bool IsTouchVerticalWall => IsTouchLeftWall || IsTouchRightWall;
        
    public bool IsGrounded => IsTouchHorizontalWall || IsTouchVerticalWall;

    private void OnCollisionEnter2D(Collision2D otherCollider)
    {
        HandleCollisionState(otherCollider, 1);
    }

    private void OnCollisionExit2D(Collision2D otherCollider)
    {
        HandleCollisionState(otherCollider, -1);
    }

    protected void HandleCollisionState(Collision2D otherCollider, int collisionIncrement)
    {
        if (!otherCollider.gameObject.tag.Contains(Tags.WallSuffix)) return;
        switch (otherCollider.gameObject.tag)
        {
            case Tags.LeftWall:
                _lWallCollisionEntered += collisionIncrement;
                break;
            case Tags.RightWall:
                _rWallCollisionEntered += collisionIncrement;
                break;
            case Tags.BottomWall:
                _bWallCollisionEntered += collisionIncrement;
                break;
            case Tags.UpperWall:
                _uWallCollisionEntered += collisionIncrement;
                break;
        }
    }
}