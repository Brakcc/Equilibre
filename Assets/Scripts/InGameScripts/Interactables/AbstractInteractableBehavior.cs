using UnityEngine;

namespace InGameScripts.Interactables
{
    public abstract class AbstractInteractableBehavior : MonoBehaviour
    {
        #region methodes

        //To herit

        protected virtual void OnTriggerEnter(Collider other)
        {
            OnAction();
            OnAction(boolVal: default);
        }

        protected virtual void OnAction() {}
        protected virtual void OnAction(bool boolVal) {}

        #endregion
    }
}