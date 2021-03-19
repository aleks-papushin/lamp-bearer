using Player;
using Spawn;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameOverUiObject : MonoBehaviour
    {
        private OilSpawnManager _oilSpawnManager;
        private TextMeshProUGUI _textMesh;
        private Button _restartButton;

        private void Start()
        {
            _oilSpawnManager = FindObjectOfType<OilSpawnManager>();
            _textMesh = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
            _restartButton = GetComponentInChildren<Button>();
            _restartButton.onClick.AddListener(RestartGame);
            PlayerCollisions.OnPlayerDied += PlayerCollisions_OnPlayerDied;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) ||
                Input.GetKeyDown("joystick button 0") ||
                Input.GetKeyDown("joystick button 7"))
            {
                RestartGame();
            }
        }

        private void PlayerCollisions_OnPlayerDied()
        {
            gameObject.SetActive(true);
            _textMesh.text = $"Your Score: {_oilSpawnManager.CurrentScore}";
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
