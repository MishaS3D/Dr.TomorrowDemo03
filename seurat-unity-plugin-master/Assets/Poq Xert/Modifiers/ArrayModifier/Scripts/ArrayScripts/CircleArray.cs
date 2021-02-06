using UnityEngine;

namespace PoqXert.ArrayModifier
{
	[AddComponentMenu("Modifiers/Array/Circle")]
	public class CircleArray : BaseArray
	{
		[SerializeField]
		private float _radius = 2;
		[SerializeField]
		private Vector3 _directionRadius = Vector3.right;
		[SerializeField]
		private Vector3 _axisRotation = Vector3.forward;
		[SerializeField]
		private bool _useRotation = true;

		protected override ArrayType type
		{
			get{return ArrayType.Circle;}
		}

		protected override Vector3 GetPosition (int elemID)
		{
			float angle = (180f - (360f/this._count * elemID)) * Mathf.Deg2Rad;
			Vector3 normalRadir = this._directionRadius.normalized;
			Vector3 sin = Vector3.Cross(this._directionRadius, this._axisRotation).normalized * Mathf.Sin(angle);
			Vector3 cos = normalRadir  * Mathf.Cos(angle);
			return this._transform.position + (normalRadir + sin + cos) * this._radius;
		}

		protected override Quaternion GetRotation (int elemID)
		{
			if(!this._useRotation)
			{
				return base.GetRotation (elemID);
			}

			return this._transform.rotation * Quaternion.Euler(this._axisRotation.normalized * 360/this._count * elemID);
		}
	}
}