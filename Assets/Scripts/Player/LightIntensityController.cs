using System.Collections;
using UnityEngine;

public class LightIntensityController : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _initIntensity;
    [SerializeField] private float _lightFading;
    [SerializeField] private float _oilBottleModifier;
    [SerializeField] private float _maxLightIntensity;
    [SerializeField] private GameObject _dirLightObj;
    [SerializeField] private float _dirLightMod;

    private readonly float _decreasingIntervalSec = 0.025f;
    private Light _dirLight;

    public float Intensity
    {
        get => _light.intensity;
        set
        {
            _light.intensity = value;
            _dirLight.intensity = (_light.intensity * _dirLightMod) + 0.05f; // hardcode to not to set dir lighth to 0
        }
    }

    private void Start()
    {
        _dirLight = _dirLightObj.GetComponent<Light>();
        Intensity = _initIntensity;
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
