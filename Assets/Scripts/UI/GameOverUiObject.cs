using Game;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameOverUiObject : MonoBehaviour
    {
        private GameStatus _gameStatus;
        private TextMeshProUGUI _textMesh;
        private Button _restartButton;
        private bool _submitWasPressed;

        private void Start()
        {
            _gameStatus = FindObjectOfType<GameStatus>();
            _textMesh = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
            _restartButton = GetComponentInChildren<Button>();
            _restartButton.onClick.AddListener(RestartGame);
            PlayerCollisions.OnPlayerDeath += PlayerCollisions_OnPlayerDied;
            _submitWasPressed = Input.GetButtonDown("Submit");
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_submitWasPressed && Input.GetButtonUp("Submit"))
            {
                _submitWasPressed = false;
            }
            
            if (!_submitWasPressed && Input.GetButtonDown("Submit"))
            {
                RestartGame();
            }
        }

        private void PlayerCollisions_OnPlayerDied(bool deathOfFire)
        {
            gameObject.SetActive(true);
            _textMesh.text = $"Your Score: {_gameStatus.Score}";
        }

        private static void RestartGame()
        {
            SceneManager.LoadScene(1);
        }

        private void OnDestroy()
        {
            PlayerCollisions.OnPlayerDeath -= PlayerCollisions_OnPlayerDied;
        }
    }
}
