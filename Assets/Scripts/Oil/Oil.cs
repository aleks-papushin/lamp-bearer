using Game;
using Spawn;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Oil
{
    public class Oil : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _oilTaken;
        private OilSpawnManager _oilSpawner;
        private GameStatus _gameStatus;

        private void Start()
        {
            _oilSpawner = FindObjectOfType<OilSpawnManager>();
            _gameStatus = FindObjectOfType<GameStatus>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            AudioSource.PlayClipAtPoint(_oilTaken[Random.Range(0, _oilTaken.Length)], Camera.main.transform.position);
            _oilSpawner.SpawnNext();
            _gameStatus.AddToScore();
            Destroy(gameObject);
        }
    }
}