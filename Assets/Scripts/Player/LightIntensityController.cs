using Assets.Scripts;
using Assets.Scripts.Resources;
using System.Collections;
using UnityEngine;

public class LightIntensityController : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _easyModInitIntensity;
    [SerializeField] private float _hardModInitIntensity;
    [SerializeField] private float _lightDecrement;
    [SerializeField] private float _oilBottleModifier;
    [SerializeField] private float _maxLightIntensity;
    [SerializeField] private float _dirLightMod;
    [SerializeField] private bool _debugNotUseDirLight; // debug

    private GameObject _dirLightObj;
    private GameManager _gameManager;

    private readonly float _decreasingIntervalSec = 0.025f;
    private readonly float _dirLightHardModeMin = 0;
    private readonly float _dirLightEasyModeIntensity = 0.8f;
    private Light _dirLight;

    public float Intensity
    {
        get => _light.intensity;
        set
        {
            _light.intensity = value;
            if (!_debugNotUseDirLight)
                _dirLight.intensity = (_light.intensity * _dirLightMod) + _dirLightHardModeMin;
        }
    }

    public bool IsOilAffectLight { get; set; }

    private void Start()
    {
        _dirLightObj = GameObject.FindGameObjectWithTag(Tags.DirectionalLight);
        _dirLight = _dirLightObj.GetComponent<Light>();
        _gameManager = FindObjectOfType<GameManager>();
        IsOilAffectLight = _gameManager.WaveManager.CurrentWave.isOilAffectLight;

        if (IsOilAffectLight)
        {
            Intensity = _hardModInitIntensity;
            StartCoroutine(DecreaseIntensityRoutine());
        }
        else
        {
            Intensity = _easyModInitIntensity;
            //_dirLight.intensity = _dirLightEasyModeIntensity;
        }
    }

    public void OilTaken()
    {
        if (Intensity < _maxLightIntensity && IsOilAffectLight)
        {
            Intensity += _oilBottleModifier;
        }
    }

    private IEnumerator DecreaseIntensityRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_decreasingIntervalSec);
            Intensity -= _lightDecrement;
        }
    }
}
