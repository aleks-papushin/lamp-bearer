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

        private List<WallDanger> _walls;
        private GameWaveManager _waveManager;

        private void Awake()
        {
            _waveManager = FindObjectOfType<GameWaveManager>();
            _walls = FindObjectsOfType<WallDanger>().ToList();
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
            var dangerWallsCurrently = 0;

            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                var wallsToBeDangerous = _walls.Where(w => w.CanBeDangerous).ToList();
                if (wallsToBeDangerous.Count == 0)
                {
                    yield return new WaitForSeconds(1);
                    continue;
                }

                if (_waveManager.CurrentWave.dangerWallAmount == 0 ||
                    _waveManager.CurrentWave.dangerWallAmount <= dangerWallsCurrently)
                {
                    continue;
                }

                var wall = wallsToBeDangerous[new Random().Next(wallsToBeDangerous.Count)];
                dangerWallsCurrently++;
                StartCoroutine(wall.BecameDangerousCoroutine(
                    () => dangerWallsCurrently--,
                    _wallWarningInterval,
                    _wallDangerousInterval,
                    _wallCoolDownInterval));
            }
        }
    }
}

