//Created by PoqXert (poqxert@gmail.com)

using UnityEngine;
using UnityEditor;
using PoqXert.ArrayModifier;

namespace PoqXertEditor.ArrayModifier
{
	[CustomEditor(typeof(CircleArray))]
	public class CircleArrayEditor : BaseArrayEditor
	{
		protected override void ShowParams ()
		{
			base.ShowParams ();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_radius"), new GUIContent("Radius", "Radius of circle"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_directionRadius"), new GUIContent("Direction radius", "Direction radius axis"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_axisRotation"), new GUIContent("Rotation axis", "Normal of circle"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_useRotation"), new GUIContent("Use rotation", "Apply rotation for elements"));
		}
	}
}