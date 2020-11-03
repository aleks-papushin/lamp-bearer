using UnityEngine;

public class SpawnOil : MonoBehaviour
{
    public GameObject _oilBottle;

    private float _xRange = 8;
    private float _yRange = 4; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Spawn()
    {
        var position = GetRandomPosition();
        Instantiate(_oilBottle, new Vector3(position.x, position.y, 0), _oilBottle.transform.rotation);
    }

    private Vector2 GetRandomPosition()
    {
        return new Vector2(
            Random.Range(-_xRange, _xRange),
            Random.Range(-_yRange, _yRange));
    }
}
