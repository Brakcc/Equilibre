using UnityEngine;

//Git
namespace InGameScripts.Interactables
{
    public abstract class AbstractInteractableBehavior : MonoBehaviour
    {
        #region methodes

        //To herit
        protected abstract void OnAction<T>(T t) where T : struct;

        #endregion
    }
}