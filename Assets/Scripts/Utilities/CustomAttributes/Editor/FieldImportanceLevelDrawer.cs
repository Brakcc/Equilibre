﻿using UnityEditor;
using UnityEngine;

//Git
namespace Utilities.CustomAttributes.Editor
{
    [CustomPropertyDrawer(typeof(FieldImportanceLevel))]
    public class FieldImportanceLevelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var field = attribute as FieldImportanceLevel;
        
            if(property.objectReferenceValue == null)
            {
                if (field != null)
                    GUI.color = field.unCheckedColor;
                EditorGUI.PropertyField(position, property, label);
                GUI.color = Color.white;
            }
            else
            {
                if (field != null)
                    GUI.color = field.checkedColor;
                EditorGUI.PropertyField(position, property, label);
                GUI.color = Color.white;
            }
        }
    }
}