using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wall;
using Random = System.Random;

namespace Game
{
    public class WallDangerManager : MonoBehaviour
    {
        [SerializeField] private float _wallWarningInterval;
        [SerializeField] private float _wallDangerousInterval;
        [SerializeField] private float _wallCoolDownInterval;

        private List<WallDanger> _restingWalls = new List<WallDanger>();
        private List<WallDanger> _wallsToBeDangerous;
        private GameWaveManager _waveManager;

        private void Awake()
        {
            _waveManager = FindObjectOfType<GameWaveManager>();
            _wallsToBeDangerous = FindObjectsOfType<WallDanger>().ToList();
        }

        private void Start()
        {
            GameTimer_OnWaveIncrementing();
            StartCoroutine(WallsDangerousnessCoroutine());
            GameTimer.OnWaveIncrementing += GameTimer_OnWaveIncrementing;
        }

        private void GameTimer_OnWaveIncrementing()
        {
            _wallWarningInterval = _waveManager.CurrentWave.wallWarningInterval;
            _wallDangerousInterval = _waveManager.CurrentWave.wallDangerousInterval;
            _wallCoolDownInterval = _waveManager.CurrentWave.wallCoolDownInterval;
        }

        private IEnumerator WallsDangerousnessCoroutine()
        {
            int dangerWallsCurrently = 0;

            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                if (_wallsToBeDangerous.Count == 0)
                {
                    yield return new WaitForSeconds(1);
                    continue;
                }

                MoveRestingWallsToDangerous();

                if (_waveManager.CurrentWave.dangerWallAmount == 0 ||
                    _waveManager.CurrentWave.dangerWallAmount <= dangerWallsCurrently)
                {
                    continue;
                }

                var wall = _wallsToBeDangerous[new Random().Next(_wallsToBeDangerous.Count)];
                dangerWallsCurrently++;
                StartCoroutine(wall.BecameDangerousCoroutine(
                    () => dangerWallsCurrently--,
                    _wallWarningInterval,
                    _wallDangerousInterval,
                    _wallCoolDownInterval));
                _wallsToBeDangerous.Remove(wall);
                _restingWalls.Add(wall);
            }
        }

        private void MoveRestingWallsToDangerous()
        {
            if (_restingWalls.Count == 0) return;

            var wallsToMove = _restingWalls.Where(w => w.CanBeDangerous).ToList();
            _restingWalls = _restingWalls.Except(wallsToMove).ToList();
            _wallsToBeDangerous.AddRange(wallsToMove);
        }
    }
}

