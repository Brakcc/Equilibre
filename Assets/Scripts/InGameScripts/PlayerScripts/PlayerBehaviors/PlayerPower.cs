using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.CustomAttributes;

//Git
namespace InGameScripts.PlayerScripts.PlayerBehaviors
{
    public class PlayerPower : AbstractPlayerBehavior
    {
        #region fields
        
        [FieldCompletion] [SerializeField] private Collider iceCollider;
        [FieldCompletion] [SerializeField] private Collider fireCollider;

        [FieldCompletion] [SerializeField] private MeshRenderer iceRend;
        [FieldCompletion] [SerializeField] private MeshRenderer fireRend;

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
            if (iceCollider.enabled)
                return;
            
            fireCollider.enabled = ctx.performed;
            fireRend.enabled = ctx.performed;
        }

        public void OnActivateIce(InputAction.CallbackContext ctx)
        {
            if (fireCollider.enabled)
                return;
            
            iceCollider.enabled = ctx.performed;
            iceRend.enabled = ctx.performed;
        }

        #endregion
    }
}