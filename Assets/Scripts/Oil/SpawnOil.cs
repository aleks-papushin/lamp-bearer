using UnityEngine;

public class SpawnOil : MonoBehaviour
{
    public GameObject _oilBottle;

    private float _xRange = 6;
    private float _yRange = 2; 

    public void Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var position = GetRandomPosition();
            Instantiate(_oilBottle, new Vector3(position.x, position.y, 0), _oilBottle.transform.rotation);
        }
    }

    private Vector2 GetRandomPosition()
    {
        return new Vector2(
            Random.Range(-_xRange, _xRange),
            Random.Range(-_yRange, _yRange));
    }
}
