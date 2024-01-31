using InGameScripts.PlayerScripts.PlayerBehaviors;
using UnityEngine;

//Git
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