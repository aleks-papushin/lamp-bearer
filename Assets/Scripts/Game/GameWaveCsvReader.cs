using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Enums;
using Resources;

namespace Game
{
    public class GameWaveCsvReader
    {
        public List<GameWaveDto> ReadGameData(GameDifficulty difficulty)
        {
            var waveFilePath = difficulty == GameDifficulty.Easy
                ? Paths.WaveEasyModeFilePath
                : Paths.WaveHardModeFilePath;

            using (var file = new StreamReader(waveFilePath))
            {
                _ = file.ReadLine(); // feed line with column names
                string wave;
                var gameWaves = new List<GameWaveDto>();

                while ((wave = file.ReadLine()) != null)
                {
                    gameWaves.Add(ParseWave(wave));
                }

                return gameWaves;
            }
        }

        private static GameWaveDto ParseWave(string waveString)
        {
            var wave = waveString.Split(',');

            return new GameWaveDto
            {
                number = int.Parse(wave[0]),
                wallWarningInterval = float.Parse(wave[1], CultureInfo.InvariantCulture),
                wallDangerousInterval = float.Parse(wave[2], CultureInfo.InvariantCulture),
                wallCoolDownInterval = float.Parse(wave[3], CultureInfo.InvariantCulture),
                switchOnWalls = int.Parse(wave[4]) != 0,
                enemyCount = int.Parse(wave[5]),
                enemySpeed = float.Parse(wave[6], CultureInfo.InvariantCulture),
                isOilAffectLight = int.Parse(wave[7]) != 0,
                waveDuration = float.Parse(wave[8], CultureInfo.InvariantCulture)
            };
        }
    }
}
