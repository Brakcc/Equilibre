using UnityEngine;

//Git
namespace InGameScripts.Interactables
{
    public abstract class AbstractInteractableBehavior : MonoBehaviour
    {
        #region methodes

        //To herit
        protected virtual void OnTriggerEnter(Collider other)
        {
            OnAction();
            OnAction(default);
        }
        
        protected virtual void OnAction() {}
        protected virtual void OnAction(bool tempB) {}

        #endregion
    }
}