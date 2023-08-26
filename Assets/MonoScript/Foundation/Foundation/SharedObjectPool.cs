using System;
using System.Collections.Generic;

namespace Foundation
{
	public sealed class SharedObjectPool<T> : ObjectPool<T> where T : SharedObject
	{
		private readonly Dictionary<string, T> _nameToObjects;

		public SharedObjectPool(string name, bool didIgnoreCase = false)
			: base(name, didIgnoreCase)
		{
			_nameToObjects = (didIgnoreCase ? new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase) : new Dictionary<string, T>());
		}

		public override bool CanSpawn(string name)
		{
			Log.Assert(targetToObjects.Count == _nameToObjects.Count);
			T value;
			if (!string.IsNullOrEmpty(name) && _nameToObjects.TryGetValue(name, out value))
			{
				return value != null;
			}
			return false;
		}

		public override bool CanSpawn(object target)
		{
			Log.Assert(targetToObjects.Count == _nameToObjects.Count);
			T value;
			if (target != null && targetToObjects.TryGetValue(target, out value))
			{
				return value != null;
			}
			return false;
		}

		public override T Spawn(string name)
		{
			Log.Assert(targetToObjects.Count == _nameToObjects.Count);
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			T value;
			if (!_nameToObjects.TryGetValue(name, out value) || value == null)
			{
				return null;
			}
			value.OnSpawn();
			return value;
		}

		public override T Spawn(object target)
		{
			Log.Assert(targetToObjects.Count == _nameToObjects.Count);
			if (target == null)
			{
				return null;
			}
			T value;
			if (!targetToObjects.TryGetValue(target, out value) || value == null)
			{
				return null;
			}
			value.OnSpawn();
			return value;
		}

		public override void Recycle(object target)
		{
			Log.Assert(targetToObjects.Count == _nameToObjects.Count);
			T value;
			if (target != null && targetToObjects.TryGetValue(target, out value) && value != null)
			{
				value.OnRecycle();
			}
		}

		public override void SetLock(object target, bool locked)
		{
			Log.Assert(targetToObjects.Count == _nameToObjects.Count);
			T value;
			if (target != null && targetToObjects.TryGetValue(target, out value) && value != null)
			{
				value.Locked = locked;
			}
		}

		public override void Destroy()
		{
			Log.Assert(targetToObjects.Count == _nameToObjects.Count);
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
			Log.Assert(targetToObjects.Count == _nameToObjects.Count);
			T value;
			if (obj != null && targetToObjects.TryGetValue(obj, out value))
			{
				return value != null;
			}
			return false;
		}

		protected override void Add(T obj)
		{
			Log.Assert(targetToObjects.Count == _nameToObjects.Count);
			targetToObjects[obj.Target] = obj;
			_nameToObjects[obj.Name] = obj;
		}

		protected override void Remove(T obj)
		{
			Log.Assert(targetToObjects.Count == _nameToObjects.Count);
			targetToObjects.Remove(obj.Target);
			_nameToObjects.Remove(obj.Name);
		}
	}
}
