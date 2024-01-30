using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.CustomAttributes;

namespace InGameScripts.PlayerScripts.PlayerBehaviors
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMove : AbstractPlayerBehavior
    {
        #region fields

        [FieldImportanceLevel] [SerializeField] private InputActionReference move;
        
        [Space]
        [SerializeField] private float moveSpeed;
        [FieldColorLerp(0, 10), Range(0, 10), SerializeField] private float lerpCoef;
        private float _currentSpeed;
        private float _currentLerpCoef;
        private Vector3 _lastDir;
        
        #endregion

        #region methodes

        #region inherited

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            _currentLerpCoef = Mathf.Clamp(_currentLerpCoef, 0, 1);
            
            if (GetDir().magnitude <= Constants.MinimalMoveInputSecu)
                _currentLerpCoef -= lerpCoef * Time.deltaTime;
            
            else
                _currentLerpCoef += lerpCoef * Time.deltaTime;
            
            _currentSpeed = Mathf.Lerp(0, moveSpeed, _currentLerpCoef);
            Velocity = _lastDir * (_currentSpeed * Time.deltaTime);
        }

        #endregion

        #region checkers

        private Vector3 GetDir()
        {
            var newDir = move.action.ReadValue<Vector2>();
            
            if (newDir.magnitude >= Constants.MinimalMoveInputSecu)
                _lastDir = new Vector3(newDir.x, 0, newDir.y).normalized;
            
            return new Vector3(newDir.x, 0, newDir.y).normalized;
        }

        #endregion
        
        #endregion
    }
}