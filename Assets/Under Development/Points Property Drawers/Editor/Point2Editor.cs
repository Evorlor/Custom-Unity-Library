namespace CustomUnityLibrary
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(Point2))]
    public class Point2Editor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.Vector2Field(position, label, property.vector2Value);
        }
    }
}