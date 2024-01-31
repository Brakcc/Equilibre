﻿using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

//Git
namespace InGameScripts.PlayerScripts.PlayerBehaviors
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerRotation : AbstractPlayerBehavior
    {
        #region fields

        [FieldCompletion] [SerializeField] private InputActionReference move;
        
        [Space]
        [FieldColorLerp(FieldColor.Orange, FieldColor.Cyan, 1, 10)] [Range(1, 10)] [SerializeField] private float lerpCoef;

        private Vector3 _lastDir;
        private Vector3 _currentLookDir;
        private PlayerDeath _death;

        #endregion

        #region methodes

        protected override void OnStart()
        {
            _lastDir = Vector3.forward;
            _currentLookDir = Vector3.forward;
            _death = GetComponent<PlayerDeath>();
        }

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            GetDir();

            if (_death.isDed)
                return;
            
            var angle = Vector3.Dot(_lastDir, _currentLookDir) / (_currentLookDir.magnitude * _lastDir.magnitude);
            if (Mathf.Acos(angle) > Constants.MinPlayerRotationAngle)
            {
                _currentLookDir = Vector3.MoveTowards(_currentLookDir, _lastDir, lerpCoef * Time.deltaTime);
            }
            
            transform.rotation = Quaternion.LookRotation(_currentLookDir);
        }
        
        private void GetDir()
        {
            var newDir = move.action.ReadValue<Vector2>();
            
            if (newDir.magnitude >= Constants.MinimalRotaInputSecu)
                _lastDir = new Vector3(newDir.x, 0, newDir.y).normalized;
        }

        #endregion
    }
}