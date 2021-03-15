using Enums;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ChangeDifficultyButton : MonoBehaviour
    {
        private Button _changeDifficultyButton;
        public TMP_Text difficultyLabel;

        public string DifficultyLabelText => $"{GameWaveManager.GameDifficulty} Mode";

        private void Start()
        {
            _changeDifficultyButton = GetComponent<Button>();
            _changeDifficultyButton.onClick.AddListener(ChangeDifficulty);
            difficultyLabel.text = DifficultyLabelText;
        }

        private void ChangeDifficulty()
        {
            GameWaveManager.GameDifficulty = GameWaveManager.GameDifficulty == GameDifficulty.Easy ? GameDifficulty.Hard : GameDifficulty.Easy;
            difficultyLabel.text = DifficultyLabelText;
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