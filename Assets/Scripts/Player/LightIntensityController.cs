using System.Collections;
using Game;
using Resources;
using UnityEngine;

namespace Player
{
    public class LightIntensityController : MonoBehaviour
    {
        [SerializeField] private Light _light;
        [SerializeField] private float _easyModIntensity;
        [SerializeField] private float _hardModInitIntensity;
        [SerializeField] private float _lightDecrement;
        [SerializeField] private float _oilBottleModifier;
        [SerializeField] private float _maxLightIntensity;
        [SerializeField] private float _dirLightMod;
        
        private const float DecreasingIntervalSec = 0.025f;
        private const float DirLightHardModeMin = 0;
        private Light _dirLight;
        private bool _isOilAffectLight;
        
        private float Intensity
        {
            get => _light.intensity;
            set
            {
                _light.intensity = value;
                _dirLight.intensity = _light.intensity * _dirLightMod + DirLightHardModeMin;
            }
        }

        
        private void Start()
        {
            _dirLight = GameObject.FindGameObjectWithTag(Tags.DirectionalLight).GetComponent<Light>();
            _isOilAffectLight = FindObjectOfType<GameWaveManager>().CurrentWave.isOilAffectLight;

            if (_isOilAffectLight)
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
            if (Intensity < _maxLightIntensity && _isOilAffectLight)
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
}
