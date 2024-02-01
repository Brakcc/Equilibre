using UnityEngine;
using UnityEngine.UI;

namespace UIScripts.MenuScripts
{
    public class MouseHoverPause : MonoBehaviour
    {
        [SerializeField] private Button button;

        private readonly Vector3 originsCale = new(1, 1, 1);

        private readonly Vector3 scaleChange = new(0.1f, 0.1f, 0.1f);

        public void OnPointerEnter()
        {
            button.transform.localScale = originsCale + scaleChange;
        }

        public void OnPointerExit()
        {
            button.transform.localScale = originsCale;
        }

        public void OnEnable()
        {
            button.transform.localScale = originsCale;
        }
        public void OnDisable()
        {
            button.transform.localScale = originsCale;
        }
    }
}