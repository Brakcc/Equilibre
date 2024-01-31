using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

//Git
namespace InGameScripts.PlayerScripts.PlayerBehaviors
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMove : AbstractPlayerBehavior
    {
        #region fields

        [FieldCompletion] [SerializeField] private InputActionReference move;
        [FieldCompletion] [SerializeField] private CharacterController _charaCont;
        
        [Space]
        [SerializeField] private float moveSpeed;
        [FieldColorLerp(0, 10), Range(0, 10), SerializeField] private float lerpCoef;
        
        [Space]
        [Header("HitAndSlip Parameters")]
        [FieldColorLerp(0, 1), Range(0, 1), SerializeField] private float bottomHorizRayCastOffset;
        [FieldColorLerp(0, 1), Range(0, 1), SerializeField] private float sideHorizRayCastOffset;
        [FieldColorLerp(FieldColor.Cyan, FieldColor.Blue, 0, 1), Range(0, 1), SerializeField] private float sideHorizRayCastHeight;
        [FieldColorLerp(0, 1), Range(0, 1), SerializeField] private float minHitAndSlipVel;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask iceLayer;
        
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

            var tempDir = GetDir();

            if (!IsOnIce())
            {
                if (tempDir.magnitude <= Constants.MinimalMoveInputSecu)
                    _currentLerpCoef -= lerpCoef * Time.deltaTime;

                else
                    _currentLerpCoef += lerpCoef * Time.deltaTime;

                _currentSpeed = Mathf.Lerp(0, moveSpeed, _currentLerpCoef);
            }

            if (IsSlipHit(out var hitDir))
            {
                if (IsDotted(hitDir))
                {
                    var tempAngle = Mathf.Acos(Vector3.Dot(_lastDir, -hitDir) / (_lastDir.magnitude * hitDir.magnitude)) * Mathf.Rad2Deg;

                    if (tempAngle <= Constants.MaxSlipContactAngle)
                    {
                        var tempVec = Vector3.Cross(_lastDir, -hitDir);
                        var tempCrossedVec = Vector3.Cross(-hitDir, tempVec.normalized);
                        _lastDir = tempCrossedVec.normalized * Mathf.Lerp(minHitAndSlipVel, tempCrossedVec.magnitude, tempVec.magnitude);
                    }
                }
            }

            if (IsOnIce())
            {
                
                if (tempDir.magnitude <= Constants.MinimalMoveInputSecu)
                    _currentLerpCoef -= lerpCoef * Time.deltaTime / 10;

                else
                    _currentLerpCoef += lerpCoef * Time.deltaTime / 10;

                _currentSpeed = Mathf.Lerp(0, moveSpeed, _currentLerpCoef);
            }
            
            Velocity = _lastDir * (_currentSpeed * Time.deltaTime);
        }

        #endregion

        #region checkers

        private Vector3 GetDir()
        {
            var newDir = move.action.ReadValue<Vector2>();
            
            if (newDir.magnitude >= Constants.MinimalMoveInputSecu)
                _lastDir = new Vector3(newDir.x, 0, newDir.y).normalized;
            
            return new Vector3(newDir.x, 0, newDir.y);
        }

        private static bool IsDotted(Vector3 vec) => Vector3.Dot(vec, Vector3.forward) <= Constants.MinDotSecuVal ||
                                                     Vector3.Dot(vec, Vector3.right) <= Constants.MinDotSecuVal ||
                                                     Vector3.Dot(vec, Vector3.left) <= Constants.MinDotSecuVal ||
                                                     Vector3.Dot(vec, Vector3.back) <= Constants.MinDotSecuVal ||
                                                     Vector3.Dot(vec, new Vector3(1, 0, 1).normalized) <= Constants.MinDotSecuVal ||
                                                     Vector3.Dot(vec, new Vector3(-1, 0, 1).normalized) <= Constants.MinDotSecuVal ||
                                                     Vector3.Dot(vec, new Vector3(1, 0, -1).normalized) <= Constants.MinDotSecuVal ||
                                                     Vector3.Dot(vec, new Vector3(-1, 0, -1).normalized) <= Constants.MinDotSecuVal;

        private bool IsOnIce() => Physics.Raycast(transform.position, Vector3.down, _charaCont.height / 2 + Constants.MaxStepHeight, iceLayer);
        
        private bool IsSlipHit(out Vector3 hitDir)
        {
            hitDir = default;
            
            var position = transform.position;
            var tempHitInfo = new List<RaycastHit>();
            
            //Bottom RayCasts
            var c1 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                Vector3.forward, out var hit1, _charaCont.radius + bottomHorizRayCastOffset, groundLayer);
            if (c1) tempHitInfo.Add(hit1);
                
            var c2 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                Vector3.right, out var hit2, _charaCont.radius + bottomHorizRayCastOffset, groundLayer);
            if (c2) tempHitInfo.Add(hit2);
            
            var c3 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                Vector3.left, out var hit3, _charaCont.radius + bottomHorizRayCastOffset, groundLayer);
            if (c3) tempHitInfo.Add(hit3);
            
            var c4 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                Vector3.back, out var hit4, _charaCont.radius + bottomHorizRayCastOffset, groundLayer);
            if (c4) tempHitInfo.Add(hit4);
            
            var c5 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                new Vector3(1, 0, 1).normalized, out var hit5, _charaCont.radius + bottomHorizRayCastOffset, groundLayer);
            if (c5) tempHitInfo.Add(hit5);
            
            var c6 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                new Vector3(1, 0, -1).normalized, out var hit6, _charaCont.radius + bottomHorizRayCastOffset, groundLayer);
            if (c6) tempHitInfo.Add(hit6);
            
            var c7 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                new Vector3(-1, 0, 1).normalized, out var hit7, _charaCont.radius + bottomHorizRayCastOffset, groundLayer);
            if (c7) tempHitInfo.Add(hit7);
            
            var c8= Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                new Vector3(-1, 0, -1).normalized, out var hit8, _charaCont.radius + bottomHorizRayCastOffset, groundLayer);
            if (c8) tempHitInfo.Add(hit8);
            
            //Middle RayCasts
            var s1 = Physics.Raycast(position - new Vector3(0, sideHorizRayCastHeight, 0), 
                Vector3.forward, out var shit1, _charaCont.radius + sideHorizRayCastOffset, groundLayer);
            if (s1) tempHitInfo.Add(shit1);
                
            var s2 = Physics.Raycast(position - new Vector3(0, sideHorizRayCastHeight, 0), 
                Vector3.right, out var shit2, _charaCont.radius + sideHorizRayCastOffset, groundLayer);
            if (s2) tempHitInfo.Add(shit2);
            
            var s3 = Physics.Raycast(position - new Vector3(0, sideHorizRayCastHeight, 0), 
                Vector3.left, out var shit3, _charaCont.radius + sideHorizRayCastOffset, groundLayer);
            if (s3) tempHitInfo.Add(shit3);
            
            var s4 = Physics.Raycast(position - new Vector3(0, sideHorizRayCastHeight, 0), 
                Vector3.back, out var shit4, _charaCont.radius + sideHorizRayCastOffset, groundLayer);
            if (s4) tempHitInfo.Add(shit4);
            
            var s5 = Physics.Raycast(position - new Vector3(0, sideHorizRayCastHeight, 0), 
                new Vector3(1, 0, 1).normalized, out var shit5, _charaCont.radius + sideHorizRayCastOffset, groundLayer);
            if (s5) tempHitInfo.Add(shit5);
            
            var s6 = Physics.Raycast(position - new Vector3(0, sideHorizRayCastHeight, 0), 
                new Vector3(1, 0, -1).normalized, out var shit6, _charaCont.radius + sideHorizRayCastOffset, groundLayer);
            if (s6) tempHitInfo.Add(shit6);
            
            var s7 = Physics.Raycast(position - new Vector3(0, sideHorizRayCastHeight, 0), 
                new Vector3(-1, 0, 1).normalized, out var shit7, _charaCont.radius + sideHorizRayCastOffset, groundLayer);
            if (s7) tempHitInfo.Add(shit7);
            
            var s8= Physics.Raycast(position - new Vector3(0, sideHorizRayCastHeight, 0), 
                new Vector3(-1, 0, -1).normalized, out var shit8, _charaCont.radius + sideHorizRayCastOffset, groundLayer);
            if (s8) tempHitInfo.Add(shit8);

            if (tempHitInfo.Count == 0)
                return (c1 || c2 || c3 || c4 || c5 || c6 || c7 || c8) && (s1 || s2 || s3 || s4 || s5 || s6 || s7 || s8);

            var tempMag = Vector3.one;
            foreach (var hit in tempHitInfo)
            {
                if (hit.distance < tempMag.magnitude)
                    hitDir = hit.normal;
            }

            return (c1 || c2 || c3 || c4 || c5 || c6 || c7 || c8) && (s1 || s2 || s3 || s4 || s5 || s6 || s7 || s8);
        }
        
        #endregion

        private void OnDrawGizmos()
        {
            var position = transform.position;
            var height = _charaCont.height;
            var radius = _charaCont.radius;
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + Vector3.forward * (radius + bottomHorizRayCastOffset));
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + Vector3.right * (radius + bottomHorizRayCastOffset));
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + Vector3.left * (radius + bottomHorizRayCastOffset));
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + Vector3.back * (radius + bottomHorizRayCastOffset));
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + new Vector3(1, 0, 1).normalized * (radius + bottomHorizRayCastOffset));
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + new Vector3(1, 0, -1).normalized * (radius + bottomHorizRayCastOffset));
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + new Vector3(-1, 0, 1).normalized * (radius + bottomHorizRayCastOffset));
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + new Vector3(-1, 0, -1).normalized * (radius + bottomHorizRayCastOffset));
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(position - new Vector3(0, sideHorizRayCastHeight, 0),
                position - new Vector3(0, sideHorizRayCastHeight, 0) + Vector3.forward * (radius + sideHorizRayCastOffset));
            
            Gizmos.DrawLine(position - new Vector3(0, sideHorizRayCastHeight, 0),
                position - new Vector3(0, sideHorizRayCastHeight, 0) + Vector3.right * (radius + sideHorizRayCastOffset));
            
            Gizmos.DrawLine(position - new Vector3(0, sideHorizRayCastHeight, 0),
                position - new Vector3(0, sideHorizRayCastHeight, 0) + Vector3.left * (radius + sideHorizRayCastOffset));
            
            Gizmos.DrawLine(position - new Vector3(0, sideHorizRayCastHeight, 0),
                position - new Vector3(0, sideHorizRayCastHeight, 0) + Vector3.back * (radius + sideHorizRayCastOffset));
            
            Gizmos.DrawLine(position - new Vector3(0, sideHorizRayCastHeight, 0),
                position - new Vector3(0, sideHorizRayCastHeight, 0) + new Vector3(1, 0, 1).normalized * (radius + sideHorizRayCastOffset));
            
            Gizmos.DrawLine(position - new Vector3(0, sideHorizRayCastHeight, 0),
                position - new Vector3(0, sideHorizRayCastHeight, 0) + new Vector3(1, 0, -1).normalized * (radius + sideHorizRayCastOffset));
            
            Gizmos.DrawLine(position - new Vector3(0, sideHorizRayCastHeight, 0),
                position - new Vector3(0, sideHorizRayCastHeight, 0) + new Vector3(-1, 0, 1).normalized * (radius + sideHorizRayCastOffset));
            
            Gizmos.DrawLine(position - new Vector3(0, sideHorizRayCastHeight, 0),
                position - new Vector3(0, sideHorizRayCastHeight, 0) + new Vector3(-1, 0, -1).normalized * (radius + sideHorizRayCastOffset));
        }

        #endregion
    }
}