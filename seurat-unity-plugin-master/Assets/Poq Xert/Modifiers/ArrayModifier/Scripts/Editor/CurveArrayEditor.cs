//Created by PoqXert (poqxert@gmail.com)

using UnityEngine;
using UnityEditor;
using PoqXert.ArrayModifier;

namespace PoqXertEditor.ArrayModifier
{
	[CustomEditor(typeof(CurveArray))]
	public class CurveArrayEditor : BaseArrayEditor
	{
		protected override void ShowParams ()
		{
			base.ShowParams ();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_curveColor"), new GUIContent("Curve color"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_distance"), new GUIContent("Distence", "Distance between objects"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_allowCount"), new GUIContent("Allow count", "Calculate distance base on count objects"));
		}
		
		//Отрисовка в сцене
		public void OnSceneGUI()
		{
			CurveArray arr = this.GetTarget() as CurveArray;
			//Рисуем манипуляторы
			Handles.DrawLine(arr.pos0, arr.pos1);
			Handles.DrawLine(arr.pos2, arr.pos3);
		
			//Манипуляторы для контрольных точек
			Quaternion rot = Quaternion.identity;
			arr.pos0 = Handles.PositionHandle(arr.pos0, rot);
			arr.pos1 = Handles.PositionHandle(arr.pos1, rot);
			arr.pos2 = Handles.PositionHandle(arr.pos2, rot);
			arr.pos3 = Handles.PositionHandle(arr.pos3, rot);
		}
	}
}
