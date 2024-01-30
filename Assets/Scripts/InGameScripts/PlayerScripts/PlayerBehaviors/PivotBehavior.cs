﻿using UnityEngine;
using Utilities.CustomAttributes;

namespace InGameScripts.PlayerScripts.PlayerBehaviors
{
    public sealed class PivotBehavior : AbstractPlayerBehavior
    {
        [FieldCompletion] [SerializeField] private Transform playerPos;
        [SerializeField] private Vector3 offset;
        
        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            transform.position = playerPos.position + offset;
        }
    }
}