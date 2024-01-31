using System.Threading.Tasks;
using UnityEngine;
using Utilities.CustomAttributes;

//Git
namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public class DoorInter : AbstractInteractableBehavior
    {
        #region fields

        [InterfaceConstraint(typeof(IActivator))]
        [SerializeField] private AbstractInteractableBehavior[] activators;
        
        [SerializeField] private bool[] statesRefs;

        [Tooltip("En Millisecondes")]
        [SerializeField] private int doorCoolDown;
            
        private Collider _meshCol;
        private MeshRenderer _meshRend;

        #endregion

        #region methodes

        private async void Start()
        {
            _meshCol = GetComponent<Collider>();
            _meshRend = GetComponent<MeshRenderer>();
            await Task.Delay(Constants.DelayBeforeDoorAction);
            OnActivatorChangeState();
        }

        public async void OnActivatorChangeState()
        {
            await Task.Delay(doorCoolDown);
            OnAction();
        }

        protected override void OnAction()
        {
            var tempB = false;
            
            for (var i = 0; i < activators.Length; i++)
            {
                var tempActivator = (IActivator)activators[i];
                if (tempActivator.IsActivated == statesRefs[i])
                    continue;
                
                tempB = true;
                break;
            }

            _meshCol.enabled = tempB;
            _meshRend.enabled = tempB;
        }

        #endregion
    }
}