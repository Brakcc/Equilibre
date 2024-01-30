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
            OnAction(intVal: default);
        }

        protected virtual void OnAction() {}
        protected virtual void OnAction(bool boolVal) {}
        protected virtual void OnAction(int intVal) {}

        #endregion
    }
}