using System.Collections.Generic;
using InGameScripts.PlayerScripts.PlayerBehaviors;
using UnityEngine;

//Git
namespace InGameScripts.PlayerScripts
{
    public class PlayerFullBehave : MonoBehaviour
    {
        #region fields
        
        [SerializeField] private List<AbstractPlayerBehavior> behaviors;
        private CharacterController _charaCont;
        private PlayerDeath _death;

        #endregion

        #region methodes

        private void Start()
        {
            _death = GetComponent<PlayerDeath>();
            _charaCont = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            if (_death.isDed)
                return;
            
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