using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spawn;
using UI;
using UnityEngine;
using Wall;
using Random = System.Random;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float _wallWarningInterval;
        [SerializeField] private float _wallDangerousInterval;
        [SerializeField] private float _wallCoolDownInterval;
        [SerializeField] private GameObject _spawner;
        [SerializeField] private int oilBottleCountForSpawn;

        private List<WallDanger> _restingWalls = new List<WallDanger>();
        private List<WallDanger> _wallsToBeDangerous;
        private GameWaveManager _waveManager;
        private Score _score;

        public int CurrentScore { get; private set; }

        private void Awake()
        {
            _waveManager = FindObjectOfType<GameWaveManager>();
            _wallsToBeDangerous = FindObjectsOfType<WallDanger>().ToList();
        }

        private void Start()
        {
            _score = FindObjectOfType<Score>();
            GameTimer_OnWaveIncrementing();
            StartCoroutine(WallsDangerousnessCoroutine());
            StartCoroutine(MoveRestingWallsToDangerousCoroutine());
            _spawner.GetComponent<SpawnOil>().Spawn(oilBottleCountForSpawn, 0, 0);
            GameTimer.OnWaveIncrementing += GameTimer_OnWaveIncrementing;
        }

        public void UpdateScore()
        {
            CurrentScore++;
            _score.SetScore(CurrentScore);
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
                while (_waveManager.CurrentWave.dangerWallAmount == 0 || 
                    _waveManager.CurrentWave.dangerWallAmount <= dangerWallsCurrently ||
                    _wallsToBeDangerous.Count == 0)
                {
                    yield return new WaitForSeconds(1);
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

                // wait to not to make 2 walls dangerous at the same second
                yield return new WaitForSeconds(1);
            }
        }

        private IEnumerator MoveRestingWallsToDangerousCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                if (_restingWalls.Count == 0) continue;

                var wallsToMove = _restingWalls.Where(w => w.CanBeDangerous).ToList();
                _restingWalls = _restingWalls.Except(wallsToMove).ToList();
                _wallsToBeDangerous.AddRange(wallsToMove);
            }
        }
    }
}

