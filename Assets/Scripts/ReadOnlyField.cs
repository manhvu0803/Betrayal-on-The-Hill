using UnityEngine;
using UnityEditor;

// Read Only attribute.
// Attribute is use only to mark ReadOnly properties.
public class ReadOnlyFieldAttribute : PropertyAttribute { }


// This class contain custom drawer for ReadOnly attribute.
[CustomPropertyDrawer(typeof(ReadOnlyFieldAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, label, true);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label, true);
		GUI.enabled = true;
	}
}