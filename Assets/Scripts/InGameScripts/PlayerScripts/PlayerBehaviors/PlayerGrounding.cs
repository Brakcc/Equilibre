using UnityEngine;
using Utilities.CustomAttributes;

namespace InGameScripts.PlayerScripts.PlayerBehaviors
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerGrounding : AbstractPlayerBehavior
    {
        #region fields
        
        [FieldImportanceLevel] [SerializeField] private CharacterController _charaCont;
        [Space]
        
        [FieldColorLerp(0, 1), Range(0, 1), SerializeField] private float lerpCoef;
        [SerializeField] private float maxAccelCoef;
        [Space]
        [FieldColorLerp(-1, 1), Range(-1, 1), SerializeField] private float sideRayCastOffset;
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
            
            var r1 = Physics.Raycast(position + new Vector3(_charaCont.radius - sideRayCastOffset, 0, 0), Vector3.down,  
                1 + Constants.MaxStepHeight, groundLayer);
            
            var r2 = Physics.Raycast(position + new Vector3(-_charaCont.radius + sideRayCastOffset, 0, 0), Vector3.down,
                1 + Constants.MaxStepHeight, groundLayer);
           
            var r3 = Physics.Raycast(position + new Vector3(0, 0, _charaCont.radius - sideRayCastOffset), Vector3.down,
                1 + Constants.MaxStepHeight, groundLayer);
            
            var r4 = Physics.Raycast(position + new Vector3(0, 0, -_charaCont.radius + sideRayCastOffset), Vector3.down,
                1 + Constants.MaxStepHeight, groundLayer);
            
            var r5 = Physics.Raycast(position, Vector3.down, 1 + Constants.MaxStepHeight, groundLayer);
            
            return r1 || r2 || r3 || r4 || r5;
        }

        private bool IsSideRayHit(out Vector3 pushDir)
        {
            var position = transform.position;
            pushDir = default;
            
            var c1 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                Vector3.forward, _charaCont.radius, groundLayer);
            if (c1) pushDir = -Vector3.forward;
                
            var c2 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                Vector3.right, _charaCont.radius, groundLayer);
            if (c2) pushDir = -Vector3.right;
            
            var c3 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                Vector3.left, _charaCont.radius, groundLayer);
            if (c3) pushDir = -Vector3.left;
            
            var c4 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                Vector3.back, _charaCont.radius, groundLayer);
            if (c4) pushDir = -Vector3.back;
            
            var c5 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                new Vector3(1, 0, 1).normalized, _charaCont.radius, groundLayer);
            if (c5) pushDir = -new Vector3(1, 0, 1).normalized;
            
            var c6 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                new Vector3(1, 0, -1).normalized, _charaCont.radius, groundLayer);
            if (c6) pushDir = -new Vector3(1, 0, -1).normalized;
            
            var c7 = Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                new Vector3(-1, 0, 1).normalized, _charaCont.radius, groundLayer);
            if (c7) pushDir = -new Vector3(-1, 0, 1).normalized;
            
            var c8= Physics.Raycast(position - new Vector3(0, _charaCont.height / 2, 0), 
                new Vector3(-1, 0, -1).normalized, _charaCont.radius, groundLayer);
            if (c8) pushDir = -new Vector3(-1, 0, -1).normalized;
            
            return c1 || c2 || c3 || c4 || c5 || c6 || c7 || c8;
        }

        #endregion
        
        private void OnDrawGizmos()
        {
            var position = transform.position;
            var radius = _charaCont.radius;
            var height = _charaCont.height;
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position + new Vector3(radius - sideRayCastOffset, 0, 0), 
                position + new Vector3(radius - sideRayCastOffset, 0, 0) + Vector3.down * (1 + Constants.MaxStepHeight));
            
            Gizmos.DrawLine(position + new Vector3(-radius + sideRayCastOffset, 0, 0), 
                position + new Vector3(-radius + sideRayCastOffset, 0, 0) + Vector3.down * (1 + Constants.MaxStepHeight));
            
            Gizmos.DrawLine(position + new Vector3(0, 0, radius - sideRayCastOffset), 
                position + new Vector3(0, 0, radius - sideRayCastOffset) + Vector3.down * (1 + Constants.MaxStepHeight));
            
            Gizmos.DrawLine(position + new Vector3(0, 0, -radius + sideRayCastOffset), 
                position + new Vector3(0, 0, -radius + sideRayCastOffset) + Vector3.down * (1 + Constants.MaxStepHeight));
            
            Gizmos.DrawLine(position, position + Vector3.down * (1 + Constants.MaxStepHeight));
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + Vector3.forward * radius);
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + Vector3.right * radius);
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + Vector3.left * radius);
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + Vector3.back * radius);
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + new Vector3(1, 0, 1).normalized * radius);
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + new Vector3(1, 0, -1).normalized * radius);
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + new Vector3(-1, 0, 1).normalized * radius);
            
            Gizmos.DrawLine(position - new Vector3(0, height / 2, 0),
                position - new Vector3(0, height / 2, 0) + new Vector3(-1, 0, -1).normalized * radius);
        }

        #endregion
    }
}