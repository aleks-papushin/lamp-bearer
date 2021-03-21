using Difficulties;
using UI;
using UnityEngine;

namespace Game
{
    public class GameStatus: MonoBehaviour
    {
        private Score _score;
        private int _scoreValue;

        private void Start()
        {
            _score = FindObjectOfType<Score>();
        }

        public int getScore()
        {
            return _scoreValue;
        }

        public void AddToScore()
        {
            _scoreValue += GameWaveManager.GameDifficulty == GameDifficulty.Easy ? 1 : 2;
            _score.SetScore(_scoreValue);
        }
    }
}