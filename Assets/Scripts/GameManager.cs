using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject _spawner;
    public int oilBottleCount;


    // Start is called before the first frame update
    void Start()
    {
        _spawner.GetComponent<SpawnOil>().Spawn(oilBottleCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
