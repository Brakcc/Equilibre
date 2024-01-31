using InGameScripts.Interactables.InteractablesBehaviors;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.CustomAttributes;

namespace InGameScripts.PlayerScripts.PlayerBehaviors
{
    public sealed class PlayerPush : AbstractPlayerBehavior
    {
        #region fields

        [FieldCompletion] [SerializeField] private InputActionReference pushAndClick;
        [FieldCompletion] [SerializeField] private InputActionReference move;
        [FieldColorLerp(0, 5)] [Range(0, 5)] [SerializeField] private float raySize;
        [SerializeField] private LayerMask activatorLayer;

        private IActivator _currentActivatorInRange;
        private Vector3 lastDir;

        #endregion

        #region methodes

        protected override void OnStart()
        {
            _currentActivatorInRange = null;
        }

        protected override void OnUpdate()
        {
            if (pushAndClick.action.WasPressedThisFrame() && _currentActivatorInRange != null)
            {
                _currentActivatorInRange.OnActivatorAction();
            }
            
            if (HasActivatorInRange(out var hit) && _currentActivatorInRange == null)
                _currentActivatorInRange = hit.transform.GetComponent<IActivator>();
            
            else if (!HasActivatorInRange(out _))
                _currentActivatorInRange = null;
        }

        private bool HasActivatorInRange(out RaycastHit rayHit)
        {
            var dir = new Vector3(move.action.ReadValue<Vector2>().x, 0, move.action.ReadValue<Vector2>().y);
            if (dir.magnitude >= Constants.MinimalMoveInputSecu)
                lastDir = dir;
            
            if (Physics.Raycast(transform.position, lastDir.normalized, out var hit, raySize, activatorLayer))
            {
                rayHit = hit;
                return true;
            }

            rayHit = default;
            return false;
        }

        #endregion
    }
}