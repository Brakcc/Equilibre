using UnityEngine;

//Git
namespace InGameScripts.PlayerScripts
{
    public abstract class AbstractPlayerBehavior : MonoBehaviour
    {
        #region fields

        public Vector3 Velocity { get; protected set; }

        #endregion
        
        #region base unity calls

        private void Start() => OnStart();
        private void Update() => OnUpdate();
        private void FixedUpdate() => OnFixedUpdate();
        private void LateUpdate() => OnLateUpdate();

        #endregion
        
        #region methodes to herit

        protected virtual void OnStart() {}
        
        protected virtual void OnFixedUpdate() {}

        protected virtual void OnUpdate() {}
        
        protected virtual void OnLateUpdate() {}
        
        #endregion
    }
}