using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class UserInterface : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        public int CurrentScore { get; private set; }

        public void UpdateScore(int increment)
        {
            CurrentScore += increment;
            UpdateScoreDisplay();
        }

        public void ResetScore()
        {
            CurrentScore = 0;
            UpdateScoreDisplay();
        }

        private void UpdateScoreDisplay()
        {
            scoreText.text = $": {CurrentScore}";
        }
    }
}
