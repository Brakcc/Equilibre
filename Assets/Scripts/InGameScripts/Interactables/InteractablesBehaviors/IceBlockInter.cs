using UnityEngine;

namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public sealed class IceBlockInter : AbstractInteractableBehavior
    {
        #region methodes

        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("FirePower"))
                return;
            
            OnAction();
        }

        protected override void OnAction() => Destroy(gameObject);

        #endregion
    }
}