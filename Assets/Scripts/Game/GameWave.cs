using Assets.Scripts.Game;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameWave : MonoBehaviour
    {
        [SerializeField] private int _currentWave;
        private bool _currentWaveIsLast = false;
        private List<GameWaveDto> _waveList;

        public int Current 
        { 
            get => _currentWave; 
            set => _currentWave = value; 
        }

        private void Start()
        {
            _waveList = new GameWaveCsvReader().ReadGameData();
        }

        public GameWaveDto TryGetWaveTraits()
        {
            if (!_currentWaveIsLast)
            {
                Current++;
                GameWaveDto waveTraits = _waveList.Where(w => w.number == Current).SingleOrDefault();

                if (waveTraits == null)
                {
                    _currentWaveIsLast = true;
                }

                return waveTraits;
            }
            return null;
        }
    }
}
