﻿using System;
using System.Collections;
using UnityEngine;

public class EnemyLifetimeScale : MonoBehaviour//, IEnemyInitialScaleHandler
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
        //Debug.Log($"In Awake before size reducing. Scale: {transform.localScale}, scale.x: {transform.localScale.x}, scale.y: {transform.localScale.y}, scale.x: { transform.localScale.z}");
        _defaultScaleX = Math.Abs(transform.localScale.x);
        transform.localScale *= _initScaleMultiplier;
        //Debug.Log($"In Awake after size reducing. Scale: {transform.localScale}, scale.x: {transform.localScale.x}, scale.y: {transform.localScale.y}, scale.x: { transform.localScale.z}");        
    }

    public IEnumerator DecreaseSizeRoutine()
    {
        yield return null;

        var scaleIncrementVector = ScaleIncrementVector;
        var finalScale = _defaultScaleX * _initScaleMultiplier;
        while (Math.Abs(transform.localScale.x) > finalScale)
        {
            transform.localScale -= scaleIncrementVector;
            yield return new WaitForSeconds(_routineTimeInterval);
        }
    }

    public IEnumerator IcreaseSizeRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        var scaleIncrementVector = ScaleIncrementVector;
        while (Math.Abs(transform.localScale.x) < _defaultScaleX)
        {            
            transform.localScale += scaleIncrementVector;
            yield return new WaitForSeconds(_routineTimeInterval);
        }
    }
}
