using Assets.Scripts.Enums;
using Assets.Scripts.Game;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class GameWaveManager
    {
        private readonly List<GameWaveDto> _waveList;
        private readonly int _maxWaveNumber;

        public static GameDifficulty GameDifficulty { get; set; }
        public int CurrentNumber { get; set; }
        public bool IsCurrentWaveLast => CurrentNumber >= _maxWaveNumber;

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
            this.TryIncrement();
        }

        private bool TryIncrement()
        {
            if (!IsCurrentWaveLast)
            {
                CurrentNumber++;
                return true;
            }
            else return false;
        }
    }
}
