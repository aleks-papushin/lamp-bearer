﻿using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameOverUiObject : MonoBehaviour
    {
        private UserInterface _ui;
        private TextMeshProUGUI _textMesh;
        private Button _restartButton;

        private void Start()
        {
            _ui = FindObjectOfType<UserInterface>();
            _textMesh = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
            _restartButton = GetComponentInChildren<Button>();
            _restartButton.onClick.AddListener(RestartGame);
            PlayerCollisions.OnPlayerDied += PlayerCollisions_OnPlayerDied;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                RestartGame();
            }
        }

        private void PlayerCollisions_OnPlayerDied()
        {
            gameObject.SetActive(true);
            _textMesh.text = $"Your Score: {_ui.CurrentScore}";
        }

        private static void RestartGame()
        {
            SceneManager.LoadScene(1);
        }

        private void OnDestroy()
        {
            PlayerCollisions.OnPlayerDied -= PlayerCollisions_OnPlayerDied;
        }
    }
}
