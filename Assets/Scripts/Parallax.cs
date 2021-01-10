using Assets.Scripts.Resources;
using System.Collections;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _degree;
    private GameObject _source;

    void Start()
    {
        _source = GameObject.FindGameObjectWithTag(Tags.Player);

        StartCoroutine(ParallaxRoutine());
    }

    private IEnumerator ParallaxRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);

            var newPosX = _source.transform.position.x * _degree;
            var newPosY = _source.transform.position.y * _degree;

            transform.position = new Vector3(newPosX, newPosY, transform.position.z);
        }
    }
}
