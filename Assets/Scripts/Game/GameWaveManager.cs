using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Enums;
using Resources;

namespace Game
{
    public class GameWaveManager
    {
        private readonly List<GameWaveDto> _waveList;
        private readonly int _maxWaveNumber;

        public static GameDifficulty GameDifficulty { get; set; }
        private int CurrentNumber { get; set; }
        private bool IsCurrentWaveLast => CurrentNumber >= _maxWaveNumber;

        public GameWaveDto CurrentWave => _waveList[CurrentNumber];

        public GameWaveManager()
        {
            _waveList = ReadGameData(GameDifficulty);
            CurrentNumber = _waveList.Min(w => w.number);
            _maxWaveNumber = _waveList.Max(w => w.number);

            GameTimer.OnWaveIncrementing += GameTimer_OnWaveIncrementing;
        }

        private void GameTimer_OnWaveIncrementing()
        {
            TryIncrement();
        }

        private void TryIncrement()
        {
            if (!IsCurrentWaveLast)
            {
                CurrentNumber++;
            }
        }
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
