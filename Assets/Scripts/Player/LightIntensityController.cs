using Assets.Scripts;
using Assets.Scripts.Resources;
using System.Collections;
using UnityEngine;

public class LightIntensityController : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _easyModIntensity;
    [SerializeField] private float _hardModInitIntensity;
    [SerializeField] private float _lightDecrement;
    [SerializeField] private float _oilBottleModifier;
    [SerializeField] private float _maxLightIntensity;
    [SerializeField] private float _dirLightMod;

    private GameObject _dirLightObj;
    private GameManager _gameManager;

    private const float DecreasingIntervalSec = 0.025f;
    private const float DirLightHardModeMin = 0;
    private Light _dirLight;

    private float Intensity
    {
        get => _light.intensity;
        set
        {
            _light.intensity = value;
            _dirLight.intensity = _light.intensity * _dirLightMod + DirLightHardModeMin;
        }
    }

    private bool IsOilAffectLight { get; set; }

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
            Intensity = _easyModIntensity;
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
            yield return new WaitForSeconds(DecreasingIntervalSec);
            Intensity -= _lightDecrement + _lightDecrement * Intensity;
        }
    }
}
