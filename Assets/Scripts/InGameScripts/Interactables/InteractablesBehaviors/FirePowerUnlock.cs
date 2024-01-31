using InGameScripts.PlayerScripts.PlayerBehaviors;
using UnityEngine;

namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public class FirePowerUnlock : AbstractInteractableBehavior
    {
        #region methodes

        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            
            var p = other.GetComponent<PlayerPower>();
            p.hasFireUnlocked = true;
            Destroy(gameObject);
        }

        #endregion
    }
}