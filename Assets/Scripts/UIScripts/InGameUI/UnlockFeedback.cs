using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Utilities.CustomAttributes;

namespace UIScripts.InGameUI
{
    public class UnlockFeedback : MonoBehaviour
    {
        #region fields

        [FieldCompletion] [SerializeField] private TMP_Text unlockFeed;

        #endregion

        #region methodes

        public void OnEnable()
        {
            unlockFeed.text = "FIRE  UNLOCKED  !!!";
            transform.DOMoveY(transform.position.y + 0.75f, 2);
            unlockFeed.color = Color.red;
            
            OnStartLife();
            
            Destroy(gameObject, 2.01f);
        }

        private async void OnStartLife()
        {
            for (var i = 0; i < 4; i++)
            {
                if (i % 2 == 0)
                {
                    unlockFeed.color = Color.Lerp(Color.red, Color.yellow, 0.5f);
                    await Task.Delay(500);
                    continue;
                }
                unlockFeed.color = Color.red;

                await Task.Delay(500);
            }
        }

        #endregion
    }
}