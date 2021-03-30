using System.Collections.Generic;
using System.Linq;
using Difficulties;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ChangeDifficultyButton : MonoBehaviour
    {
        public TMP_Text difficultyLabel;
        private List<Button> _changeDifficultyButtons = new List<Button>();
        private bool _horAxisWasZero;

        private static string DifficultyLabelText => $"{GameWaveManager.GameDifficulty} Mode";

        private void Start()
        {
            _changeDifficultyButtons = GetComponentsInChildren<Button>().ToList();
            _changeDifficultyButtons.ForEach(b => b.onClick.AddListener(ChangeDifficulty));
            difficultyLabel.text = DifficultyLabelText;
        }

        private void ChangeDifficulty()
        {
            GameWaveManager.GameDifficulty = 
                GameWaveManager.GameDifficulty == GameDifficulty.Easy ? GameDifficulty.Hard : GameDifficulty.Easy;
            difficultyLabel.text = DifficultyLabelText;
            _horAxisWasZero = Input.GetAxisRaw("Horizontal") != 0;
        }
        
        private void Update()
        {
            if (_horAxisWasZero && Input.GetAxisRaw("Horizontal") != 0)
            {
                _horAxisWasZero = false;
                ChangeDifficulty();
            }
            else if (!_horAxisWasZero && Input.GetAxisRaw("Horizontal") == 0)
            {
                _horAxisWasZero = true;
            }
        }
    }
}