using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIScripts.MenuScripts
{
    public class MenuManager : MonoBehaviour
    {
        #region fields

        [SerializeField] private string gameScene;

        #endregion

        #region methodes

        public void OnLoadScene(string arg) => SceneManager.LoadScene(arg);

        public void OnQuitGame() => Application.Quit();

        #endregion
    }
}