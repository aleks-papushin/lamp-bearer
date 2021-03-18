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
        private int _dangerWallsCurrently;

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
            StartCoroutine(HandleWallsDangerousness());
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

        private IEnumerator HandleWallsDangerousness()
        {
            // every N sec pick random wall and make it danger

            while (true)
            {
                yield return null;

                MoveRestingWallsToDangerous();

                while (_waveManager.CurrentWave.dangerWallAmount == 0 || 
                    _waveManager.CurrentWave.dangerWallAmount <= _dangerWallsCurrently ||
                    _wallsToBeDangerous.Count == 0)
                {
                    MoveRestingWallsToDangerous();
                    yield return new WaitForSeconds(1);
                }

                var wall = _wallsToBeDangerous[new Random().Next(_wallsToBeDangerous.Count)];
                _dangerWallsCurrently++;
                wall.CanBeDangerous = false;
                StartCoroutine(wall.BecameDangerousCoroutine(
                    () => _dangerWallsCurrently--,
                    _wallWarningInterval,
                    _wallDangerousInterval,
                    _wallCoolDownInterval));                
                MoveWallToRestingWalls(wall);
            }
        }

        // TODO Possibly should be implemented as independent coroutine 
        // and removed from HandleWallsDangerousness()
        private void MoveRestingWallsToDangerous()
        {
            if (_restingWalls.Count == 0) return;

            var wallsToMove = _restingWalls.Where(w => w.CanBeDangerous).ToList();
            _restingWalls = _restingWalls.Except(wallsToMove).ToList();
            _wallsToBeDangerous.AddRange(wallsToMove);
        }

        private void MoveWallToRestingWalls(WallDanger wall)
        {
            _wallsToBeDangerous.Remove(wall);
            _restingWalls.Add(wall);
        }
    }
}

