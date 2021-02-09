using Assets.Scripts;
using Assets.Scripts.Enums;
using UnityEngine;

public class DifficultyDropDownHandler : MonoBehaviour
{
    public void HandleDifficultyDropDown(int value) =>
        GameWaveManager.GameDifficulty = value == 0
            ? GameDifficulty.Easy
            : GameDifficulty.Hard;
}
