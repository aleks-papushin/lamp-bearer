using Assets.Scripts;
using Assets.Scripts.Player;
using Assets.Scripts.Resources;
using System.Collections;
using UnityEngine;

public class SpawnOil : MonoBehaviour
{
    public GameObject _oilBottle;

    private readonly float _xRange = 6;
    private readonly float _yRange = 2;
    private GameObject _player;
    private GameObject _bottle;

    public bool IsBottleExist => _bottle != null;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Tags.Player);
        PlayerCollisions.OnOilBottleTaken += PlayerCollisions_OnOilBottleTaken;
    }

    public void Spawn(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            var position = GetRandomPosition();
            this.Spawn(position.x, position.y);
        }
    }

    public void Spawn(int count, float x, float y)
    {
        for (int i = 0; i < count; i++)
        {
            this.Spawn(x, y);
        }
    }

    private void Spawn(float x, float y)
    {
        _bottle = Instantiate(_oilBottle, new Vector3(x, y, 0), _oilBottle.transform.rotation);
    }

    private Vector2 GetRandomPosition()
    {
        return new Vector2(
            Random.Range(-_xRange, _xRange),
            Random.Range(-_yRange, _yRange));
    }

    private void PlayerCollisions_OnOilBottleTaken()
    {
        StartCoroutine(WaitPlayerGroundedAndSpawnOil());
    }

    private IEnumerator WaitPlayerGroundedAndSpawnOil()
    {
        var playerWallCollisions = _player.GetComponent<ObjectWallCollisions>();

        while (!playerWallCollisions.IsGrounded)
        {
            yield return new WaitForSeconds(0.5f);
        }

        Spawn();
    }
}
