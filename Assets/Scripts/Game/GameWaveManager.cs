﻿using Assets.Scripts.Game;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class GameWaveManager
    {
        private readonly List<GameWaveDto> _waveList;
        private readonly int _maxWaveNumber;

        public int CurrentNumber { get; set; }
        public bool IsCurrentWaveLast => CurrentNumber >= _maxWaveNumber;

        public GameWaveDto CurrentWave => _waveList[CurrentNumber];

        public GameWaveManager()
        {
            _waveList = new GameWaveCsvReader().ReadGameData();
            CurrentNumber = _waveList.Min(w => w.number);
            _maxWaveNumber = _waveList.Max(w => w.number);
        }

        public bool TryIncrement()
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
