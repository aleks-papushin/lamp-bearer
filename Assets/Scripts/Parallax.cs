using Assets.Scripts.Resources;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _degree;
    private GameObject _source;

    void Start()
    {
        _source = GameObject.FindGameObjectWithTag(Tags.Player);                
    }

    void Update()
    {
        var newPosX = _source.transform.position.x * _degree;
        var newPosY = _source.transform.position.y * _degree;

        transform.position = new Vector3(newPosX, newPosY, transform.position.z);
    }
}
