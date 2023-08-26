using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foundation
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Framework/Reference")]
	public sealed class ReferenceMod : ModBase
	{
		private readonly Dictionary<Type, ReferencePool> _pools = new Dictionary<Type, ReferencePool>();

		public int Count
		{
			get
			{
				return _pools.Count;
			}
		}

		public ReferenceInfo[] ReferenceInfos
		{
			get
			{
				int num = 0;
				ReferenceInfo[] array = new ReferenceInfo[_pools.Count];
				foreach (KeyValuePair<Type, ReferencePool> pool in _pools)
				{
					array[num++] = new ReferenceInfo(pool.Key, pool.Value.UnusedCount, pool.Value.UsingCount, pool.Value.AcquiredCount, pool.Value.RecycledCount, pool.Value.AddedCount, pool.Value.RemovedCount);
				}
				return array;
			}
		}

		public T Acquire<T>() where T : class, IReference, new()
		{
			return GetPool(typeof(T)).Acquire<T>();
		}

		public void Recycle<T>(T obj) where T : class, IReference
		{
			GetPool(obj.GetType()).Recycle(obj);
		}

		public void Add<T>(int count) where T : class, IReference, new()
		{
			GetPool(typeof(T)).Add<T>(count);
		}

		public void Remove<T>(int count) where T : class, IReference
		{
			GetPool(typeof(T)).Remove(count);
		}

		public void RemoveAll<T>() where T : class, IReference
		{
			GetPool(typeof(T)).RemoveAll();
		}

		public void RemoveAll()
		{
			foreach (KeyValuePair<Type, ReferencePool> pool in _pools)
			{
				pool.Value.RemoveAll();
			}
			_pools.Clear();
		}

		protected override void Awake()
		{
			Mod.Reference = this;
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
		}

		internal override void OnExit()
		{
			foreach (KeyValuePair<Type, ReferencePool> pool in _pools)
			{
				pool.Value.RemoveAll();
			}
			_pools.Clear();
		}

		private ReferencePool GetPool(Type type)
		{
			ReferencePool value;
			if (!_pools.TryGetValue(type, out value) || value == null)
			{
				value = new ReferencePool();
				_pools[type] = value;
			}
			return value;
		}
	}
}
