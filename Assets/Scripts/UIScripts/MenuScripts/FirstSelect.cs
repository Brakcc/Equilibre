using UnityEngine;
using UnityEngine.UI;

namespace UIScripts.MenuScripts
{
    public class FirstSelect : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<Button>().Select();
        }
    }
}