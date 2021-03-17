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
        public GameObject _spawner;
        public int oilBottleCountForSpawn;
        private UserInterface _userInterface;

        public GameWaveManager WaveManager { get; private set; }
        public static List<GameObject> WallsToBeDangerous { get; set; } 
        private static List<GameObject> RestingWalls { get; set; } = new List<GameObject>();

        [SerializeField] private float _wallWarningInterval;
        [SerializeField] private float _wallDangerousInterval;
        [SerializeField] private float _wallCoolDownInterval;

        private void Awake()
        {
            WaveManager = FindObjectOfType<GameWaveManager>();
            WallsToBeDangerous = FindObjectsOfType<WallDanger>().Select(w => w.gameObject).ToList();
        }

        private void Start()
        {
            _userInterface = FindObjectOfType<UserInterface>();
            GameTimer_OnWaveIncrementing();
            StartCoroutine(HandleWallsDangerousness());
            _spawner.GetComponent<SpawnOil>().Spawn(oilBottleCountForSpawn, 0, 0);
            GameTimer.OnWaveIncrementing += GameTimer_OnWaveIncrementing;
        }

        public void UpdateScore(int increment)
        {
            _userInterface.UpdateScore(increment);
        }

        private void GameTimer_OnWaveIncrementing()
        {
            _wallWarningInterval = WaveManager.CurrentWave.wallWarningInterval;
            _wallDangerousInterval = WaveManager.CurrentWave.wallDangerousInterval;
            _wallCoolDownInterval = WaveManager.CurrentWave.wallCoolDownInterval;
        }

        private IEnumerator HandleWallsDangerousness()
        {
            // every N sec pick random wall and make it danger

            while (true)
            {
                yield return null;

                while (WaveManager.CurrentWave.dangerWallAmount == 0)
                {
                    yield return new WaitForSeconds(1);
                }

                var dangerousInterval = _wallDangerousInterval + _wallWarningInterval;
                var wall = WallsToBeDangerous[new Random().Next(WallsToBeDangerous.Count())];                
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
            if (RestingWalls.Count == 0) return;

            WallsToBeDangerous.AddRange(RestingWalls);
            RestingWalls.Clear();
        }

        private void MoveWallToRestingWalls(GameObject wall)
        {
            WallsToBeDangerous.Remove(wall);
            RestingWalls.Add(wall);
        }
    }
}

