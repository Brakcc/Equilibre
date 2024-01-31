using UnityEngine;
using Utilities.CustomAttributes;

namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public sealed class TorchInter : AbstractInteractableBehavior, IActivator
    {
        #region fields

        [SerializeField] private MeshRenderer fireRend;
        [SerializeField] private MeshRenderer externFire;
        [SerializeField] private new Light light;
        [CheckerState] public bool isOnFire;

        public bool IsActivated { get; private set; }
        [SerializeField] private DoorInter doorRef;
        public DoorInter DoorRef { get => doorRef; private set => doorRef = value; }

        #endregion

        #region methodes

        private void Start()
        {
            fireRend.enabled = isOnFire;
            externFire.enabled = isOnFire;
            light.intensity = GetLightIntensity(isOnFire);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("FirePower"))
                OnAction(true);
            
            if (other.CompareTag("IcePower"))
                OnAction(false);
        }

        protected override void OnAction(bool active)
        {
            isOnFire = active;
            fireRend.enabled = active;
            externFire.enabled = active;
            light.intensity = GetLightIntensity(active);
            IsActivated = isOnFire;
            DoorRef.OnActivatorChangeState();
        }

        public void OnActivatorAction() {}

        private static int GetLightIntensity(bool isOn) => isOn ? 5 : 0;

        #endregion
    }
}