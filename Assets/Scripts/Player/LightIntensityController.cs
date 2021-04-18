using System.Collections;
using Game;
using Resources;
using UnityEngine;

namespace Player
{
    public class LightIntensityController : MonoBehaviour
    {
        private const float DecreasingIntervalSec = 0.025f;

        [SerializeField] private float _dirLightMinimum;
        [SerializeField] private Light _light;
        [SerializeField] private float _easyModIntensity;
        [SerializeField] private float _hardModInitIntensity;
        [SerializeField] private float _lightDecrement;
        [SerializeField] private float _oilBottleModifier;
        [SerializeField] private float _maxLightIntensity;
        [SerializeField] private float _dirLightMod;

        private GameWaveManager _waveManager;
        private Light _dirLight;
        private IEnumerator _lightControlRoutine;
        private bool _oilAffectedLightInPreviouisWave;
        
        private float Intensity
        {
            get => _light.intensity;
            set
            {
                _light.intensity = value;

                if (!OilAffectsLightInCurrentWave)
                {
                    _dirLight.intensity = _dirLightMinimum;
                }
                else
                {
                    _dirLight.intensity = 0;
                }
            }
        }

        public bool OilAffectsLightInCurrentWave => _waveManager != null && _waveManager.CurrentWave.isOilAffectLight;
        public bool ShouldChangeLightMode => _oilAffectedLightInPreviouisWave != OilAffectsLightInCurrentWave;

        private void Start()
        {
            _waveManager = FindObjectOfType<GameWaveManager>();
            _dirLight = GameObject.FindGameObjectWithTag(Tags.DirectionalLight).GetComponent<Light>();
            _lightControlRoutine = DecreaseIntensityRoutine();
            PickLightControlMode();
            GameTimer.OnWaveIncrementing += GameTimer_OnWaveIncrementing;
            _oilAffectedLightInPreviouisWave = OilAffectsLightInCurrentWave;
        }

        public void OilTaken()
        {
            if (Intensity < _maxLightIntensity && OilAffectsLightInCurrentWave)
            {
                Intensity += _oilBottleModifier;
            }
        }

        private void GameTimer_OnWaveIncrementing()
        {
            if (!ShouldChangeLightMode) return;

            PickLightControlMode();
            _oilAffectedLightInPreviouisWave = OilAffectsLightInCurrentWave;
        }

        private void PickLightControlMode()
        {
            if (OilAffectsLightInCurrentWave)
            {
                Intensity = _hardModInitIntensity;
                StartCoroutine(_lightControlRoutine);
            }
            else
            {
                StopCoroutine(_lightControlRoutine);
                Intensity = _easyModIntensity;
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

        private void OnDestroy()
        {
            GameTimer.OnWaveIncrementing -= GameTimer_OnWaveIncrementing;
        }
    }
}
