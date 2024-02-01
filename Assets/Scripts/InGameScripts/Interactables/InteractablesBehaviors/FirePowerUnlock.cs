using InGameScripts.PlayerScripts.PlayerBehaviors;
using UnityEngine;
using Utilities.CustomAttributes;

namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public class FirePowerUnlock : AbstractInteractableBehavior
    {
        #region fields

        [FieldCompletion] [SerializeField] private GameObject feedback;

        #endregion
        
        #region methodes

        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            
            var p = other.GetComponent<PlayerPower>();
            p.hasFireUnlocked = true;
            var goI = Instantiate(feedback, transform.position + Vector3.up * 5, Quaternion.identity);
            Destroy(gameObject);
        }

        #endregion
    }
}