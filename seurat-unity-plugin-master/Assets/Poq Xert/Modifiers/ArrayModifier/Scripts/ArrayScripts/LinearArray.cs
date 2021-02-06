//Created by PoqXert (poqxert@gmail.com)

using UnityEngine;
using System.Collections;

namespace PoqXert.ArrayModifier
{
	[AddComponentMenu("Modifiers/Array/Linear")]
	public class LinearArray : BaseArray
	{
		[SerializeField]
		private bool _useLocalAxis = true;
		//Смещение
		[SerializeField]
		private bool _constantOffset = true;
		[SerializeField]
		private Vector3 _constantVect = Vector3.one;

		//Относительное смещение
		[SerializeField]
		private bool _relativeOffset = false;
		[SerializeField]
		private Vector3 _relativeVect = Vector3.zero;

		//Вращение
		[SerializeField]
		private bool _constantRot = true;
		[SerializeField]
		private Vector3 _constantRotVect = Vector3.zero;

		//Относительное вращение
		[SerializeField]
		private bool _relativeRot = false;
		[SerializeField]
		private Vector3 _relativeRotVect = Vector3.zero;

		//Масштаб
		[SerializeField]
		private bool _constantScale = true;
		[SerializeField]
		private Vector3 _constantScaleVect = Vector3.zero;
		
		//Относительный масштаб
		[SerializeField]
		private bool _relativeScale = false;
		[SerializeField]
		private Vector3 _relativeScaleVect = Vector3.zero;

		//Относительные объекты
		[SerializeField]
		private int _relativeObject = 0;
		[SerializeField]
		private int _relativeRotObject = 0;
		[SerializeField]
		private int _relativeScaleObject = 0;

		[SerializeField]
		private Collider _collider = null;
		[SerializeField]
		private Renderer _renderer = null;

		protected override ArrayType type
		{
			get{return ArrayType.Linear;}
		}

		protected override Vector3 GetPosition (int elemID)
		{
			Vector3 result = Vector3.zero;
			if(this._constantOffset)
			{
				result += this._constantVect;
			}
			if(this._relativeOffset)
			{
				Vector3 sizeVect = Vector3.zero;
				if(this._relativeObject == 0)
				{
					sizeVect = this._collider.bounds.size;
				}
				else if(this._relativeObject == 1)
				{
					sizeVect = this._renderer.bounds.size;
				}
				result += Vector3.Scale(this._relativeVect, sizeVect);
			}
			result = result * elemID;
			if(this._useLocalAxis)
			{
				result = this._transform.TransformDirection(result);
			}

			return this._transform.position + result;
		}

		protected override Quaternion GetRotation (int elemID)
		{
			Quaternion result = Quaternion.identity;
			if(this._constantRot)
			{
				result = Quaternion.Euler(this._constantRotVect * elemID);
			}
			if(this._relativeRot)
			{
				Vector3 sizeRotVect = Vector3.zero;
				if(this._relativeRotObject == 0)
				{
					sizeRotVect = this._transform.rotation.eulerAngles;
				}
				else if(this._relativeRotObject == 1)
				{
					sizeRotVect = this._transform.localRotation.eulerAngles;
				}
				result *= Quaternion.Euler((this._relativeRotVect + sizeRotVect) * elemID);
			}
			return this._transform.rotation * result;
		}

		protected override Vector3 GetScale (int elemID)
		{
			Vector3 result = Vector3.one;
			if(this._relativeScale)
			{
				result = Vector3.zero;
				Vector3 sizeScaleVect = Vector3.zero;
				if(this._relativeScaleObject == 0)
				{
					sizeScaleVect = this._transform.lossyScale;
				}
				else if(this._relativeScaleObject == 1)
				{
					sizeScaleVect = this._transform.localScale;
				}
				result += sizeScaleVect + this._relativeScaleVect * elemID;
			}
			if(this._constantScale)
			{
				result += this._constantScaleVect * elemID;
			}
			return result;
		}

		protected override void Set (BaseArray original, int dim, int ID)
		{
			LinearArray o = original as LinearArray;
			if(o != null)
			{
				this._constantOffset		= o._constantOffset;
				this._constantVect			= o._constantVect;
				this._constantRot			= o._constantRot;
				this._constantRotVect		= o._constantRotVect;
				this._constantScale			= o._constantScale;
				this._constantScaleVect		= o._constantScaleVect;
				this._relativeObject 		= o._relativeObject;
				this._relativeOffset 		= o._relativeOffset;
				this._relativeVect 			= o._relativeVect;
				this._relativeRotObject		= o._relativeRotObject;
				this._relativeRot 			= o._relativeRot;
				this._relativeRotVect		= o._relativeRotVect;
				this._relativeScaleObject	= o._relativeScaleObject;
				this._relativeScale 		= o._relativeScale;
				this._relativeScaleVect 	= o._relativeScaleVect;
				this._useLocalAxis			= o._useLocalAxis;
			}
			base.Set (original, dim, ID);
		}

		public override void UpdateInternalInfo ()
		{
			base.UpdateInternalInfo();
			this._collider = this.GetComponent<Collider>();
			this._renderer = this.GetComponent<Renderer>();
		}
	}
}