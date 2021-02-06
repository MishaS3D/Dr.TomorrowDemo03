//Created by PoqXert (poqxert@gmail.com)

using UnityEngine;
using UnityEditor;
using PoqXert.ArrayModifier;

namespace PoqXertEditor.ArrayModifier
{
	[CustomEditor(typeof(LinearArray))]
	public class LinearArrayEditor : BaseArrayEditor
	{
		SerializedProperty _constantOffset;
		SerializedProperty _constantRot;
		SerializedProperty _constantScale;
		
		SerializedProperty _relativeObject;
		SerializedProperty _relativeOffset;
		SerializedProperty _relativeRotObject;
		SerializedProperty _relativeRot;
		SerializedProperty _relativeScaleObject;
		SerializedProperty _relativeScale;

		GUIContent[] _relaSpace = new GUIContent[]{new GUIContent("World"), new GUIContent("Parent")};

		protected override void OnEnable()
		{
			base.OnEnable();
			_constantOffset = serializedObject.FindProperty("_constantOffset");
			_constantRot = serializedObject.FindProperty("_constantRot");
			_constantScale = serializedObject.FindProperty("_constantScale");

			_relativeObject = serializedObject.FindProperty("_relativeObject");
			_relativeOffset = serializedObject.FindProperty("_relativeOffset");
			_relativeRotObject = serializedObject.FindProperty("_relativeRotObject");
			_relativeRot = serializedObject.FindProperty("_relativeRot");
			_relativeScaleObject = serializedObject.FindProperty("_relativeScaleObject");
			_relativeScale = serializedObject.FindProperty("_relativeScale");
		}

		protected override void ShowParams()
		{
			base.ShowParams();
			//Cмещение
			_constantOffset.boolValue = EditorGUILayout.BeginToggleGroup(new GUIContent("Constant Offset", "Offset of a element position on constant value"), _constantOffset.boolValue);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_constantVect"), new GUIContent("Offset", "Offset value"));
			EditorGUILayout.EndToggleGroup();
			//Вращение
			_constantRot.boolValue = EditorGUILayout.BeginToggleGroup(new GUIContent("Constant Rotation", "Rotate of a element on constant value"), _constantRot.boolValue);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_constantRotVect"), new GUIContent("Rotation", "Rotation value"));
			EditorGUILayout.EndToggleGroup();
			//Масштабирование
			_constantScale.boolValue = EditorGUILayout.BeginToggleGroup(new GUIContent("Constant Scale", "Scale of element on constant value"), _constantScale.boolValue);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_constantScaleVect"), new GUIContent("Scale", "Scale value"));
			EditorGUILayout.EndToggleGroup();
			
			//Относительные
			//Смещение
			_relativeOffset.boolValue = EditorGUILayout.BeginToggleGroup(new GUIContent("Relative Offset", "Offset of a element position on relative value"), _relativeOffset.boolValue);
			_relativeObject.intValue = EditorGUILayout.Popup(new GUIContent("Relative Object"), _relativeObject.intValue, new GUIContent[]{new GUIContent("Collider"), new GUIContent("Mesh")});
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_relativeVect"), new GUIContent("Offset", "Offset value"));
			EditorGUILayout.EndToggleGroup();
				
			//Вращение
			_relativeRot.boolValue = EditorGUILayout.BeginToggleGroup(new GUIContent("Relative Rotation", "Rotate of a element on relative value"), _relativeRot.boolValue);
			_relativeRotObject.intValue = EditorGUILayout.Popup(new GUIContent("Relative Object"), _relativeRotObject.intValue, _relaSpace);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_relativeRotVect"), new GUIContent("Rotation", "Rotation value"));
			EditorGUILayout.EndToggleGroup();

			//Масштабирование
			_relativeScale.boolValue = EditorGUILayout.BeginToggleGroup(new GUIContent("Relative Scale", "Scale of a element on relative value"), _relativeScale.boolValue);
			_relativeScaleObject.intValue = EditorGUILayout.Popup(new GUIContent("Relative Object"), _relativeScaleObject.intValue, _relaSpace);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_relativeScaleVect"), new GUIContent("Scale", "Scale value"));
			EditorGUILayout.EndToggleGroup();

			if(_relativeOffset.boolValue)
			{
				if(serializedObject.FindProperty("_collider").objectReferenceValue == null)
					_relativeObject.intValue = 1;
				else if(serializedObject.FindProperty("_renderer").objectReferenceValue == null)
					_relativeObject.intValue = 0;
			}
			if(_relativeRot.boolValue && (serializedObject.FindProperty("_transform").objectReferenceValue as Transform).parent == null)
				_relativeRotObject.intValue = 0;
		}
	}//-Class
}