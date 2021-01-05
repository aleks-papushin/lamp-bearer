using Assets.Scripts.Player;
using Assets.Scripts.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public GameObject _spawner;
        public int oilBottleCountForSpawn;

        private UserInterface _userInterface;

        public GameWaveManager WaveManager { get; set; }

        public List<GameObject> WallsToBeDangerous => 
            FindObjectsOfType<WallDanger>().Where(ws => !ws.IsPlayerStandsOnMe).Select(ws => ws.gameObject).ToList();

        public bool IsThereOilBottles => GameObject.FindGameObjectsWithTag(Tags.OilBottle).Any();

        [SerializeField] private float _wallWarningInterval;
        [SerializeField] private float _wallDangerousInterval;
        [SerializeField] private float _wallCoolDownInterval;

        private void Awake()
        {
            WaveManager = new GameWaveManager();
        }

        void Start()
        {
            _userInterface = FindObjectOfType<UserInterface>();
            this.GameTimer_OnWaveIncrementing();
            PlayerCollisions.OnPlayerDied += PlayerCollisions_OnPlayerDied;
            StartCoroutine(this.HandleWallsDangerousness());
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

                while (!WaveManager.CurrentWave.switchOnWalls)
                {
                    yield return new WaitForSeconds(1);
                }

                var dangerousInterval = _wallDangerousInterval + _wallWarningInterval;

                var wallIdx = new System.Random().Next(WallsToBeDangerous.Count);
                var wall = WallsToBeDangerous[wallIdx];
                
                StartCoroutine(wall.GetComponent<WallDanger>().BecameDangerousCoroutine(_wallWarningInterval));

                yield return new WaitForSeconds(dangerousInterval);

                wall.GetComponent<WallDanger>().BecameSafe();

                yield return new WaitForSeconds(_wallCoolDownInterval);
            }
        }

        private void PlayerCollisions_OnPlayerDied()
        {
            _userInterface.ResetScore();
        }
    }
}

