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

    [SerializeField]
    private float _wallWarningInterval;
    [SerializeField]
    private float _wallChangingInterval;
    [SerializeField]
    private float _wallCoolDownInterval;

    // Debugging variables
    public bool switchOffWalls;
    //

    // Start is called before the first frame update
    void Start()
    {
        switchOffWalls = false;
        StartCoroutine(this.MakeWallsDangerous());

        _spawner.GetComponent<SpawnOil>().Spawn(oilBottleCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator MakeWallsDangerous()
    {
        // every N sec pick random wall and make it danger

        while (true)
        {
            var dangerousInterval = _wallChangingInterval + _wallWarningInterval;

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
