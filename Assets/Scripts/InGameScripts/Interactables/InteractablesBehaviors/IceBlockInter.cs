using UnityEngine;

//Git
namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public sealed class IceBlockInter : AbstractInteractableBehavior
    {
        #region fields

        [Header("Graph")]
        public Animation liquefactionAnim;
        public ParticleSystem liquefactionVFX;
        public Animation solidificationAnim;
        public ParticleSystem solidificationVFX;

        #endregion

        #region methodes

        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("FirePower"))
                return;
            
            OnAction();
        }

        protected override void OnAction()
        {

            Destroy(gameObject);
        }

        #endregion
    }
}