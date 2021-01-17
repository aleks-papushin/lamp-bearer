using Assets.Scripts.Resources;
using System.Collections;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _degree;
    private GameObject _source;

    private void Awake()
    {
        transform.position = new Vector3(0, 0, transform.position.z);
    }

    void Start()
    {
        _source = GameObject.FindGameObjectWithTag(Tags.Player);
        if (_source != null)
        {
            StartCoroutine(ParallaxRoutine());
        }
    }

    private IEnumerator ParallaxRoutine()
    {
        while (true)
        {
            if (_source != null)
            {
                var newPosX = _source.transform.position.x * _degree;
                var newPosY = _source.transform.position.y * _degree;

                transform.position = new Vector3(newPosX, newPosY, transform.position.z);
            }
            else
            {
                _source = GameObject.FindGameObjectWithTag(Tags.Player);
            }

            yield return new WaitForSeconds(0.02f);
        }
    }
}
