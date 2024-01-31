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

        [FieldCompletion] [SerializeField] private ParticleSystem iceParts;
        [FieldCompletion] [SerializeField] private ParticleSystem fireParts;

        public bool hasFireUnlocked;
        
        #endregion

        #region methodes

        protected override void OnStart()
        {
            base.OnStart();

            hasFireUnlocked = false;
            
            iceCollider.enabled = false;
            fireCollider.enabled = false;

            //iceRend.enabled = false;
            //fireRend.enabled = false;

            iceParts.Stop();
            fireParts.Stop();
        }

        public void OnActivateFire(InputAction.CallbackContext ctx)
        {
            if (iceCollider.enabled)
                return;
            
            if (!hasFireUnlocked)
                return;
            
            fireCollider.enabled = ctx.performed;
            //fireRend.enabled = ctx.performed;
            if (ctx.performed)
                fireParts.Play();
            else
                fireParts.Stop();
        }

        public void OnActivateIce(InputAction.CallbackContext ctx)
        {
            if (fireCollider.enabled)
                return;
            
            iceCollider.enabled = ctx.performed;
            //iceRend.enabled = ctx.performed;
            if (ctx.performed)
                iceParts.Play();
            else
            {
                iceParts.Stop();
                iceParts.Clear();
            }
        }

        #endregion
    }
}