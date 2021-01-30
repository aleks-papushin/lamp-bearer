using System;
using System.Collections;
using UnityEngine;

public class EnemyScaling : MonoBehaviour
{
    [SerializeField] private float _initScaleMultiplier;
    [SerializeField] private float _incrementValue;
    [SerializeField] private float _routineTimeInterval;
    private float _defaultScaleX;
    private Vector3 _scaleIncrementVector;

    public Vector3 ScaleIncrementVector
    {
        get
        {
            float x = transform.localScale.x > 0 ? _incrementValue : -_incrementValue;
            float y = transform.localScale.y > 0 ? _incrementValue : -_incrementValue;
            float z = transform.localScale.z > 0 ? _incrementValue : -_incrementValue;

            _scaleIncrementVector = new Vector3(x, y, z);

            return _scaleIncrementVector;
        }
    }

    private void Awake()
    {
        _defaultScaleX = Math.Abs(transform.localScale.x);
        transform.localScale *= _initScaleMultiplier;
    }

    public IEnumerator DecreaseSizeRoutine()
    {
        var scaleIncrementVector = ScaleIncrementVector;
        var finalScale = _defaultScaleX * _initScaleMultiplier * 0.1;
        while (Math.Abs(transform.localScale.x) > finalScale)
        {
            transform.localScale -= scaleIncrementVector * 0.3f;
            yield return new WaitForSeconds(_routineTimeInterval);
        }

        Destroy(gameObject);
    }

    public IEnumerator IncreaseSizeRoutine()
    {
        yield return new WaitForSeconds(0.3f);

        var scaleIncrementVector = ScaleIncrementVector;
        while (Math.Abs(transform.localScale.x) < _defaultScaleX)
        {            
            transform.localScale += scaleIncrementVector;
            yield return new WaitForSeconds(_routineTimeInterval);
        }
    }
}
