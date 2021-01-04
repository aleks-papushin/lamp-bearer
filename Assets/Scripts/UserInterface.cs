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
            this.UpdateScoreDisplay();
        }

        public void ResetScore()
        {
            _currentScore = 0;
            this.UpdateScoreDisplay();
        }

        private void UpdateScoreDisplay()
        {
            scoreText.text = $": {_currentScore}";
        }
    }
}
