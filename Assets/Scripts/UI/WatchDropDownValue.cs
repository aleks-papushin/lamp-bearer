using System;
using Assets.Scripts;
using Assets.Scripts.Enums;
using TMPro;
using UnityEngine;

public class WatchDropDownValue : MonoBehaviour
{
    private TMP_Dropdown _dropdown;

    private void Start()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        Handle();
    }

    private void Handle()
    {
        switch (GameWaveManager.GameDifficulty)
        {
            case GameDifficulty.Easy:
                _dropdown.value = 0;
                break;
            case GameDifficulty.Hard:
                _dropdown.value = 1;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
