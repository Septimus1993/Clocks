using UnityEditor;
using UnityEngine;

namespace ClockEngine
{
    [CustomPropertyDrawer(typeof(HandData))]
    public class HandDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var labelPosition = position.SetWidth(EditorGUIUtility.labelWidth);

            EditorGUI.LabelField(position, label);

            position = position.AlignRight(position.width - labelPosition.width - 2);

            EditorGUI.PropertyField(position, property.FindPropertyRelative("dragger"), GUIContent.none);

            position = EditorGUILayout.GetControlRect(false).AlignRight(position.width);

            EditorGUI.PropertyField(position, property.FindPropertyRelative("inputField"), GUIContent.none);
        }
    }
}