﻿using UnityEngine;

namespace Wall
{
    public class WallLighting : MonoBehaviour
    {
        [SerializeField] private int _dangerIntensity;
        [SerializeField] private int _warningIntensity;

        private Light _light;

        // Start is called before the first frame update
        private void Start()
        {
            _light = GetComponent<Light>();
            gameObject.SetActive(false);
        }

        public void SetWarning()
        {
            _light.intensity = _warningIntensity;
            gameObject.SetActive(true);
        }

        public void SetDanger()
        {
            _light.intensity = _dangerIntensity;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
