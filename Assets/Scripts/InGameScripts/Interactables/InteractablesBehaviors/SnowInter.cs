using UnityEngine;

namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public sealed class SnowInter : AbstractInteractableBehavior
    {
        #region methodes
        
        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("FirePower"))
                return;
            
            OnAction();
        }

        protected override void OnAction()
        {
            var tempRend = GetComponent<MeshRenderer>();
            var tempCol = GetComponent<Collider>();
                
            tempRend.enabled = false;
            tempCol.enabled = false;
        }
        
        #endregion
    }
}