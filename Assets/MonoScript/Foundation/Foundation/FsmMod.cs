using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foundation
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Framework/FSM")]
	public sealed class FsmMod : ModBase
	{
		private readonly Dictionary<string, IFsm> _fsms = new Dictionary<string, IFsm>();

		public int Count
		{
			get
			{
				return _fsms.Count;
			}
		}

		public IFsm[] Fsms
		{
			get
			{
				int num = 0;
				IFsm[] array = new IFsm[_fsms.Count];
				foreach (KeyValuePair<string, IFsm> fsm in _fsms)
				{
					array[num++] = fsm.Value;
				}
				return array;
			}
		}

		public bool Contains<TOwner>() where TOwner : class
		{
			string fullName = ReflectionUtility.GetFullName<TOwner>(string.Empty);
			return InternalContains(fullName);
		}

		public bool Contains(Type type)
		{
			if ((object)type == null)
			{
				Log.Warning("Owner type is invalid.");
				return false;
			}
			string fullName = ReflectionUtility.GetFullName(type, string.Empty);
			return InternalContains(fullName);
		}

		public bool Contains<TOwner>(string name) where TOwner : class
		{
			string fullName = ReflectionUtility.GetFullName<TOwner>(name);
			return InternalContains(fullName);
		}

		public bool Contains(Type type, string name)
		{
			if ((object)type == null)
			{
				Log.Warning("Owner type is invalid.");
				return false;
			}
			string fullName = ReflectionUtility.GetFullName(type, name);
			return InternalContains(fullName);
		}

		public IFsm<TOwner> Get<TOwner>() where TOwner : class
		{
			string fullName = ReflectionUtility.GetFullName<TOwner>(string.Empty);
			return (IFsm<TOwner>)InternalGet(fullName);
		}

		public IFsm Get(Type type)
		{
			if ((object)type == null)
			{
				Log.Error("Owner type is invalid.");
				return null;
			}
			string fullName = ReflectionUtility.GetFullName(type, string.Empty);
			return InternalGet(fullName);
		}

		public IFsm<TOwner> Get<TOwner>(string name) where TOwner : class
		{
			string fullName = ReflectionUtility.GetFullName<TOwner>(name);
			return (IFsm<TOwner>)InternalGet(fullName);
		}

		public IFsm Get(Type type, string name)
		{
			if ((object)type == null)
			{
				Log.Error("Owner type is invalid.");
				return null;
			}
			string fullName = ReflectionUtility.GetFullName(type, name);
			return InternalGet(fullName);
		}

		public IFsm<TOwner> Create<TOwner>(TOwner owner, params FsmState<TOwner>[] states) where TOwner : class
		{
			return Create(string.Empty, owner, states);
		}

		public IFsm<TOwner> Create<TOwner>(string name, TOwner owner, params FsmState<TOwner>[] states) where TOwner : class
		{
			string fullName = ReflectionUtility.GetFullName<TOwner>(name);
			if (Contains<TOwner>(name))
			{
				Log.Warning("Already exist FSM '" + fullName + "'.");
				return null;
			}
			Fsm<TOwner> fsm = new Fsm<TOwner>(name, owner, states);
			_fsms.Add(fullName, fsm);
			return fsm;
		}

		public bool Destroy<TOwner>() where TOwner : class
		{
			string fullName = ReflectionUtility.GetFullName<TOwner>(string.Empty);
			return InternalDestroy(fullName);
		}

		public bool Destroy(Type type)
		{
			if ((object)type == null)
			{
				Log.Warning("Owner type is invalid.");
				return false;
			}
			string fullName = ReflectionUtility.GetFullName(type, string.Empty);
			return InternalDestroy(fullName);
		}

		public bool Destroy<TOwner>(string name) where TOwner : class
		{
			string fullName = ReflectionUtility.GetFullName<TOwner>(name);
			return InternalDestroy(fullName);
		}

		public bool Destroy(Type type, string name)
		{
			if ((object)type == null)
			{
				Log.Warning("Owner type is invalid.");
				return false;
			}
			string fullName = ReflectionUtility.GetFullName(type, name);
			return InternalDestroy(fullName);
		}

		public bool Destroy<TOwner>(IFsm<TOwner> fsm) where TOwner : class
		{
			if (fsm == null)
			{
				Log.Warning("FSM is invalid.");
				return false;
			}
			string fullName = ReflectionUtility.GetFullName<TOwner>(fsm.Name);
			return InternalDestroy(fullName);
		}

		public bool Destroy(IFsm fsm)
		{
			if (fsm == null)
			{
				Log.Warning("FSM is invalid.");
				return false;
			}
			string fullName = ReflectionUtility.GetFullName(fsm.OwnerType, fsm.Name);
			return InternalDestroy(fullName);
		}

		protected override void Awake()
		{
			Mod.Fsm = this;
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("FsmMod.OnTick");
			foreach (KeyValuePair<string, IFsm> fsm in _fsms)
			{
				if (!fsm.Value.IsDestroyed)
				{
					fsm.Value.Tick(elapseSeconds, realElapseSeconds);
				}
			}
			Profiler.EndSample();
		}

		internal override void OnExit()
		{
			foreach (KeyValuePair<string, IFsm> fsm in _fsms)
			{
				fsm.Value.Destroy();
			}
			_fsms.Clear();
		}

		private bool InternalContains(string key)
		{
			return _fsms.ContainsKey(key);
		}

		private IFsm InternalGet(string key)
		{
			IFsm value;
			if (!_fsms.TryGetValue(key, out value))
			{
				return null;
			}
			return value;
		}

		private bool InternalDestroy(string key)
		{
			IFsm value;
			if (_fsms.TryGetValue(key, out value))
			{
				value.Destroy();
				return _fsms.Remove(key);
			}
			return false;
		}
	}
}
