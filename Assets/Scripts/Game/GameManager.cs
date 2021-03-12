﻿using System.Collections;
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
        public GameWaveManager WaveManager { get; private set; }

        private UserInterface _userInterface;
        private static WallDanger[] WallsToBeDangerous => FindObjectsOfType<WallDanger>();

        [SerializeField] private float _wallWarningInterval;
        [SerializeField] private float _wallDangerousInterval;
        [SerializeField] private float _wallCoolDownInterval;

        private void Awake()
        {
            WaveManager = new GameWaveManager();
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

                while (!WaveManager.CurrentWave.switchOnWalls)
                {
                    yield return new WaitForSeconds(1);
                }

                var dangerousInterval = _wallDangerousInterval + _wallWarningInterval;

                var wall = WallsToBeDangerous[new Random().Next(WallsToBeDangerous.Length)];
                
                StartCoroutine(wall.BecameDangerousCoroutine(_wallWarningInterval));

                yield return new WaitForSeconds(dangerousInterval);

                wall.BecameSafe();

                yield return new WaitForSeconds(_wallCoolDownInterval);
            }
        }
    }
}

