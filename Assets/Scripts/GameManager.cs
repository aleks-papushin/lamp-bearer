using Assets.Scripts.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject _spawner;
    public int oilBottleCount;

    public List<GameObject> WallsToBeDangerous => 
        FindObjectsOfType<Wall>().Where(w => !w.IsPlayerStandingOn).Select(w => w.gameObject).ToList();

    public bool IsThereOilBottles => GameObject.FindGameObjectsWithTag(TagNames.OilBottle).Any();

    [SerializeField]
    private float _wallWarningInterval;
    [SerializeField]
    private float _wallDangerousInterval;
    [SerializeField]
    private float _wallCoolDownInterval;



    // Debugging variables
    public bool switchOffWalls;
    public GameObject _player;
    //

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(this.HandleWallsDangerousness());
    }

    // Update is called once per frame
    void Update()
    {
        this.HandleOilSpawn();

        // Debug 
        this.RespawnPlayerIfHeDied();
        // 
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

            if (!switchOffWalls)
            {
                var wallIdx = new System.Random().Next(WallsToBeDangerous.Count);
                var wall = WallsToBeDangerous[wallIdx];
                
                StartCoroutine(wall.GetComponent<Wall>().BecameDangerousCoroutine(_wallWarningInterval));

                yield return new WaitForSeconds(dangerousInterval);

                wall.GetComponent<Wall>().BecameSafe();
            }
        }
    }
}
