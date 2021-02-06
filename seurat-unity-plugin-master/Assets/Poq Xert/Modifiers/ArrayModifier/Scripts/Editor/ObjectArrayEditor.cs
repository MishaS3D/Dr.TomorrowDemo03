//Created by PoqXert (poqxert@gmail.com)

using UnityEngine;
using UnityEditor;
using PoqXert.ArrayModifier;

namespace PoqXertEditor.ArrayModifier
{
	[CustomEditor(typeof(ObjectArray))]
	public class ObjectArrayEditor : BaseArrayEditor
	{
		protected override void ShowParams ()
		{
			base.ShowParams();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_object"), new GUIContent("Object"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_useOffset"), new GUIContent("Use offset"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_useRotation"), new GUIContent("Use rotation"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_useScale"), new GUIContent("Use scale"));

		}
	}
}