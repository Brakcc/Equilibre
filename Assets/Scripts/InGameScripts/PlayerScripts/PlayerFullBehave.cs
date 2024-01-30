using System.Collections.Generic;
using UnityEngine;

namespace InGameScripts.PlayerScripts
{
    public class PlayerFullBehave : MonoBehaviour
    {
        #region fields
        
        [SerializeField] private List<AbstractPlayerBehavior> behaviors;
        private CharacterController _charaCont;

        #endregion

        #region methodes

        private void Start()
        {
            _charaCont = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            var fullDir = new Vector3();
            foreach (var behavior in behaviors)
            {
                fullDir += behavior.Velocity;
            }

            _charaCont.Move(fullDir);
        }

        #endregion
    }
}