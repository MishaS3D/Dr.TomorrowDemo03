//Created by PoqXert (poqxert@gmail.com)

using UnityEngine;

namespace PoqXert.ArrayModifier
{
	[AddComponentMenu("Modifiers/Array/Object")]
	public class ObjectArray : BaseArray
	{
		[SerializeField]
		private Transform _object = null;
		[SerializeField]
		private bool _useOffset = true;
		[SerializeField]
		private bool _useRotation = true;
		[SerializeField]
		private bool _useScale = false;

		protected override ArrayType type
		{
			get{return ArrayType.Object;}
		}

		protected override Vector3 GetPosition (int elemID)
		{
			if(!this._useOffset || this._object == null)
			{
				return base.GetPosition(elemID);
			}

			return (this._transform.position + (this._object.position - this._transform.position) * elemID);
		}

		protected override Quaternion GetRotation (int elemID)
		{
			if(!this._useRotation || this._object == null)
			{
				return base.GetRotation(elemID);
			}

			return (this._transform.rotation * Quaternion.Euler(((this._object.rotation.eulerAngles - this._transform.rotation.eulerAngles) * elemID)));
		}

		protected override Vector3 GetScale (int elemID)
		{
			if(!this._useScale || this._object == null)
			{
				return base.GetScale(elemID);
			}

			return (Vector3.one + (this._object.lossyScale - Vector3.one) * elemID);
		}

		protected override void Set (BaseArray original, int dim, int ID)
		{
			ObjectArray o = original as ObjectArray;
			if(o != null)
			{
				this._object = o._object;
				this._useOffset = o._useOffset;
				this._useRotation = o._useRotation;
				this._useScale = o._useScale;
			}
			base.Set (original, dim, ID);
		}

		public override bool IsUseDimensions ()
		{
			return false;
		}
	}
}