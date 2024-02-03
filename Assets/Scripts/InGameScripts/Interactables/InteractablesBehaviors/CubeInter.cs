using UnityEngine;
using UnityEngine.Serialization;
using Utilities.CustomAttributes;

namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public class CubeInter : AbstractInteractableBehavior, IActivator    
    {
        #region fields

        [SerializeField] private GameObject cubeRef;
        
        public bool IsActivated { get; private set; }
        public DoorInter[] DoorRef => null;

        [FieldColorLerp(0.001f, 0.1f)] [Range(0.001f, 0.1f)] [SerializeField] private float sideRayOffset;
        [SerializeField] private LayerMask fullLayer;
        [FormerlySerializedAs("playerMask")] [SerializeField] private LayerMask playerLayer;

        private CubeDir _currentCubeDir;
        private Vector3 _currentMoveDir;
        private GameObject _currentPlayer;
        private Vector3 _originPos;
        
        #endregion
        
        #region methodes

        private void Start()
        {
            _currentPlayer = null;
            IsActivated = false;
            _currentMoveDir = Vector3.zero;
            _currentCubeDir = default;
            _originPos = cubeRef.transform.position;
        }

        private void Update()
        {
            if (Physics.Raycast(cubeRef.transform.position, Vector3.down, out var hit, 5f, fullLayer))
            {
                if (hit.transform.gameObject.layer == 10)
                {
                    IsActivated = false;
                    _currentMoveDir = Vector3.zero;
                    _currentCubeDir = default;
                    cubeRef.transform.position = _originPos;
                }
            }

            if (!IsActivated || _currentCubeDir == CubeDir.Default)
                HasPlayer();
            
            OnMoveCube();
        }

        #region Triggers
        
        protected override void OnTriggerEnter(Collider other) {}
        
        #endregion

        public void OnActivatorAction()
        {
            if (IsActivated && _currentMoveDir != Vector3.zero)
                return;
            
            _currentMoveDir = GetDir(_currentCubeDir);
            IsActivated = true;
        }

        private void OnMoveCube()
        {
            if (!IsActivated)
                return;

            //Debug.Log($"{_currentCubeDir} + {_currentMoveDir}");
            
            if (IsWallHitting(_currentCubeDir, out var wall))
            {
                if (wall.transform.gameObject.layer is 6 or 8 or 9)
                {
                    IsActivated = false;
                    _currentMoveDir = Vector3.zero;
                    _currentCubeDir = default;
                }
            }
            
            if (IsHittingGround(_currentCubeDir, out var hit, out _))
            {
                if (hit.transform.gameObject.layer is 6 or 8 or 9 /*|| hitSecu.transform.gameObject.layer is 6 or 8 or 9*/)
                {
                    IsActivated = false;
                    _currentMoveDir = Vector3.zero;
                    _currentCubeDir = default;
                }
            }
            
            cubeRef.transform.position += _currentMoveDir * (Time.deltaTime * 4);
        }

        #region rays

        private bool IsHittingGround(CubeDir dir, out RaycastHit ray, out RaycastHit raySecu)
        {
            ray = default;
            raySecu = default;
            
            var pos = cubeRef.transform.position;
            
            var r1 = Physics.Raycast(pos + new Vector3(0, 0, Constants.HalfCubeSizeConsideration + sideRayOffset),
                Vector3.down, out var hit1, 5,
                fullLayer);
            // var l1 = Physics.Raycast(pos + new Vector3(0, 0, Constants.HalfCubeSizeConsideration + sideRayOffset + 0.1f),
            //     Vector3.down, out var hit10, 5,
            //     fullLayer);
            
            var r2 = Physics.Raycast(pos + new Vector3(Constants.HalfCubeSizeConsideration + sideRayOffset, 0, 0),
                Vector3.down, out var hit2, 5,
                fullLayer);
            //var l2 = Physics.Raycast(pos + new Vector3(Constants.HalfCubeSizeConsideration + sideRayOffset + 0.1f, 0, 0),
            //    Vector3.down, out var hit20, 5,
            //    fullLayer);
            
            var r3 = Physics.Raycast(pos + new Vector3(0, 0, -(Constants.HalfCubeSizeConsideration + sideRayOffset)),
                Vector3.down, out var hit3, 5,
                fullLayer);
            //var l3 = Physics.Raycast(pos + new Vector3(0, 0, -(Constants.HalfCubeSizeConsideration + sideRayOffset + 0.1f)),
            //    Vector3.down, out var hit30, 5,
            //    fullLayer);
            
            var r4 = Physics.Raycast(pos + new Vector3(-(Constants.HalfCubeSizeConsideration + sideRayOffset), 0, 0),
                Vector3.down, out var hit4, 5,
                fullLayer);
            //var l4 = Physics.Raycast(pos + new Vector3(-(Constants.HalfCubeSizeConsideration + sideRayOffset + 0.1f), 0, 0),
            //    Vector3.down, out var hit40, 5,
            //    fullLayer);
            
            switch (dir)
            {
                case CubeDir.North :
                    ray = hit1;
                    //raySecu = hit10;
                    return r1 /*|| l1*/;

                case CubeDir.East :
                    ray = hit2;
                    //raySecu = hit20;
                    return r2 /*|| l2*/;

                case CubeDir.South :
                    ray = hit3;
                    //raySecu = hit30;
                    return r3 /*|| l3*/;

                case CubeDir.West :
                    ray = hit4;
                    //raySecu = hit40;
                    return r4 /*|| l4*/;

                default :
                    ray = default;
                    //raySecu = default;
                    return false;
            }
        }

        private bool IsWallHitting(CubeDir dir, out RaycastHit ray)
        {
            ray = default;
            var pos = cubeRef.transform.position;
            switch (dir)
            {
                case CubeDir.North :
                    var r1 = Physics.Raycast(pos + Vector3.forward * 0.51f,
                        Vector3.forward, out var hit1, 0.15f,
                        fullLayer);
                    ray = hit1;
                    return r1;

                case CubeDir.East :
                    var r2 = Physics.Raycast(pos + Vector3.right * 0.51f,
                        Vector3.right, out var hit2, 0.15f,
                        fullLayer);
                    ray = hit2;
                    return r2;

                case CubeDir.South :
                    var r3 = Physics.Raycast(pos + Vector3.back * 0.51f,
                        Vector3.back, out var hit3, 0.15f,
                        fullLayer);
                    ray = hit3;
                    return r3;

                case CubeDir.West :
                    var r4 = Physics.Raycast(pos + Vector3.left * 0.51f,
                        Vector3.left, out var hit4, 0.15f,
                        fullLayer);
                    ray = hit4;
                    return r4;

                default :
                    ray = default;
                    return false;
            }
        }
        
        private void HasPlayer()
        {
            if (Physics.Raycast(transform.position, Vector3.forward, out var hit1, 1.5f, playerLayer))
            {
                _currentCubeDir = CubeDir.South;
                _currentPlayer =  hit1.transform.gameObject;
            }

            if (Physics.Raycast(transform.position, Vector3.right, out var hit2, 1.5f, playerLayer))
            {
                _currentCubeDir = CubeDir.West;
                _currentPlayer =  hit2.transform.gameObject;
            }

            if (Physics.Raycast(transform.position, Vector3.back, out var hit3, 1.5f, playerLayer))
            {
                _currentCubeDir = CubeDir.North;
                _currentPlayer =  hit3.transform.gameObject;
            }

            if (Physics.Raycast(transform.position, Vector3.left, out var hit4, 1.5f, playerLayer))
            {
                _currentCubeDir = CubeDir.East;
                _currentPlayer =  hit4.transform.gameObject;
            }

            //_currentCubeDir = CubeDir.Default;
        }

        #endregion

        private static Vector3 GetDir(CubeDir dir) => dir switch
        {
            CubeDir.North => Vector3.forward,
            CubeDir.East => Vector3.right,
            CubeDir.South => Vector3.back,
            CubeDir.West => Vector3.left,
            CubeDir.Default => Vector3.zero,
            _ => Vector3.zero
        };

        #endregion
    }
}

internal enum CubeDir
{
    Default,
    North,
    East,
    South,
    West
}