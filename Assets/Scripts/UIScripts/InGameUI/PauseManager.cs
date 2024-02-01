using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UIScripts.InGameUI
{
    public class PauseManager : MonoBehaviour
    {
        #region fields

        [SerializeField] private GameObject pauseGO;
        
        private bool _isPaused;

        #endregion

        #region methodes

        private void Start()
        {
            pauseGO.SetActive(false);
        }

        public void OnPauseGame(InputAction.CallbackContext ctx)
        {
            if (!_isPaused)
            {
                _isPaused = true;
                Time.timeScale = 0;
                pauseGO.SetActive(true);
            }
            else
            {
                _isPaused = false;
                Time.timeScale = 1;
                pauseGO.SetActive(false);
            }
        }

        public void OnResume()
        {
            _isPaused = false;
            Time.timeScale = 1;
            pauseGO.SetActive(false);
        }

        public void OnBackToMenu() => SceneManager.LoadScene("Menu");

        #endregion
    }
}