using System;

namespace Game
{
    [Serializable]
    public struct GameWaveDto
    {
        public int number;
        public float wallWarningInterval;
        public float wallDangerousInterval;
        public float wallCoolDownInterval;
        public int dangerWallAmount;
        public int enemyCount;
        public float enemySpeed;
        public bool isOilAffectLight;
        public float waveDuration;
    }
}
