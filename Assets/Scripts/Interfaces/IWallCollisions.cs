namespace Assets.Scripts.Interfaces
{
    public interface IWallCollisions
    {
        bool IsTouchBottomWall { get; }
        bool IsTouchUpperWall { get; }
        bool IsTouchLeftWall { get; }
        bool IsTouchRightWall { get; }
        bool IsTouchHorizontalWall { get; }
        bool IsTouchVerticalWall { get; }
    }
}
