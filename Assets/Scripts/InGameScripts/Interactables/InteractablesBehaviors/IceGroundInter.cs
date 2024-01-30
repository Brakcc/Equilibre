using UnityEngine;

namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public class IceGroundInter : AbstractInteractableBehavior
    {
        #region fields

        [SerializeField] private bool hasSnowOnBlock;
        [SerializeField] private float noIceTimer;
        [SerializeField] private GameObject iceLayer;

        private float _noIceCounter;
        private bool _isIced;

        #endregion
        
        #region methodes

        private void Update()
        {
            if (_isIced)
                return;

            _noIceCounter -= Time.deltaTime;
            
            if (_noIceCounter <= 0)
                OnAction(true);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("FirePower"))
            {
                if (hasSnowOnBlock)
                {
                    hasSnowOnBlock = false;
                    return;
                }
                
                OnAction(false);
            }
            
            if (other.CompareTag("IcePower"))
                OnAction(true);
        }

        protected override void OnAction(bool boolVal)
        {
            var tempCol = iceLayer.GetComponent<Collider>();
            var tempRend = iceLayer.GetComponent<MeshRenderer>();

            tempCol.enabled = boolVal;
            tempRend.enabled = boolVal;

            _isIced = boolVal;

            if (boolVal == false)
                _noIceCounter = noIceTimer;
        }

        #endregion
    }
}