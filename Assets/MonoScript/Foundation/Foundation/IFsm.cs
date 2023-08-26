using System;

namespace Foundation
{
	public interface IFsm
	{
		string Name { get; }

		Type OwnerType { get; }

		int StateCount { get; }

		bool IsRunning { get; }

		bool IsDestroyed { get; }

		string CurrentStateName { get; }

		float CurrentStateDuration { get; }

		void Tick(float elapseSeconds, float realElapseSeconds);

		void Destroy();
	}
	public interface IFsm<TOwner> where TOwner : class
	{
		string Name { get; }

		TOwner Owner { get; }

		int StateCount { get; }

		bool IsRunning { get; }

		bool IsDestroyed { get; }

		FsmState<TOwner> CurrentState { get; }

		float CurrentStateDuration { get; }

		FsmState<TOwner>[] States { get; }

		void Start<TState>() where TState : FsmState<TOwner>;

		void Start(Type type);

		bool Contains<TState>() where TState : FsmState<TOwner>;

		bool Contains(Type type);

		TState GetState<TState>() where TState : FsmState<TOwner>;

		FsmState<TOwner> GetState(Type type);

		void Fire(object sender, int eventId, object userData);

		bool HasData(string key);

		TData GetData<TData>(string key) where TData : IVar;

		IVar GetData(string key);

		void SetData<TData>(string key, TData value) where TData : IVar;

		void SetData(string key, IVar value);

		bool RemoveData(string key);
	}
}
