using System.Collections;
using Player;
using UI;
using UnityEngine;
using Wall;
using Random = UnityEngine.Random;

namespace Spawn
{
    public class OilSpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject _oilBottle;

        private const float XRange = 5.5f;
        private const float YRange = 1.5f;

        private Score _score;

        public int CurrentScore { get; private set; }

        private void Start()
        {
            _score = FindObjectOfType<Score>();
            PlayerCollisions.OnOilBottleTaken += PlayerCollisions_OnOilBottleTaken;
            Spawn(0, 0);
        }

        public void UpdateScore()
        {
            CurrentScore++;
            _score.SetScore(CurrentScore);
        }

        private void SpawnBottlesInRamdomPlace()
        {
            var position = new Vector2(Random.Range(-XRange, XRange), Random.Range(-YRange, YRange));
            Spawn(position.x, position.y);
        }

        private void Spawn(float x, float y)
        {
            Instantiate(_oilBottle, new Vector3(x, y, 0), _oilBottle.transform.rotation);
        }

        private void PlayerCollisions_OnOilBottleTaken()
        {
            StartCoroutine(WaitPlayerGroundedAndSpawnOil());
        }

        // TODO change to event handler which will be check if there is no oil when player is grounded
        private IEnumerator WaitPlayerGroundedAndSpawnOil()
        {
            var playerWallCollisions = PlayerController.Player.GetComponent<ObjectWallCollisions>();

            while (!playerWallCollisions.IsGrounded)
            {
                yield return new WaitForSeconds(0.01f);
            }

            SpawnBottlesInRamdomPlace();
        }

        private void OnDestroy()
        {
            PlayerCollisions.OnOilBottleTaken -= PlayerCollisions_OnOilBottleTaken;
        }
    }
}