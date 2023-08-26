using System;
using System.Collections.Generic;

namespace Foundation
{
	public sealed class CachedObjectPool<T> : ObjectPool<T> where T : SharedObject
	{
		private readonly Dictionary<string, List<T>> _nameToObjects;

		public CachedObjectPool(string name, bool didIgnoreCase = false)
			: base(name, didIgnoreCase)
		{
			_nameToObjects = (didIgnoreCase ? new Dictionary<string, List<T>>(StringComparer.OrdinalIgnoreCase) : new Dictionary<string, List<T>>());
		}

		public override bool CanSpawn(string name)
		{
			List<T> value;
			if (string.IsNullOrEmpty(name) || !_nameToObjects.TryGetValue(name, out value) || value == null)
			{
				return false;
			}
			for (int i = 0; i < value.Count; i++)
			{
				T val = value[i];
				if (!val.IsInUsing)
				{
					return true;
				}
			}
			return false;
		}

		public override bool CanSpawn(object target)
		{
			T value;
			if (target == null || !targetToObjects.TryGetValue(target, out value) || value == null)
			{
				return false;
			}
			return !value.IsInUsing;
		}

		public override T Spawn(string name)
		{
			List<T> value;
			if (string.IsNullOrEmpty(name) || !_nameToObjects.TryGetValue(name, out value) || value == null)
			{
				return null;
			}
			for (int i = 0; i < value.Count; i++)
			{
				T val = value[i];
				if (!val.IsInUsing)
				{
					val.OnSpawn();
					return val;
				}
			}
			return null;
		}

		public override T Spawn(object target)
		{
			T value;
			if (target == null || !targetToObjects.TryGetValue(target, out value) || value == null)
			{
				return null;
			}
			if (value.IsInUsing)
			{
				return null;
			}
			value.OnSpawn();
			return value;
		}

		public override void Recycle(object target)
		{
			T value;
			if (target != null && targetToObjects.TryGetValue(target, out value) && value != null)
			{
				Log.Assert(value.IsInUsing);
				value.OnRecycle();
			}
		}

		public override void SetLock(object target, bool locked)
		{
			T value;
			if (target != null && targetToObjects.TryGetValue(target, out value) && value != null)
			{
				value.Locked = locked;
			}
		}

		public override void Destroy()
		{
			T[] array = new T[targetToObjects.Count];
			targetToObjects.Values.CopyTo(array, 0);
			targetToObjects.Clear();
			_nameToObjects.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnUnload(true);
			}
		}

		protected override bool Contains(T obj)
		{
			T value;
			if (obj != null && targetToObjects.TryGetValue(obj, out value))
			{
				return value != null;
			}
			return false;
		}

		protected override void Add(T obj)
		{
			targetToObjects[obj.Target] = obj;
			List<T> value;
			if (!_nameToObjects.TryGetValue(obj.Name, out value) || value == null)
			{
				value = new List<T>();
			}
			value.Add(obj);
			_nameToObjects[obj.Name] = value;
		}

		protected override void Remove(T obj)
		{
			targetToObjects.Remove(obj.Target);
			List<T> value;
			if (_nameToObjects.TryGetValue(obj.Name, out value) && value != null)
			{
				value.Remove(obj);
			}
		}
	}
}
