using System.Collections.Generic;
using System.Linq;
using Enums;

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
            _waveList = new GameWaveCsvReader().ReadGameData(GameDifficulty);
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
    }
}
