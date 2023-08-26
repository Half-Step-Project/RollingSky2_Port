using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Foundation
{
	public abstract class ObjectPool<T> : IObjectPool where T : SharedObject
	{
		[Serializable]
		[CompilerGenerated]
		private sealed class _003C_003Ec
		{
			public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

			public static Comparison<PoolObjectInfo> _003C_003E9__16_0;

			internal int _003Cget_ObjectInfos_003Eb__16_0(PoolObjectInfo x, PoolObjectInfo y)
			{
				return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
			}
		}

		protected readonly Dictionary<object, T> targetToObjects = new Dictionary<object, T>();

		private readonly LinkedList<T> _cacheObjects = new LinkedList<T>();

		private readonly List<T> _cacheUnloads = new List<T>();

		[CompilerGenerated]
		private readonly string _003CName_003Ek__BackingField;

		[CompilerGenerated]
		private readonly bool _003CDidIgnoreCase_003Ek__BackingField;

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return _003CName_003Ek__BackingField;
			}
		}

		public Type ObjectType
		{
			get
			{
				return typeof(T);
			}
		}

		public bool DidIgnoreCase
		{
			[CompilerGenerated]
			get
			{
				return _003CDidIgnoreCase_003Ek__BackingField;
			}
		}

		public int Count
		{
			get
			{
				return targetToObjects.Count;
			}
		}

		public int CanUnloadCount
		{
			get
			{
				int num = 0;
				foreach (KeyValuePair<object, T> targetToObject in targetToObjects)
				{
					T value = targetToObject.Value;
					if (!value.IsInUsing && !value.Locked)
					{
						num++;
					}
				}
				return num;
			}
		}

		public PoolObjectInfo[] ObjectInfos
		{
			get
			{
				int num = 0;
				PoolObjectInfo[] array = new PoolObjectInfo[targetToObjects.Count];
				foreach (KeyValuePair<object, T> targetToObject in targetToObjects)
				{
					T value = targetToObject.Value;
					array[num++] = new PoolObjectInfo(value.Name, value.Locked, value.UseCount);
				}
				Array.Sort(array, _003C_003Ec._003C_003E9__16_0 ?? (_003C_003Ec._003C_003E9__16_0 = _003C_003Ec._003C_003E9._003Cget_ObjectInfos_003Eb__16_0));
				return array;
			}
		}

		protected ObjectPool(string name, bool didIgnoreCase)
		{
			_003CName_003Ek__BackingField = name;
			_003CDidIgnoreCase_003Ek__BackingField = didIgnoreCase;
		}

		public void Register(T obj)
		{
			if (obj == null)
			{
				Log.Warning("Object is invalid.");
				return;
			}
			if (Contains(obj))
			{
				Log.Warning("Object " + obj.Name + " was exist.");
				return;
			}
			obj.OnRegister();
			Add(obj);
		}

		public abstract bool CanSpawn(string name);

		public abstract bool CanSpawn(object target);

		public abstract T Spawn(string name);

		public abstract T Spawn(object target);

		public abstract void Recycle(object target);

		public abstract void SetLock(object target, bool locked);

		public void Unload()
		{
			List<T> list = new List<T>();
			foreach (KeyValuePair<object, T> targetToObject in targetToObjects)
			{
				T value = targetToObject.Value;
				if (!value.IsInUsing && !value.Locked)
				{
					list.Add(value);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				T val = list[i];
				Remove(val);
				val.OnUnload();
			}
		}

		public abstract void Destroy();

		protected abstract bool Contains(T obj);

		protected abstract void Add(T obj);

		protected abstract void Remove(T obj);
	}
}
