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
            if (Physics.Raycast(cubeRef.transform.position, Vector3.down, out var hit, 2f, fullLayer))
            {
                Debug.Log(hit.transform.gameObject.layer);
                if (hit.transform.gameObject.layer == 10)
                {
                    IsActivated = false;
                    _currentMoveDir = Vector3.zero;
                    _currentCubeDir = default;
                    cubeRef.transform.position = _originPos;
                }
            }

            _currentPlayer = HasPlayer();
            
            OnMoveCube();
        }

        #region Triggers
        
        protected override void OnTriggerEnter(Collider other) {}
        
        #endregion

        public void OnActivatorAction()
        {
            Debug.Log(1);
            
            if (IsActivated)
                return;
            
            _currentMoveDir = GetDir(_currentCubeDir);
            IsActivated = true;
        }

        private void OnMoveCube()
        {
            if (!IsActivated)
                return;

            if (IsHittingGround(_currentCubeDir, out var hit))
            {
                if (hit.transform.gameObject.layer is 6 or 8)
                {
                    IsActivated = false;
                    _currentMoveDir = Vector3.zero;
                    _currentCubeDir = default;
                }
            }
            
            cubeRef.transform.position += _currentMoveDir * (Time.deltaTime * 4);
        }

        #region rays

        private bool IsHittingGround(CubeDir dir, out RaycastHit ray)
        {
            ray = default;
            var pos = cubeRef.transform.position;
            switch (dir)
            {
                case CubeDir.North :
                    var r1 = Physics.Raycast(pos + new Vector3(0, 0,
                            Constants.HalfCubeSizeConsideration + sideRayOffset),
                        Vector3.down, out var hit1, Constants.HalfCubeSizeConsideration + Constants.MaxStepHeight,
                        fullLayer);
                    ray = hit1;
                    return r1;

                case CubeDir.East :
                    var r2 = Physics.Raycast(pos + new Vector3(Constants.HalfCubeSizeConsideration + sideRayOffset,
                                        0, 0),
                        Vector3.down, out var hit2, Constants.HalfCubeSizeConsideration + Constants.MaxStepHeight,
                        fullLayer);
                    ray = hit2;
                    return r2;

                case CubeDir.South :
                    var r3 = Physics.Raycast(pos + new Vector3(0, 0,
                                        -(Constants.HalfCubeSizeConsideration + sideRayOffset)),
                        Vector3.down, out var hit3, Constants.HalfCubeSizeConsideration + Constants.MaxStepHeight,
                        fullLayer);
                    ray = hit3;
                    return r3;

                case CubeDir.West :
                    var r4 = Physics.Raycast(pos + new Vector3(-(Constants.HalfCubeSizeConsideration + sideRayOffset),
                                        0, 0),
                        Vector3.down, out var hit4, Constants.HalfCubeSizeConsideration + Constants.MaxStepHeight,
                        fullLayer);
                    ray = hit4;
                    return r4;

                default :
                    ray = default;
                    return false;
            }
        }

        private GameObject HasPlayer()
        {
            if (Physics.Raycast(transform.position, Vector3.forward, out var hit1, 1.5f, playerLayer))
            {
                _currentCubeDir = CubeDir.South;
                return hit1.transform.gameObject;
            }

            if (Physics.Raycast(transform.position, Vector3.right, out var hit2, 1.5f, playerLayer))
            {
                _currentCubeDir = CubeDir.West;
                return hit2.transform.gameObject;
            }

            if (Physics.Raycast(transform.position, Vector3.back, out var hit3, 1.5f, playerLayer))
            {
                _currentCubeDir = CubeDir.North;
                return hit3.transform.gameObject;
            }

            if (Physics.Raycast(transform.position, Vector3.left, out var hit4, 1.5f, playerLayer))
            {
                _currentCubeDir = CubeDir.East;
                return hit4.transform.gameObject;
            }

            return null;
        }

        #endregion

        private static Vector3 GetDir(CubeDir dir) => dir switch
        {
            CubeDir.North => Vector3.forward,
            CubeDir.East => Vector3.right,
            CubeDir.South => Vector3.back,
            CubeDir.West => Vector3.left,
            _ => Vector3.zero
        };

        #endregion
    }
}

internal enum CubeDir
{
    North,
    East,
    South,
    West
}