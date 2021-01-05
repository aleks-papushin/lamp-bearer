using Assets.Scripts;
using Assets.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUiObject : MonoBehaviour
{
    private UserInterface _ui;
    private TextMeshProUGUI _textMesh;
    private Button _restartButton;

    private void Awake()
    {
        
    }

    void Start()
    {
        _ui = FindObjectOfType<UserInterface>();
        _textMesh = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        _restartButton = GetComponentInChildren<Button>();
        _restartButton.onClick.AddListener(RestartGame);
        PlayerCollisions.OnPlayerDied += PlayerCollisions_OnPlayerDied;
        gameObject.SetActive(false);
    }

    private void PlayerCollisions_OnPlayerDied()
    {
        gameObject.SetActive(true);
        _textMesh.text = $"Your Score: {_ui.CurrentScore}";
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void OnDestroy()
    {
        PlayerCollisions.OnPlayerDied -= PlayerCollisions_OnPlayerDied;
    }
}
