﻿using System.Collections;
using UnityEngine;

public class LightIntensityController : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _initIntensity;
    [SerializeField] private float _lightFading;
    [SerializeField] private float _oilBottleModifier;
    [SerializeField] private float _maxLightIntensity;

    private readonly float _decreasingIntervalSec = 0.025f;

    public float Intensity
    {
        get => _light.intensity;
        set => _light.intensity = value;
    }

    private void Awake()
    {
        Intensity = _initIntensity;
    }

    private void Start()
    {
        StartCoroutine(DecreaseIntensityRoutine());
    }

    public void OilTaken()
    {
        if (Intensity < _maxLightIntensity)
        {
            Intensity += _oilBottleModifier;
        }
    }

    private IEnumerator DecreaseIntensityRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_decreasingIntervalSec);
            Intensity -= _lightFading;
        }
    }
}
