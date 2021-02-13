using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WatchDropDownValue : MonoBehaviour
{
    private TMP_Dropdown _dropdown;

    void Start()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        Handle();
    }

    private void Handle()
    {
        if (GameWaveManager.GameDifficulty == GameDifficulty.Easy)
        {
            _dropdown.value = 0;
        }
        else if (GameWaveManager.GameDifficulty == GameDifficulty.Hard)
        {
            _dropdown.value = 1;
        }
    }
}
