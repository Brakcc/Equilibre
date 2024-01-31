using UnityEngine;
using Utilities.CustomAttributes;

//Git
namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public class InterrupterInter : AbstractInteractableBehavior, IActivator
    {
        #region fields

        [CheckerState] [SerializeField] private bool isStartingOn;
        
        public bool IsActivated { get => isActive; private set => isActive = value; }
        [SerializeField] private bool isActive;
        public DoorInter[] DoorRef { get => doorRef; set => doorRef = value; }
        [SerializeField] private DoorInter[] doorRef;
        
        [SerializeField] private float activationCooldown;
        
        private float _cooldownCounter;

        #endregion

        #region methodes

        private void Start()
        {
            IsActivated = isStartingOn;
            _cooldownCounter = Constants.SecuDeltaTimeOffset;
        }

        private void Update()
        {
            if (_cooldownCounter >= Constants.SecuDeltaTimeOffset)
                _cooldownCounter -= Time.deltaTime;
        }

        protected override void OnTriggerEnter(Collider other) {}

        public void OnActivatorAction()
        {
            if (_cooldownCounter >= 0)
                return;
            
            _cooldownCounter = activationCooldown;
            IsActivated = !IsActivated;
            if (DoorRef.Length == 0)
                return;
            foreach (var d in DoorRef)
            {
                d.OnActivatorChangeState();
            }
        }

        #endregion
        
    }
}