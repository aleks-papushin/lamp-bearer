using Assets.Scripts.Resources;
using System.Collections;
using UnityEngine;

public class LightIntensityController : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _initIntensity;
    [SerializeField] private float _lightFading;
    [SerializeField] private float _oilBottleModifier;
    [SerializeField] private float _maxLightIntensity;
    [SerializeField] private float _dirLightMod;
    private GameObject _dirLightObj;

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
        _dirLightObj = GameObject.FindGameObjectWithTag(Tags.DirectionalLight);
        _dirLight = this.InitDirLight();
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
    private Light InitDirLight()
    {
        return _dirLightObj.GetComponent<Light>();
    }
}
