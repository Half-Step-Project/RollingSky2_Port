using System;
using System.Collections.Generic;

namespace Foundation
{
	public abstract class FsmState<TOwner> where TOwner : class
	{
		private readonly Dictionary<int, FsmEventHandler<TOwner>> _handlers = new Dictionary<int, FsmEventHandler<TOwner>>();

		internal void OnEvent(IFsm<TOwner> fsm, object sender, int eventId, object userData)
		{
			FsmEventHandler<TOwner> value;
			if (_handlers.TryGetValue(eventId, out value) && value != null)
			{
				value(fsm, sender, userData);
			}
		}

		protected internal virtual void OnInit(IFsm<TOwner> fsm)
		{
		}

		protected internal virtual void OnEnter(IFsm<TOwner> fsm)
		{
		}

		protected internal virtual void OnTick(IFsm<TOwner> fsm, float elapseSeconds, float realElapseSeconds)
		{
		}

		protected internal virtual void OnLeave(IFsm<TOwner> fsm, bool isShutdown)
		{
		}

		protected internal virtual void OnDestroy(IFsm<TOwner> fsm)
		{
			_handlers.Clear();
		}

		protected void Subscribe(int eventId, FsmEventHandler<TOwner> handler)
		{
			if (handler == null)
			{
				Log.Warning("eventHandler is null.");
				return;
			}
			if (!_handlers.ContainsKey(eventId))
			{
				_handlers[eventId] = handler;
				return;
			}
			Dictionary<int, FsmEventHandler<TOwner>> handlers = _handlers;
			handlers[eventId] = (FsmEventHandler<TOwner>)Delegate.Combine(handlers[eventId], handler);
		}

		protected void Unsubscribe(int eventId, FsmEventHandler<TOwner> handler)
		{
			FsmEventHandler<TOwner> value;
			if (handler == null)
			{
				Log.Warning("eventHandler is invalid.");
			}
			else if (_handlers.TryGetValue(eventId, out value) && value != null)
			{
				if (value == handler)
				{
					_handlers.Remove(eventId);
					return;
				}
				value = (FsmEventHandler<TOwner>)Delegate.Remove(value, handler);
				_handlers[eventId] = value;
			}
		}

		protected void ChangeState<TState>(IFsm<TOwner> fsm) where TState : FsmState<TOwner>
		{
			Fsm<TOwner> fsm2 = fsm as Fsm<TOwner>;
			if (fsm2 == null)
			{
				Log.Warning("fsm is not a FsmImpl<T>.");
			}
			else
			{
				fsm2.ChangeState<TState>();
			}
		}

		protected void ChangeState(IFsm<TOwner> fsm, Type stateType)
		{
			Fsm<TOwner> fsm2 = fsm as Fsm<TOwner>;
			if (fsm2 == null)
			{
				Log.Warning("fsm is not a FsmImpl<T>.");
			}
			else if ((object)stateType == null)
			{
				Log.Warning("stateType is null.");
			}
			else if (!typeof(FsmState<TOwner>).IsAssignableFrom(stateType))
			{
				Log.Warning("stateType '" + stateType.FullName + "' is not a FsmState<T>.");
			}
			else
			{
				fsm2.ChangeState(stateType);
			}
		}
	}
}
