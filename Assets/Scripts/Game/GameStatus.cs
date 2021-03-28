using Difficulties;
using TMPro;
using UnityEngine;

namespace Game
{
    public class GameStatus: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        public int Score { get; private set; }

        public void AddToScore()
        {
            Score++;
            _scoreText.text = $": {Score}";
        }
    }
}