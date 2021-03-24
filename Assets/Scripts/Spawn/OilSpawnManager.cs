using System.Collections;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawn
{
    public class OilSpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject _oilBottle;

        private const float XRange = 5.5f;
        private const float YRange = 1.5f;
        
        private void Start()
        {
            Instantiate(_oilBottle, new Vector3(0, 0, 0), _oilBottle.transform.rotation);
        }

        private void SpawnBottle()
        {
            var position = new Vector2(Random.Range(-XRange, XRange), Random.Range(-YRange, YRange));
            Instantiate(_oilBottle, new Vector3(position.x, position.y, 0), _oilBottle.transform.rotation);
        }

        public void SpawnNext()
        {
            StartCoroutine(WaitPlayerGroundedAndSpawnOil());
        }

        // TODO change to event handler which will be check if there is no oil when player is grounded
        private IEnumerator WaitPlayerGroundedAndSpawnOil()
        {
            var playerWallCollisions = FindObjectOfType<PlayerWallCollisions>();

            while (!playerWallCollisions.IsGrounded)
            {
                yield return new WaitForSeconds(0.01f);
            }

            SpawnBottle();
        }
    }
}