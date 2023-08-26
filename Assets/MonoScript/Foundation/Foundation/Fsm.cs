using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Foundation
{
	public sealed class Fsm<TOwner> : IFsm, IFsm<TOwner> where TOwner : class
	{
		private readonly Dictionary<string, FsmState<TOwner>> _states = new Dictionary<string, FsmState<TOwner>>();

		private readonly Dictionary<string, IVar> _data = new Dictionary<string, IVar>();

		private float _currentStateDuration;

		private bool _isDestroyed;

		[CompilerGenerated]
		private readonly string _003CName_003Ek__BackingField;

		[CompilerGenerated]
		private readonly TOwner _003COwner_003Ek__BackingField;

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return _003CName_003Ek__BackingField;
			}
		}

		public TOwner Owner
		{
			[CompilerGenerated]
			get
			{
				return _003COwner_003Ek__BackingField;
			}
		}

		public Type OwnerType
		{
			get
			{
				return typeof(TOwner);
			}
		}

		public int StateCount
		{
			get
			{
				return _states.Count;
			}
		}

		public bool IsRunning
		{
			get
			{
				return CurrentState != null;
			}
		}

		public bool IsDestroyed
		{
			get
			{
				return _isDestroyed;
			}
		}

		public FsmState<TOwner> CurrentState { get; private set; }

		public string CurrentStateName
		{
			get
			{
				FsmState<TOwner> currentState = CurrentState;
				if (currentState == null)
				{
					return null;
				}
				return currentState.GetType().FullName;
			}
		}

		public float CurrentStateDuration
		{
			get
			{
				return _currentStateDuration;
			}
		}

		public FsmState<TOwner>[] States
		{
			get
			{
				int num = 0;
				FsmState<TOwner>[] array = new FsmState<TOwner>[_states.Count];
				foreach (KeyValuePair<string, FsmState<TOwner>> state in _states)
				{
					array[num++] = state.Value;
				}
				return array;
			}
		}

		public Fsm(string name, TOwner owner, params FsmState<TOwner>[] states)
		{
			if (owner == null)
			{
				Log.Error("owner is null.");
				return;
			}
			if (states == null || states.Length == 0)
			{
				Log.Error("states is null or that length is 0.");
				return;
			}
			_003CName_003Ek__BackingField = name;
			_003COwner_003Ek__BackingField = owner;
			foreach (FsmState<TOwner> fsmState in states)
			{
				if (fsmState == null)
				{
					Log.Warning("State is null.");
					continue;
				}
				string fullName = fsmState.GetType().FullName;
				if (fullName == null)
				{
					Log.Warning("State name is null.");
				}
				else if (_states.ContainsKey(fullName))
				{
					Log.Warning("FSM '" + ReflectionUtility.GetFullName<TOwner>(name) + "' state '" + fullName + "' is already exist.");
				}
				else
				{
					_states[fullName] = fsmState;
					fsmState.OnInit(this);
				}
			}
		}

		public void Start<TState>() where TState : FsmState<TOwner>
		{
			if (IsRunning)
			{
				Log.Warning("FSM is running, can not start again.");
				return;
			}
			TState state = GetState<TState>();
			if (state == null)
			{
				Log.Warning("FSM '" + ReflectionUtility.GetFullName<TOwner>(Name) + "' can not start state '" + typeof(TState).FullName + "' which is not exist.");
			}
			else
			{
				_currentStateDuration = 0f;
				CurrentState = state;
				CurrentState.OnEnter(this);
			}
		}

		public void Start(Type type)
		{
			if (IsRunning)
			{
				Log.Warning("FSM is running, can not start again.");
				return;
			}
			if ((object)type == null)
			{
				Log.Warning("State type is invalid.");
				return;
			}
			if (!typeof(FsmState<TOwner>).IsAssignableFrom(type))
			{
				Log.Warning("State type '" + type.FullName + "' is invalid.");
				return;
			}
			FsmState<TOwner> state = GetState(type);
			if (state == null)
			{
				Log.Warning("FSM '" + ReflectionUtility.GetFullName<TOwner>(Name) + "' can not start state '" + type.FullName + "' which is not exist.");
			}
			else
			{
				_currentStateDuration = 0f;
				CurrentState = state;
				CurrentState.OnEnter(this);
			}
		}

		public bool Contains<TState>() where TState : FsmState<TOwner>
		{
			string fullName = typeof(TState).FullName;
			if (fullName != null)
			{
				return _states.ContainsKey(fullName);
			}
			return false;
		}

		public bool Contains(Type type)
		{
			if ((object)type == null)
			{
				Log.Warning("State type is invalid.");
				return false;
			}
			if (!typeof(FsmState<TOwner>).IsAssignableFrom(type))
			{
				Log.Warning("State type '" + type.FullName + "' is invalid.");
				return false;
			}
			if (type.FullName != null)
			{
				return _states.ContainsKey(type.FullName);
			}
			return false;
		}

		public TState GetState<TState>() where TState : FsmState<TOwner>
		{
			string fullName = typeof(TState).FullName;
			FsmState<TOwner> value;
			if (fullName != null && _states.TryGetValue(fullName, out value))
			{
				return (TState)value;
			}
			return null;
		}

		public FsmState<TOwner> GetState(Type type)
		{
			if ((object)type == null)
			{
				Log.Warning("State type is invalid.");
				return null;
			}
			if (!typeof(FsmState<TOwner>).IsAssignableFrom(type))
			{
				Log.Warning("State type '" + type.FullName + "' is invalid.");
				return null;
			}
			FsmState<TOwner> value;
			if (type.FullName == null || !_states.TryGetValue(type.FullName, out value))
			{
				return null;
			}
			return value;
		}

		public void Fire(object sender, int eventId, object userData = null)
		{
			if (CurrentState == null)
			{
				Log.Warning("Current state is invalid.");
			}
			else
			{
				CurrentState.OnEvent(this, sender, eventId, userData);
			}
		}

		public bool HasData(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				Log.Warning("Data name is invalid.");
				return false;
			}
			return _data.ContainsKey(key);
		}

		public TData GetData<TData>(string key) where TData : IVar
		{
			return (TData)GetData(key);
		}

		public IVar GetData(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				Log.Warning("Data name is invalid.");
				return null;
			}
			IVar value;
			if (!_data.TryGetValue(key, out value))
			{
				return null;
			}
			return value;
		}

		public void SetData<TData>(string key, TData value) where TData : IVar
		{
			if (string.IsNullOrEmpty(key))
			{
				Log.Warning("Data name is invalid.");
			}
			else
			{
				_data[key] = value;
			}
		}

		public void SetData(string key, IVar value)
		{
			if (string.IsNullOrEmpty(key))
			{
				Log.Warning("Data name is invalid.");
			}
			else
			{
				_data[key] = value;
			}
		}

		public bool RemoveData(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				Log.Warning("Data name is invalid.");
				return false;
			}
			return _data.Remove(key);
		}

		public void Tick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("FsmMod.Fsm.Tick");
			if (CurrentState != null)
			{
				_currentStateDuration += elapseSeconds;
				CurrentState.OnTick(this, elapseSeconds, realElapseSeconds);
				Profiler.EndSample();
			}
		}

		public void Destroy()
		{
			if (CurrentState != null)
			{
				CurrentState.OnLeave(this, true);
				CurrentState = null;
				_currentStateDuration = 0f;
			}
			foreach (KeyValuePair<string, FsmState<TOwner>> state in _states)
			{
				state.Value.OnDestroy(this);
			}
			_states.Clear();
			_data.Clear();
			_isDestroyed = true;
		}

		internal void ChangeState<TState>() where TState : FsmState<TOwner>
		{
			ChangeState(typeof(TState));
		}

		internal void ChangeState(Type type)
		{
			if (CurrentState == null)
			{
				Log.Warning("Current state is invalid.");
				return;
			}
			FsmState<TOwner> state = GetState(type);
			if (state == null)
			{
				Log.Warning("FSM '" + ReflectionUtility.GetFullName<TOwner>(Name) + "' can not change state to '" + type.FullName + "' which is not exist.");
			}
			else
			{
				CurrentState.OnLeave(this, false);
				_currentStateDuration = 0f;
				CurrentState = state;
				CurrentState.OnEnter(this);
			}
		}
	}
}
