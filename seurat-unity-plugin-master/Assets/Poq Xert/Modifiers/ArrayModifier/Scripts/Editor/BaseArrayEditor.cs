using UnityEngine;
using System.Collections;
using UnityEditor;
using PoqXert.ArrayModifier;

namespace PoqXertEditor.ArrayModifier
{
	[CustomEditor(typeof(BaseArray))]
	public class BaseArrayEditor : Editor
	{
		SerializedProperty _dimensions;

		
		protected virtual void OnEnable()
		{
			this._dimensions = serializedObject.FindProperty("_dimensions");
			if(serializedObject.FindProperty("_prefab").objectReferenceValue == null)
			{
				serializedObject.FindProperty("_componentsChanged").boolValue = true;
				serializedObject.ApplyModifiedProperties();
				this.GetTarget().UpdateArray();
			}
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_type"), new GUIContent("Type", "Array type"));
			if(EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				if(this.GetTarget().Switch())
					return;
			}
			EditorGUI.BeginChangeCheck();
			ShowParams();
			if(EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				this.GetTarget().UpdateArray();
			}
			ShowButtons();
		}

		protected virtual void ShowParams()
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_count"), new GUIContent("Count", "Count of elements"));

			if(!this.GetTarget().IsUseDimensions())
				return;
			EditorGUILayout.IntPopup(this._dimensions, new GUIContent[]{new GUIContent("1D", "One-dimensional"),
				new GUIContent("2D", "Two-dimensional"), new GUIContent("3D", "Three-dimensional")}, new int[]{0, 1, 2});
			
			if(_dimensions.intValue > 0)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PrefixLabel("2D Settings");
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_dimCount1"), new GUIContent("Count", "Count in 2D"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_dimOffset1"), new GUIContent("Offset", "Offset in 2D"));
				EditorGUI.indentLevel--;
				if(_dimensions.intValue > 1)
				{
					EditorGUILayout.PrefixLabel("3D Settings");
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(serializedObject.FindProperty("_dimCount2"), new GUIContent("Count", "Count in 3D"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("_dimOffset2"), new GUIContent("Offset", "Offset in 3D"));
					EditorGUI.indentLevel--;
				}
				EditorGUI.indentLevel--;
			}
		}

		protected void ShowButtons()
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_componentsChanged"), new GUIContent("Components changed", "Any component on this object has been changed, added or deleted?"));
			if(EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button(new GUIContent("Update", "Update array on scene")))
			{
				serializedObject.ApplyModifiedProperties();
				this.GetTarget().UpdateArray();
			}
			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button(new GUIContent("Apply", "Save and end editing")))
			{
				this.GetTarget().Apply();
			}
			EditorGUILayout.Space();
			if(GUILayout.Button(new GUIContent("Cancel", "Destroy array")))
			{
				this.GetTarget().Cancel();
			}
			EditorGUILayout.EndHorizontal();
		}

		protected BaseArray GetTarget()
		{
			return (serializedObject.targetObject as BaseArray);
		}
	}
}