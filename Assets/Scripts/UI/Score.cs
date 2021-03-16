using TMPro;
using UnityEngine;

namespace UI
{
    public class Score : MonoBehaviour
    {
        private TextMeshProUGUI _scoreText;

        private void Start()
        {
            _scoreText = GetComponent<TextMeshProUGUI>();
        }

        public void SetScore(int value)
        {
            _scoreText.text = $": {value}";
        }
    }
}
