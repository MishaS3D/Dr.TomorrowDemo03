//Created by PoqXert (poqxert@gmail.com)

using UnityEngine;
using System.Collections;

namespace PoqXert.ArrayModifier
{
	[AddComponentMenu("Modifiers/Array/Curve")]
	public class CurveArray : BaseArray
	{
		[SerializeField]
		private Color _curveColor = Color.green;
		[SerializeField]
		private float _distance = 1f;
		[SerializeField]
		private bool _allowCount = false;

		[SerializeField]
		private Vector3 _lastP1 = Vector3.zero;
		[SerializeField]
		private Vector3 _lastP2 = Vector3.zero;
		[SerializeField]
		private Vector3 _lastP3 = Vector3.zero;
		//опорные точки
		public Vector3 pos0
		{
			get
			{
				return this._transform.position;
			}
			set{this._transform.position = value;}
		}
		[SerializeField]
		private Vector3 _P1 = new Vector3(0f, 0f, 5f);
		public Vector3 pos1
		{
			get{return this._P1 + this.pos0;}
			set
			{
				if(this._P1 == value - this.pos0)
					return;
				this._P1 = value - this.pos0;
				UpdateLength();
				UpdateArray();
			}
		}
		[SerializeField]
		private Vector3 _P2 = new Vector3(0f, 0f, 10f);
		public Vector3 pos2
		{
			get{return this._P2 + this.pos0;}
			set
			{
				if(this._P2 == value - this.pos0)
					return;
				this._P2 = value - this.pos0;
				UpdateLength();
				UpdateArray();
			}
		}
		[SerializeField]
		private Vector3 _P3 = new Vector3(0f, 0f, 15f);
		public Vector3 pos3
		{
			get{return this._P3 + this.pos0;}
			set
			{
				if(this._P3 == value - this.pos0)
					return;
				this._P3 = value - this.pos0;
				UpdateLength();
				UpdateArray();
			}
		}

		[SerializeField]
		private float _length = 0f;

		[SerializeField]
		private bool _draw = true;

		protected override ArrayType type
		{
			get{return ArrayType.Curve;}
		}

		private void UpdateLength()
		{
			if(this._lastP1 != this.pos1 || this._lastP2 != this.pos2 || this._lastP3 != this.pos3)
			{
				this._lastP1 = this.pos1;
				this._lastP2 = this.pos2;
				this._lastP3 = this.pos3;
				float t1 = 0f;
				float t2 = 0f;
				this._length = 0f;
				for(int i = 1; i < 50; i++)
				{
					t1 = (i - 1f)/49f;
					t2 = i/49f;
					this._length += (this.Calculate(t1) - this.Calculate(t2)).magnitude;
				}
			}
		}

		// интерполяция по кривой используя формулу кривой Bezier третьего порядка
	    public Vector3 Calculate(float t)
	    {
		    float omt = 1 - t;
			float omt2 = omt * omt;
			float t2 = t * t;
		    return omt * omt2 * this.pos0 + 3 * t * omt2 * this.pos1 +
					3 * t2 * omt * this.pos2 + t2 * t * this.pos3;
	    }
		//"Рисование" кривой
		public void OnDrawGizmos()
		{
			if(!_draw)
				return;
			Gizmos.color = this._curveColor;
			//Отрисовка кривой как несколько сегментов
			Vector3 p1 = this.pos0;
			Vector3 p2;
			for(int i = 1; i < 50; i++){
				float t = i/49f;
				p2 = Calculate(t);
				Gizmos.DrawLine(p1, p2);
				p1 = p2;
			}
		}

		protected override Vector3 GetPosition (int elemID)
		{
			if(this._allowCount)
			{
				return this.Calculate(1f/(this._count-1) * elemID);
			}

			if(this._distance < 0f)
			{
				this._distance = Mathf.Abs(this._distance);
			}

			if(this._distance < Mathf.Epsilon)
			{
				this._distance = Mathf.Epsilon;
			}

			return this.Calculate(1f/(this._length/this._distance) * elemID);
		}

		protected override void Set (BaseArray original, int dim, int ID)
		{
			CurveArray o = original as CurveArray;
			if(o != null)
			{
				this._curveColor = o._curveColor;
				this._distance = o._distance;
				this._allowCount = o._allowCount;
				this._length = o._length;
				this._P1 = o._P1;
				this._P2 = o._P2;
				this._P3 = o._P3;
				this._draw = false;
			}
			base.Set (original, dim, ID);
		}

		public override void UpdateInternalInfo ()
		{
			base.UpdateInternalInfo ();
			UpdateLength();
		}
	}
}
