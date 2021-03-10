using Player;
using Resources;
using UnityEngine;

namespace Oil
{
    public class OilLighting : MonoBehaviour
    {
        [SerializeField] private float _maxLightIntensity;
        private GameObject _player;
        private Light _light;
        private static bool _playerDied;

        public void OnPlayerDeath()
        {
            Debug.Log("here");
        }
        
        
        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag(Tags.Player);
            _playerDied = false;
            _light = GetComponent<Light>();
            PlayerCollisions.OnPlayerDied += PlayerDied;
        }

        private void Update()
        {
            if (_playerDied) return;
            var distance = Vector2.Distance(transform.position, _player.transform.position);
            _light.intensity = _maxLightIntensity / (distance * (distance * 0.1f) + 1);
        }
    
        private static void PlayerDied()
        {
            _playerDied = true;
        }
    }
}