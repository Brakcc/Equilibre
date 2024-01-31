using UnityEngine;
using Utilities.CustomAttributes;

//Git
namespace InGameScripts.Interactables.InteractablesBehaviors
{
    public class WaterInter : AbstractInteractableBehavior
    {
        #region fields

        [SerializeField] private float iceTimer;
        [FieldCompletion] [SerializeField] private GameObject iceLayer;
        
        private float _iceCounter;
        private bool _isIced;

        #endregion

        #region methodes

        private void Start()
        {
            OnAction(false);
        }

        private void Update()
        {
            if (!_isIced)
                return;

            _iceCounter -= Time.deltaTime;
            
            if (_iceCounter <= 0)
                OnAction(false);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("IcePower"))
            {
                OnAction(true);
                _iceCounter = iceTimer;
            }

            if (other.CompareTag("FirePower"))
            {
                OnAction(false);
                _iceCounter = 0;
            }
        }

        protected override void OnAction(bool active)
        {
            var tempCol = iceLayer.GetComponent<Collider>();
            var tempRend = iceLayer.GetComponent<MeshRenderer>();

            tempCol.enabled = active;
            tempRend.enabled = active;

            _isIced = active;
        }

        #endregion
    }
}