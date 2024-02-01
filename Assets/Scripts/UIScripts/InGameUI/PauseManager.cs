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
            Time.timeScale = 1;
            Cursor.visible = false;
            pauseGO.SetActive(false);
        }

        public void OnPauseGame(InputAction.CallbackContext ctx)
        {
            if (!_isPaused)
            {
                _isPaused = true;
                Cursor.visible = true;
                Time.timeScale = 0;
                pauseGO.SetActive(true);
            }
            else
            {
                _isPaused = false;
                Cursor.visible = false;
                Time.timeScale = 1;
                pauseGO.SetActive(false);
            }
        }

        public void OnResume()
        {
            _isPaused = false;
            Cursor.visible = false;
            Time.timeScale = 1;
            pauseGO.SetActive(false);
        }

        public void OnBackToMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
        }

        #endregion
    }
}