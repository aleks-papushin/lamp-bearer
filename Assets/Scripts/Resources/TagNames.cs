namespace Assets.Scripts.Resources
{
    public static class TagNames
    {
        public const string PlayerTag = "Player";

        public const string WallTagSuffix = "Wall";
        public readonly static string BottomWallTag = $"Bottom{WallTagSuffix}";
        public readonly static string UpperWallTag = $"Upper{WallTagSuffix}";
        public readonly static string LeftWallTag = $"Left{WallTagSuffix}";
        public readonly static string RightWallTag = $"Right{WallTagSuffix}";

        public const string CornerTagSuffix = "Corner";
        public const string BottomLeftCornerTag = "BottomLeftCorner";
        public const string BottomRightCornerTag = "BottomRightCorner";
        public const string UpperLeftCornerTag = "UpperLeftCorner";
        public const string UpperRightCornerTag = "UpperRightCorner";

        public const string OilBottle = "OilBottle";
    }
}
