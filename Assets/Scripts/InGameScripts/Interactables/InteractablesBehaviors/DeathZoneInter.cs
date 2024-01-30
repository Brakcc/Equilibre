using InGameScripts.PlayerScripts.PlayerBehaviors;
using UnityEngine;

namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public sealed class DeathZoneInter : AbstractInteractableBehavior
    {
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                other.GetComponent<PlayerDeath>().OnDie();
        }
    }
}