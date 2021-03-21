using Difficulties;
using TMPro;
using UnityEngine;

namespace Game
{
    public class GameStatus: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        private int _scoreValue;
        
        public int getScore()
        {
            return _scoreValue;
        }

        public void AddToScore()
        {
            _scoreValue += GameWaveManager.GameDifficulty == GameDifficulty.Easy ? 1 : 2;
            _scoreText.text = $": {_scoreValue}";
        }
    }
}