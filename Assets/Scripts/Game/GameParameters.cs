using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class GameParameters
    {
        public GameWaveDto[] gameWaves;

        public static GameParameters GetFromJson(string jObject)
        {
            return JsonUtility.FromJson<GameParameters>(jObject);
        }
    }

    [Serializable]
    public class GameWaveDto
    {
        public int number;
        public float wallWarningInterval;
        public float wallDangerousInterval;
        public float wallCoolDownInterval;
    }
}
