using Enums;
using Game;
using UnityEngine;

namespace UI
{
    public class DifficultyDropDownHandler : MonoBehaviour
    {
        public void HandleDifficultyDropDown(int value) =>
            GameWaveManager.GameDifficulty = value == 0
                ? GameDifficulty.Easy
                : GameDifficulty.Hard;
    }
}
