using System.Collections;
using Player;
using UnityEngine;

namespace Spawn
{
    public class SpawnOil : MonoBehaviour
    {
        public GameObject _oilBottle;

        private const float XRange = 6;
        private const float YRange = 2;

        private void Start()
        {
            PlayerCollisions.OnOilBottleTaken += PlayerCollisions_OnOilBottleTaken;
        }

        private void Spawn(int count = 1)
        {
            for (var i = 0; i < count; i++)
            {
                var position = GetRandomPosition();
                Spawn(position.x, position.y);
            }
        }

        public void Spawn(int count, float x, float y)
        {
            for (var i = 0; i < count; i++)
            {
                Spawn(x, y);
            }
        }

        private void Spawn(float x, float y)
        {
            Instantiate(_oilBottle, new Vector3(x, y, 0), _oilBottle.transform.rotation);
        }

        private static Vector2 GetRandomPosition()
        {
            return new Vector2(
                Random.Range(-XRange, XRange),
                Random.Range(-YRange, YRange));
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

            Spawn();
        }

        private void OnDestroy()
        {
            PlayerCollisions.OnOilBottleTaken -= PlayerCollisions_OnOilBottleTaken;
        }
    }
}
