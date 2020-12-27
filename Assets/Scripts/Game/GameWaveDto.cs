﻿using System;

namespace Assets.Scripts
{
    [Serializable]
    public class GameWaveDto
    {
        public int number;
        public float wallWarningInterval;
        public float wallDangerousInterval;
        public float wallCoolDownInterval;
        public bool switchOnWalls;
        public int enemyCount;
        public float enemySpeed;
    }
}
