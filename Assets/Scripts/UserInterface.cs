using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class UserInterface : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        private int _currentScore;

        public void UpdateScore(int increment)
        {
            _currentScore += increment;
            scoreText.text = $"Score: {_currentScore}";
        }
    }
}
