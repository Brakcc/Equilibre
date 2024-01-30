using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.CustomAttributes;

namespace InGameScripts.PlayerScripts.PlayerBehaviors
{
    public sealed class PlayerPower : AbstractPlayerBehavior
    {
        #region fields
        
        [FieldImportanceLevel] [SerializeField] private Collider iceCollider;
        [FieldImportanceLevel] [SerializeField] private Collider fireCollider;

        [FieldImportanceLevel] [SerializeField] private MeshRenderer iceRend;
        [FieldImportanceLevel] [SerializeField] private MeshRenderer fireRend;

        public bool unlockedFire;
        public bool unlockedIce;
        

        #endregion

        #region methodes

        protected override void OnStart()
        {
            base.OnStart();

            iceCollider.enabled = false;
            fireCollider.enabled = false;

            iceRend.enabled = false;
            fireRend.enabled = false;
        }

        public void OnActivateFire(InputAction.CallbackContext ctx)
        {
            if (iceCollider.enabled || !unlockedFire)
                return;
            
            fireCollider.enabled = ctx.performed;
            fireRend.enabled = ctx.performed;
        }

        public void OnActivateIce(InputAction.CallbackContext ctx)
        {
            if (fireCollider.enabled || !unlockedIce)
                return;
            
            iceCollider.enabled = ctx.performed;
            iceRend.enabled = ctx.performed;
        }

        #endregion
    }
}