using System.Collections.Generic;
using UnityEngine;
using Utilities.CustomAttributes;

//Git
namespace InGameScripts.PlayerScripts.PlayerBehaviors
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerGrounding : AbstractPlayerBehavior
    {
        #region fields
        
        [FieldCompletion] [SerializeField] private CharacterController _charaCont;
        [Space]
        
        [FieldColorLerp(0, 1), Range(0, 1), SerializeField] private float lerpCoef;
        [SerializeField] private float maxAccelCoef;
        [Space]
        [FieldColorLerp(-1, 1), Range(-1, 1), SerializeField] private float sideDownRayCastOffset;
        [SerializeField] private LayerMask groundLayer;
        
        private float _currentAccelCoef;
        private float _currentLerpCoef;
        private bool wasOnEdge;

        #endregion

        #region methodes

        protected override void OnStart()
        {
            base.OnStart();
            _charaCont = GetComponent<CharacterController>();
        }

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            
            if (SimpleHit() && IsDownRayHit())
            {
                _currentLerpCoef = 0;
                _currentAccelCoef = -5;
                wasOnEdge = false;
                
                Velocity = new Vector3(0, _currentAccelCoef, 0);
            }
            else if (!SimpleHit() && IsDownRayHit())
            {
                _currentLerpCoef = 0;
                _currentAccelCoef = -5;
                wasOnEdge = true;
                
                Velocity = new Vector3(0, _currentAccelCoef, 0);
            }
            else if (IsSideRayHit(out var push) && !IsDownRayHit())
            {
                _currentLerpCoef = 0;
                _currentAccelCoef = 0;
                wasOnEdge = true;
                
                Velocity = new Vector3(0, _currentAccelCoef, 0) + push * Time.deltaTime;
            }
            else
            {
                if (wasOnEdge)
                {
                    _currentAccelCoef = 0;
                    wasOnEdge = false;
                }
                _currentLerpCoef += lerpCoef * Time.deltaTime;
                _currentAccelCoef = Mathf.Lerp(0, maxAccelCoef, _currentLerpCoef);
                Velocity = new Vector3(0, _currentAccelCoef, 0);
            }
        }

        #region checkers

        private bool SimpleHit() => Physics.Raycast(transform.position, Vector3.down, 1 + Constants.MaxStepHeight, groundLayer);

        private bool IsDownRayHit()
        {
            var position = transform.position;
            
            var r1 = Physics.Raycast(position + new Vector3(_charaCont.radius - sideDownRayCastOffset, 0, 0), Vector3.down,  
                1 + Constants.MaxStepHeight, groundLayer);
            
            var r2 = Physics.Raycast(position + new Vector3(-_charaCont.radius + sideDownRayCastOffset, 0, 0), Vector3.down,
                1 + Constants.MaxStepHeight, groundLayer);
           
            var r3 = Physics.Raycast(position + new Vector3(0, 0, _charaCont.radius - sideDownRayCastOffset), Vector3.down,
                1 + Constants.MaxStepHeight, groundLayer);
            
            var r4 = Physics.Raycast(position + new Vector3(0, 0, -_charaCont.radius + sideDownRayCastOffset), Vector3.down,
                1 + Constants.MaxStepHeight, groundLayer);
            
            var r5 = Physics.Raycast(position, Vector3.down, 1 + Constants.MaxStepHeight, groundLayer);
            
            return r1 || r2 || r3 || r4 || r5;
        }

        private bool IsSideRayHit(out Vector3 pushDir)
        {
            pushDir = default;
            
            var position = transform.position;
            var tempHitInfo = new List<RaycastHit>();
            
            var c1 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                Vector3.forward, out var hit1, _charaCont.radius, groundLayer);
            if (c1) tempHitInfo.Add(hit1);
                
            var c2 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                Vector3.right, out var hit2, _charaCont.radius, groundLayer);
            if (c2) tempHitInfo.Add(hit2);
            
            var c3 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                Vector3.left, out var hit3, _charaCont.radius, groundLayer);
            if (c3) tempHitInfo.Add(hit3);
            
            var c4 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                Vector3.back, out var hit4, _charaCont.radius, groundLayer);
            if (c4) tempHitInfo.Add(hit4);
            
            var c5 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                new Vector3(1, 0, 1).normalized, out var hit5, _charaCont.radius, groundLayer);
            if (c5) tempHitInfo.Add(hit5);
            
            var c6 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                new Vector3(1, 0, -1).normalized, out var hit6, _charaCont.radius, groundLayer);
            if (c6) tempHitInfo.Add(hit6);
            
            var c7 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                new Vector3(-1, 0, 1).normalized, out var hit7, _charaCont.radius, groundLayer);
            if (c7) tempHitInfo.Add(hit7);
            
            var c8= Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                new Vector3(-1, 0, -1).normalized, out var hit8, _charaCont.radius, groundLayer);
            if (c8) tempHitInfo.Add(hit8);

            if (tempHitInfo.Count == 0)
                return c1 || c2 || c3 || c4 || c5 || c6 || c7 || c8;

            var tempMag = Vector3.one;
            foreach (var hit in tempHitInfo)
            {
                if (hit.distance < tempMag.magnitude)
                    pushDir = hit.normal;
            }

            return c1 || c2 || c3 || c4 || c5 || c6 || c7 || c8;
        }
        
        #endregion
        
        private void OnDrawGizmos()
        {
            var position = transform.position;
            var radius = _charaCont.radius;
            var height = _charaCont.height;
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position + new Vector3(radius - sideDownRayCastOffset, 0, 0), 
                position + new Vector3(radius - sideDownRayCastOffset, 0, 0) + Vector3.down * (1 + Constants.MaxStepHeight));
            
            Gizmos.DrawLine(position + new Vector3(-radius + sideDownRayCastOffset, 0, 0), 
                position + new Vector3(-radius + sideDownRayCastOffset, 0, 0) + Vector3.down * (1 + Constants.MaxStepHeight));
            
            Gizmos.DrawLine(position + new Vector3(0, 0, radius - sideDownRayCastOffset), 
                position + new Vector3(0, 0, radius - sideDownRayCastOffset) + Vector3.down * (1 + Constants.MaxStepHeight));
            
            Gizmos.DrawLine(position + new Vector3(0, 0, -radius + sideDownRayCastOffset), 
                position + new Vector3(0, 0, -radius + sideDownRayCastOffset) + Vector3.down * (1 + Constants.MaxStepHeight));
            
            Gizmos.DrawLine(position, position + Vector3.down * (1 + Constants.MaxStepHeight));
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2 - Constants.VirtualGraphRayCastOriginOffset, 0),
                position - new Vector3(0, height / 2, 0) + Vector3.forward * radius);
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2 - Constants.VirtualGraphRayCastOriginOffset, 0),
                position - new Vector3(0, height / 2, 0) + Vector3.right * radius);
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2 - Constants.VirtualGraphRayCastOriginOffset, 0),
                position - new Vector3(0, height / 2, 0) + Vector3.left * radius);
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2 - Constants.VirtualGraphRayCastOriginOffset, 0),
                position - new Vector3(0, height / 2, 0) + Vector3.back * radius);
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2 - Constants.VirtualGraphRayCastOriginOffset, 0),
                position - new Vector3(0, height / 2, 0) + new Vector3(1, 0, 1).normalized * radius);
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2 - Constants.VirtualGraphRayCastOriginOffset, 0),
                position - new Vector3(0, height / 2, 0) + new Vector3(1, 0, -1).normalized * radius);
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2 - Constants.VirtualGraphRayCastOriginOffset, 0),
                position - new Vector3(0, height / 2, 0) + new Vector3(-1, 0, 1).normalized * radius);
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2 - Constants.VirtualGraphRayCastOriginOffset, 0),
                position - new Vector3(0, height / 2, 0) + new Vector3(-1, 0, -1).normalized * radius);
        }

        #endregion
    }
}