using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foundation
{
	[AddComponentMenu("Framework/Object Pool")]
	[DisallowMultipleComponent]
	public sealed class ObjectPoolMod : ModBase
	{
		private readonly Dictionary<string, IObjectPool> _objectPools = new Dictionary<string, IObjectPool>();

		public int Count
		{
			get
			{
				return _objectPools.Count;
			}
		}

		public IObjectPool[] ObjectPools
		{
			get
			{
				int num = 0;
				IObjectPool[] array = new IObjectPool[_objectPools.Count];
				foreach (KeyValuePair<string, IObjectPool> objectPool in _objectPools)
				{
					array[num++] = objectPool.Value;
				}
				return array;
			}
		}

		public bool Contains<T>(string name)
		{
			string fullName = ReflectionUtility.GetFullName<T>(name);
			return InternalContains(fullName);
		}

		public bool Contains(Type type, string name)
		{
			if ((object)type == null)
			{
				Log.Warning("Object type is invalid.");
				return false;
			}
			if (!typeof(SharedObject).IsAssignableFrom(type))
			{
				Log.Warning("Object type '" + type.FullName + "' is invalid.");
				return false;
			}
			string fullName = ReflectionUtility.GetFullName(type, name);
			return InternalContains(fullName);
		}

		public ObjectPool<T> Get<T>(string name) where T : SharedObject
		{
			string fullName = ReflectionUtility.GetFullName<T>(name);
			return (ObjectPool<T>)InternalGet(fullName);
		}

		public IObjectPool Get(Type type, string name)
		{
			if ((object)type == null)
			{
				Log.Warning("Object type is invalid.");
				return null;
			}
			if (!typeof(SharedObject).IsAssignableFrom(type))
			{
				Log.Warning("Object type '" + type.FullName + "' is invalid.");
				return null;
			}
			string fullName = ReflectionUtility.GetFullName(type, name);
			return InternalGet(fullName);
		}

		public ObjectPool<T> Create<T>(string name, bool isShared, bool didIgnoreCase) where T : SharedObject
		{
			string fullName = ReflectionUtility.GetFullName<T>(name);
			if (InternalContains(fullName))
			{
				Log.Warning("Already exist object pool '" + name + "'.");
				return null;
			}
			ObjectPool<T> objectPool = (isShared ? ((ObjectPool<T>)new SharedObjectPool<T>(name, didIgnoreCase)) : ((ObjectPool<T>)new CachedObjectPool<T>(name, didIgnoreCase)));
			_objectPools[fullName] = objectPool;
			return objectPool;
		}

		private IObjectPool Create(Type type, string name, bool isShared, bool didIgnoreCase)
		{
			if ((object)type == null)
			{
				Log.Warning("Object type is invalid.");
				return null;
			}
			if (!typeof(SharedObject).IsAssignableFrom(type))
			{
				Log.Warning("Object type '" + type.FullName + "' is invalid.");
				return null;
			}
			string fullName = ReflectionUtility.GetFullName(type, name);
			if (InternalContains(fullName))
			{
				Log.Warning("Already exist object pool '" + fullName + "'.");
				return null;
			}
			IObjectPool objectPool;
			if (isShared)
			{
				Type type2 = typeof(SharedObjectPool<>).MakeGenericType(type);
				objectPool = (IObjectPool)Activator.CreateInstance(type2, name, didIgnoreCase);
			}
			else
			{
				Type type3 = typeof(CachedObjectPool<>).MakeGenericType(type);
				objectPool = (IObjectPool)Activator.CreateInstance(type3, name, didIgnoreCase);
			}
			_objectPools[fullName] = objectPool;
			return objectPool;
		}

		public bool Release<T>(string name)
		{
			string fullName = ReflectionUtility.GetFullName<T>(name);
			return InternalRelease(fullName);
		}

		public bool Release(Type type, string name)
		{
			if ((object)type == null)
			{
				Log.Warning("Object type is invalid.");
				return false;
			}
			if (!typeof(SharedObject).IsAssignableFrom(type))
			{
				Log.Warning("Object type '" + type.FullName + "' is invalid.");
				return false;
			}
			string fullName = ReflectionUtility.GetFullName(type, name);
			return InternalRelease(fullName);
		}

		public bool Release<T>(ObjectPool<T> objectPool) where T : SharedObject
		{
			if (objectPool == null)
			{
				Log.Warning("Object pool is invalid.");
				return false;
			}
			string fullName = ReflectionUtility.GetFullName<T>(objectPool.Name);
			return InternalRelease(fullName);
		}

		public bool Release(IObjectPool objectPool)
		{
			if (objectPool == null)
			{
				Log.Warning("Object pool is invalid.");
				return false;
			}
			string fullName = ReflectionUtility.GetFullName(objectPool.ObjectType, objectPool.Name);
			return InternalRelease(fullName);
		}

		public void Unload()
		{
			foreach (KeyValuePair<string, IObjectPool> objectPool in _objectPools)
			{
				objectPool.Value.Unload();
			}
		}

		protected override void Awake()
		{
			Mod.ObjectPool = this;
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
		}

		internal override void OnExit()
		{
			base.OnExit();
			foreach (KeyValuePair<string, IObjectPool> objectPool in _objectPools)
			{
				objectPool.Value.Destroy();
			}
			_objectPools.Clear();
		}

		private bool InternalContains(string key)
		{
			return _objectPools.ContainsKey(key);
		}

		private IObjectPool InternalGet(string key)
		{
			IObjectPool value;
			if (!_objectPools.TryGetValue(key, out value))
			{
				return null;
			}
			return value;
		}

		private bool InternalRelease(string key)
		{
			IObjectPool value;
			if (_objectPools.TryGetValue(key, out value) && value != null)
			{
				value.Destroy();
				return _objectPools.Remove(key);
			}
			return false;
		}
	}
}
