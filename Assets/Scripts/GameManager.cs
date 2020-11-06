using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject _spawner;
    public int oilBottleCount;

    public List<GameObject> Walls { get; set; }

    [SerializeField]
    private float _wallChangingInterval;

    // Start is called before the first frame update
    void Start()
    {
        Walls = FindObjectsOfType<Wall>().Select(w => w.gameObject).ToList();
        //StartCoroutine(this.HandleWalls());

        _spawner.GetComponent<SpawnOil>().Spawn(oilBottleCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator HandleWalls()
    {
        while (true)
        {
            // every N sec pick random wall and make it danger
            yield return new WaitForSeconds(_wallChangingInterval);

            var wallIdx = new System.Random().Next(Walls.Count);
            var wall = Walls[wallIdx];
            wall.GetComponent<Wall>().BecameDangerous();

            yield return new WaitForSeconds(_wallChangingInterval);

            wall.GetComponent<Wall>().BecameSafe();
        }
    }
}
