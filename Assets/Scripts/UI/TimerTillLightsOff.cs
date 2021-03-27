using Game;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TimerTillLightsOff : MonoBehaviour
    {
        private float _timeRemains;
        private TextMeshProUGUI _text;

        private void Start()
        {
            _timeRemains = FindObjectOfType<GameWaveManager>().GetTimeTillLightsOff();
            _text = FindObjectOfType<TextMeshProUGUI>();
            _text.text = _timeRemains.ToString("F0");
            PlayerCollisions.OnPlayerDeath += DestroyObject;
        }
    
        private void Update()
        {
            if (_timeRemains <= 0)
            {
                DestroyObject(false);
            }
            else
            {
                _timeRemains -= Time.smoothDeltaTime;
                _text.text = _timeRemains.ToString("F0");
            }
        }

        private void DestroyObject(bool ignored)
        {
            Destroy(gameObject);
        }
    }
}
