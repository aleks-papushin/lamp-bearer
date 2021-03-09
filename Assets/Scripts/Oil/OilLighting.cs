using Player;
using Resources;
using UnityEngine;

namespace Oil
{
    public class OilLighting : MonoBehaviour
    {
        private GameObject player;
        private Light _light;
        [SerializeField] private float _maxLightIntensity;
        private static bool playerDied;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag(Tags.Player);
            playerDied = false;
            _light = GetComponent<Light>();
            PlayerCollisions.OnPlayerDied += PlayerDied;
        }

        private void Update()
        {
            if (playerDied) return;
            var distance = Vector2.Distance(transform.position, player.transform.position);
            _light.intensity = _maxLightIntensity / (distance * (distance * 0.1f) + 1);
        }
    
        private static void PlayerDied()
        {
            playerDied = true;
        }
    }
}