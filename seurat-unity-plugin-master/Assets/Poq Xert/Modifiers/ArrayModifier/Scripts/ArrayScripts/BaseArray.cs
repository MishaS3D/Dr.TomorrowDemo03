//Created by PoqXert (poqxert@gmail.com)
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace PoqXert.ArrayModifier
{
	public class BaseArray : MonoBehaviour
	{
		[SerializeField]
		protected GameObject _prefab = null;
		//Количество объектов в массиве
		[SerializeField]
		protected int _count = 2;

		//Измерения
		[SerializeField]
		protected int _dimensions = 0;
		[SerializeField]
		protected int _dimCount1 = 1;
		[SerializeField]
		protected int _dimCount2 = 1;
		[SerializeField]
		protected Vector3 _dimOffset1 = Vector3.zero;
		[SerializeField]
		protected Vector3 _dimOffset2 = Vector3.zero;
		[SerializeField]
		private bool _componentsChanged = false;

		//Элементы массива
		[SerializeField]
		protected List<Transform> _elements = new List<Transform>();
		//Элементы измерений
		[SerializeField]
		protected List<Transform> _dimElems = new List<Transform>();
		[SerializeField]
		protected List<Transform> _dimElems2 = new List<Transform>();

		[SerializeField]
		protected Transform _transform = null;

		[SerializeField]
		private ArrayType _type = ArrayType.Linear;

		protected virtual ArrayType type
		{
			get{return ArrayType.Linear;}
		}

		protected bool IsChangeType()
		{
			return _type != type;
		}

		public bool Switch()
		{
			if(!IsChangeType())
				return false;

			GameObject go = gameObject;
			this.Cancel();
			SwitchType(go);
			return true;
		}

		private void SwitchType(GameObject go)
		{
			BaseArray array = null;
			switch(_type)
			{
			case ArrayType.Linear:
				array = go.AddComponent<LinearArray>();
				break;
			case ArrayType.Circle:
				array = go.AddComponent<CircleArray>();
				break;
			case ArrayType.Curve:
				array = go.AddComponent<CurveArray>();
				break;
			case ArrayType.Object:
				array = go.AddComponent<ObjectArray>();
				break;
			}
			array._componentsChanged = true;
			array._count 		= this._count;
			array._dimensions 	= this._dimensions;
			array._dimCount1 	= this._dimCount1;
			array._dimOffset1 	= this._dimOffset1;
			array._dimCount2 	= this._dimCount2;
			array._dimOffset2 	= this._dimOffset2;
			array._prefab 		= null;
			array._type			= this._type;
			array.UpdateInternalInfo();
		}

		/// <summary>
		/// Получение позиции для элемента.
		/// </summary>
		/// <returns>Позиция элемента.</returns>
		/// <param name="elemID">ID элемента.</param>
		protected virtual Vector3 GetPosition(int elemID)
		{
			return this._transform.position;
		}

		protected virtual Quaternion GetRotation(int elemID)
		{
			return this._transform.rotation;
		}

		protected virtual Vector3 GetScale(int elemID)
		{
			return Vector3.one;
		}

		public void UpdateArray()
		{
			if(this._componentsChanged)
			{
				this._type = type;
				this._componentsChanged = false;
				int	cnt = this._count;
				this._count = 1;
				UpdateCount();
				this._count = cnt;
				cnt = this._dimensions;
				this._dimensions = 0;
				UpdateDimCount();
				this._dimensions = cnt;
				UpdatePrefab();
			}

			Transform elem = null;
			if(this._count > 1)
			{
				for(int i = 1; i < this._count; i++)
				{
					if(i - 1 < this._elements.Count)
					{
						elem = this._elements[i-1];
					}
					else
					{
						elem = Instantiate(this._prefab).transform;
						elem.name = (this.name + " " + i);
						this._elements.Add(elem);
					}
					elem.parent = this._transform;
					elem.position = GetPosition(i);
					elem.rotation = GetRotation(i);
					elem.localScale = GetScale(i);
				}//-for
			}//-if
			else
			{
				this._count = 1;
			}

			UpdateCount();

			if(this._dimensions > 0)
			{
				if(this._dimCount1 < 1)
				{
					this._dimCount1 = 1;
				}
				if(this._dimCount2 < 1)
				{
					this._dimCount2 = 1;
				}
				BaseArray array = null;
				for(int m = 1; m < this._dimCount1; m++)
				{
					if(m - 1 < this._dimElems.Count)
					{
						elem = this._dimElems[m - 1];
					}
					else
					{
						elem = Instantiate(this._prefab).transform;
						array = elem.gameObject.AddComponent(this.GetType()) as BaseArray;
						elem.name = (this.name + " " + m);
						this._dimElems.Add(elem);
					}
					elem.position = this._transform.position + this._dimOffset1 * m;
					elem.rotation = this._transform.rotation;
					elem.parent = this._transform;
					elem.localScale = Vector3.one;
					if(array == null)
					{
						array = elem.GetComponent(this.GetType()) as BaseArray;
					}
					array.Set(this, 1, m);
					array = null;
				}
				
				if(this._dimensions > 1)
				{
					for(int n = 1; n < this._dimCount2; n++)
					{
						if(n - 1 < this._dimElems2.Count)
						{
							elem = this._dimElems2[n - 1];
						}
						else
						{
							elem = Instantiate(this._prefab).transform;
							array = elem.gameObject.AddComponent(this.GetType()) as BaseArray;
							elem.name = (this.name + " " + n);
							this._dimElems2.Add(elem);
						}
						elem.position = this._transform.position + this._dimOffset2 * n;
						elem.rotation = this._transform.rotation;
						elem.parent = this._transform;
						if(array == null)
						{
							array = elem.GetComponent(this.GetType()) as BaseArray;
						}
						array.Set(this, 2, n);
						array = null;
					}
				}
			}

			UpdateDimCount();
		}
		
		private void UpdateCount()
		{
			for(int j = this._elements.Count; j >= this._count; j--)
			{
				DestroyImmediate(this._elements[j-1].gameObject);
				this._elements.RemoveAt(j-1);
			}
		}

		private void UpdateDimCount()
		{			
			int cnt = (this._dimensions > 0) ? this._dimCount1 : 1;
			for(int n = this._dimElems.Count; n >= cnt; n--)
			{
				DestroyImmediate(this._dimElems[n-1].gameObject);
				this._dimElems.RemoveAt(n-1);
			}
			cnt = (this._dimensions > 1) ? this._dimCount2 : 1;
			for(int m = this._dimElems2.Count; m >= cnt; m--)
			{
				DestroyImmediate(this._dimElems2[m-1].gameObject);
				this._dimElems2.RemoveAt(m-1);
			}
		}

		protected virtual void Set(BaseArray original, int dim, int ID)
		{
			this._count 		= original._count;
			this._dimensions 	= original._dimensions - dim;
			this._dimCount1 	= original._dimCount2;
			this._dimOffset1 	= original._dimOffset2;
			this._prefab 		= original._prefab;
			this._type			= original._type;
			this.UpdateInternalInfo();
			this.UpdateArray();
		}

		public void Apply()
		{
			foreach(Transform t in this._dimElems)
			{
				(t.GetComponent(this.GetType()) as BaseArray).Apply();
			}
			foreach(Transform t2 in this._dimElems2)
			{
				(t2.GetComponent(this.GetType()) as BaseArray).Apply();
			}
			DeletePrefab();
			DestroyImmediate(this);
		}

		public void Cancel()
		{
			foreach(Transform t in this._elements)
			{
				if(t != null)
				{
					DestroyImmediate(t.gameObject);
				}
			}
			foreach(Transform td1 in this._dimElems)
			{
				if(td1 != null)
				{
					DestroyImmediate(td1.gameObject);
				}
			}
			foreach(Transform td2 in this._dimElems2)
			{
				if(td2 != null)
				{
					DestroyImmediate(td2.gameObject);
				}
			}
			DeletePrefab();
			DestroyImmediate(this);
		}

		public void UpdatePrefab()
		{
			GameObject go = Instantiate(this.gameObject);
			DestroyImmediate(go.GetComponent(this.GetType()));
			if(this._prefab == null)
			{
				string path = "Assets/Poq Xert/Modifiers/ArrayModifier";
				if(!AssetDatabase.IsValidFolder(path + "/Prefabs"))
				{
					path = AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(path, "Prefabs"));
				}
				else
				{
					path += "/Prefabs";
				}
				path += "/Original.prefab";
				path = AssetDatabase.GenerateUniqueAssetPath(path);
				this._prefab = PrefabUtility.CreatePrefab(path, go);
			}
			else
			{
				this._prefab = PrefabUtility.ReplacePrefab(go, this._prefab);
			}
			AssetDatabase.SaveAssets();
			DestroyImmediate(go);
			UpdateInternalInfo();
		}

		public virtual void UpdateInternalInfo()
		{
			this._transform = this.transform;
		}

		public void DeletePrefab()
		{
			if(AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this._prefab)))
			{
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
				this._prefab = null;
			}
		}

		public virtual bool IsUseDimensions()
		{
			return true;
		}

		public enum ArrayType
		{
			Linear = 0,
			Circle = 1,
			Curve = 2,
			Object = 3
		}
	}
}