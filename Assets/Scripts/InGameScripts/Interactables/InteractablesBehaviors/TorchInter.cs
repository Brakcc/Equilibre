using UnityEngine;

namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public class TorchInter : AbstractInteractableBehavior
    {
        #region fields

        [SerializeField] private MeshRenderer fireRend;
        public bool isOnFire;

        #endregion

        #region methodes

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("FirePower"))
                OnAction(true);
            
            if (other.CompareTag("IcePower"))
                OnAction(true);
        }

        protected override void OnAction(bool active)
        {
            isOnFire = active;
            fireRend.enabled = active;
        }

        #endregion
    }
}