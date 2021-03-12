using Enums;
using Game;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace UI
{
    public class ChangeDifficultyButton: MonoBehaviour
    {
        private Button _changeDifficultyButton;
        public TMP_Text difficultyLabel;

        private void Start()
        {
            _changeDifficultyButton = GetComponent<Button>();
            _changeDifficultyButton.onClick.AddListener(ChangeDifficulty);
            difficultyLabel.text = GameWaveManager.GameDifficulty + " Mode";
        }

        private void ChangeDifficulty()
        {
            GameWaveManager.GameDifficulty = GameWaveManager.GameDifficulty == GameDifficulty.Easy ? GameDifficulty.Hard : GameDifficulty.Easy;
            difficultyLabel.text = GameWaveManager.GameDifficulty + " Mode";
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ChangeDifficulty();
            }
        }
    }
}