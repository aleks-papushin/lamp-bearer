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
        private static readonly List<GameObject> _restingWalls = new List<GameObject>();
        private static List<GameObject> _wallsToBeDangerous;

        [SerializeField] private float _wallWarningInterval;
        [SerializeField] private float _wallDangerousInterval;
        [SerializeField] private float _wallCoolDownInterval;
        [SerializeField] private GameObject _spawner;
        [SerializeField] private int oilBottleCountForSpawn;

        private GameWaveManager _waveManager;
        private Score _score;

        public int CurrentScore { get; private set; }     

        private void Awake()
        {
            _waveManager = FindObjectOfType<GameWaveManager>();
            _wallsToBeDangerous = FindObjectsOfType<WallDanger>().Select(w => w.gameObject).ToList();
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

                while (_waveManager.CurrentWave.dangerWallAmount == 0)
                {
                    yield return new WaitForSeconds(1);
                }

                var dangerousInterval = _wallDangerousInterval + _wallWarningInterval;
                var wall = _wallsToBeDangerous[new Random().Next(_wallsToBeDangerous.Count)];                
                StartCoroutine(wall.GetComponent<WallDanger>().BecameDangerousCoroutine(_wallWarningInterval));

                yield return new WaitForSeconds(dangerousInterval);

                wall.GetComponent<WallDanger>().BecameSafe();
                MoveRestingWallsToDangerous();
                MoveWallToRestingWalls(wall);

                yield return new WaitForSeconds(_wallCoolDownInterval);
            }
        }

        private void MoveRestingWallsToDangerous()
        {
            if (_restingWalls.Count == 0) return;

            _wallsToBeDangerous.AddRange(_restingWalls);
            _restingWalls.Clear();
        }

        private void MoveWallToRestingWalls(GameObject wall)
        {
            _wallsToBeDangerous.Remove(wall);
            _restingWalls.Add(wall);
        }
    }
}

