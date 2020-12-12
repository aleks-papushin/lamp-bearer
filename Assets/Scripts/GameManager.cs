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
        public int oilBottleCount;

        private GameTimer _gameTimer;
        private GameWave _gameWave;        

        public List<GameObject> WallsToBeDangerous => 
            FindObjectsOfType<WallDanger>().Where(ws => !ws.IsPlayerStandsOnMe).Select(ws => ws.gameObject).ToList();

        public bool IsThereOilBottles => GameObject.FindGameObjectsWithTag(TagNames.OilBottle).Any();

        [SerializeField] private float _wallWarningInterval;
        [SerializeField] private float _wallDangerousInterval;
        [SerializeField] private float _wallCoolDownInterval;

        // Debugging variables
        public bool _switchOffWalls;
        public GameObject _player;        
        //

        // Start is called before the first frame update
        void Start()
        {
            _gameTimer = GetComponent<GameTimer>();
            _gameWave = GetComponent<GameWave>();

            StartCoroutine(this.HandleWallsDangerousness());
        }

        // Update is called once per frame
        void Update()
        {
            this.HandleOilSpawn();

            this.HandleWaveTraits();

            // Debug 
            this.RespawnPlayerIfHeDied();
            // 
        }

        private void HandleWaveTraits()
        {            
            if (_gameTimer.IsTimeToIncrementWave)
            {
                GameWaveDto wave = _gameWave.TryGetWaveTraits();

                if (wave == null) return;

                _wallWarningInterval = wave.wallWarningInterval;
                _wallDangerousInterval = wave.wallDangerousInterval;
                _wallCoolDownInterval = wave.wallCoolDownInterval;
            }
        }

        private void RespawnPlayerIfHeDied()
        {
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                Instantiate(_player, _player.transform.position, _player.transform.rotation);
            }
        }

        private void HandleOilSpawn()
        {
            if (!IsThereOilBottles)
            {
                _spawner.GetComponent<SpawnOil>().Spawn(oilBottleCount);
            }        
        }

        private IEnumerator HandleWallsDangerousness()
        {
            // every N sec pick random wall and make it danger

            while (true)
            {
                var dangerousInterval = _wallDangerousInterval + _wallWarningInterval;

                yield return new WaitForSeconds(_wallCoolDownInterval);

                if (!_switchOffWalls)
                {
                    var wallIdx = new System.Random().Next(WallsToBeDangerous.Count);
                    var wall = WallsToBeDangerous[wallIdx];
                
                    StartCoroutine(wall.GetComponent<WallDanger>().BecameDangerousCoroutine(_wallWarningInterval));

                    yield return new WaitForSeconds(dangerousInterval);

                    wall.GetComponent<WallDanger>().BecameSafe();
                }
            }
        }
    }
}

