namespace Assets.Scripts.Interfaces
{
    public interface IWallCollisions
    {
        bool IsTouchBottomWall { get; set; }
        bool IsTouchUpperWall { get; set; }
        bool IsTouchLeftWall { get; set; }
        bool IsTouchRightWall { get; set; }
        bool IsTouchHorizontalWall { get; }
        bool IsTouchVerticalWall { get; }
    }
}
