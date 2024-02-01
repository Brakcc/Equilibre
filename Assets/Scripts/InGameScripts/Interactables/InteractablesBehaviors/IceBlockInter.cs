using UnityEngine;
using Utilities.CustomAttributes;

//Git
namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public sealed class IceBlockInter : AbstractInteractableBehavior
    {
        #region fields

        [Header("Graph")]
        public Animation anim;
        public ParticleSystem liquefactionVFX;
        public ParticleSystem solidificationVFX;

        [Header("Meca")] [FieldCompletion] [SerializeField]
        private BoxCollider col;

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
            if (!col.enabled)
                return;

            GetComponent<BoxCollider>().enabled = false;
            col.enabled = false;
            anim.Play();
            liquefactionVFX.Play();
            Destroy(gameObject, Mathf.Max(anim.clip.length, liquefactionVFX.main.duration));
        }

        #endregion
    }
}