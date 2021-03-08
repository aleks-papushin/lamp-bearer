using Assets.Scripts.Resources;
using System.Collections;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _degree;
    private GameObject _source;

    private void Awake()
    {
        var cachedTransform = transform;
        cachedTransform.position = new Vector3(0, 0, cachedTransform.position.z);
    }

    private void Start()
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
                var position = _source.transform.position;
                var newPosX = position.x * _degree;
                var newPosY = position.y * _degree;

                var cachedTransform = transform;
                cachedTransform.position = new Vector3(newPosX, newPosY, cachedTransform.position.z);
            }
            else
            {
                _source = GameObject.FindGameObjectWithTag(Tags.Player);
            }

            yield return new WaitForSeconds(0.02f);
        }
    }
}
