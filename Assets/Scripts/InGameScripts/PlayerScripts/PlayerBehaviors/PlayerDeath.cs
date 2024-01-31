using System.Threading.Tasks;
using UnityEngine;

//Git
namespace InGameScripts.PlayerScripts.PlayerBehaviors
{
    public sealed class PlayerDeath : AbstractPlayerBehavior
    {
        #region fields

        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private int timeBeforeRespawn;
        [SerializeField] private int timeBeforeControl;
        
        private Vector3 _lastPos;
        private CharacterController _charaCont;
        
        [HideInInspector] public bool isDed;

        #endregion

        #region mathodes

        protected override void OnStart()
        {
            base.OnStart();
            _charaCont = GetComponent<CharacterController>();
        }

        protected override void OnUpdate()
        {
            if (IsOnGround(out var hit))
                _lastPos = hit.point + new Vector3(0, _charaCont.height / 2 + Constants.RespawnOffsetHeight, 0);
        }

        internal async void OnDie()
        {
            isDed = true;
            
            await Task.Delay(timeBeforeRespawn);

            OnRespawn();
        }

        private async void OnRespawn()
        {
            transform.position = _lastPos;

            await Task.Delay(timeBeforeControl);
            
            isDed = false;
        }

        private bool IsOnGround(out RaycastHit rayHit)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out var hit,
                    _charaCont.height / 2 + Constants.MaxStepHeight, groundLayer))
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