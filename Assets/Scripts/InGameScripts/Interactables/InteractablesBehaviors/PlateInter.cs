using UnityEngine;

namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public class PlateInter : AbstractInteractableBehavior, IActivator
    {
        #region fields

        
        [SerializeField] private LayerMask activatorLayer;

        public bool IsActivated { get; private set; }
        [SerializeField] private DoorInter[] doorRef;
        public DoorInter[] DoorRef => doorRef;

        #endregion

        #region methodes

        private void Start()
        {
            IsActivated = false;
        }

        private void Update()
        {
            if (HasCubeOnIt() && !IsActivated)
            {
                OnActivatorAction();
            }
            else if (!HasCubeOnIt() && IsActivated)
            {
                OnActivatorAction();
            }
        }

        private bool HasCubeOnIt() => Physics.Raycast(transform.position + new Vector3(0.5f, -0.5f, -0.5f), Vector3.up, 1f, activatorLayer);

        #endregion

        
        public void OnActivatorAction()
        {
            IsActivated = !IsActivated;
            if (doorRef.Length == 0)
                return;
            
            doorRef[0].OnActivatorChangeState();
        }
    }
}