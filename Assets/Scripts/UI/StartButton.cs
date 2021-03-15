using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class StartButton : MonoBehaviour
    {
        private Button _startButton;

        public static event Action OnGameStarted;

        private void Start()
        {
            _startButton = GetComponent<Button>();
            _startButton.onClick.AddListener(StartGame);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || 
                Input.GetKeyDown("joystick button 0") || 
                Input.GetKeyDown("joystick button 7"))
            {
                StartGame();
            }
        }

        private static void StartGame()
        {
            SceneManager.LoadScene(1);
            OnGameStarted?.Invoke();
        }
    }
}
