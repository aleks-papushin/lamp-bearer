using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Difficulties;
using UnityEngine;

namespace Game
{
    public class GameWaveManager: MonoBehaviour
    {
        private List<GameWaveDto> _waveList;
        private int _maxWaveNumber;

        [SerializeField] private Difficulty[] _difficulties;
        
        private int CurrentNumber { get; set; }
        private bool IsCurrentWaveLast => CurrentNumber >= _maxWaveNumber;

        public static GameDifficulty GameDifficulty { get; set; }
        public GameWaveDto CurrentWave => _waveList[CurrentNumber];

        public void Awake()
        {
            _waveList = ReadGameData(GameDifficulty);
            CurrentNumber = _waveList.Min(w => w.number);
            _maxWaveNumber = _waveList.Max(w => w.number);
            

            GameTimer.OnWaveIncrementing += OnWaveIncrementing;
        }

        private void OnWaveIncrementing()
        {
            if (!IsCurrentWaveLast)
            {
                CurrentNumber++;
            }
        }

        private List<GameWaveDto> ReadGameData(GameDifficulty difficulty)
        {
            var difficultySettings = _difficulties[difficulty == GameDifficulty.Easy ? 0 : 1].Text;
            using (var file = new StringReader(difficultySettings))
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
