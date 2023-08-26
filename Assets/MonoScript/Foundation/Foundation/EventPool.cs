using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Foundation
{
	public sealed class EventPool<T> where T : EventArgs
	{
		private struct Event
		{
			[CompilerGenerated]
			private readonly object _003CSender_003Ek__BackingField;

			[CompilerGenerated]
			private readonly T _003CArgs_003Ek__BackingField;

			public object Sender
			{
				[CompilerGenerated]
				get
				{
					return _003CSender_003Ek__BackingField;
				}
			}

			public T Args
			{
				[CompilerGenerated]
				get
				{
					return _003CArgs_003Ek__BackingField;
				}
			}

			public Event(object sender, T eventArgs)
			{
				_003CSender_003Ek__BackingField = sender;
				_003CArgs_003Ek__BackingField = eventArgs;
			}
		}

		private readonly Dictionary<int, EventHandler<T>> _handlers = new Dictionary<int, EventHandler<T>>();

		private readonly List<Event> _events = new List<Event>();

		private readonly EventMode _mode;

		public EventHandler<T> Handler { get; set; }

		public int Count
		{
			get
			{
				return _events.Count;
			}
		}

		public EventPool(EventMode mode)
		{
			_mode = mode;
		}

		public bool Contains(int eventId, EventHandler<T> handler)
		{
			if (handler == null)
			{
				return false;
			}
			EventHandler<T> value;
			if (!_handlers.TryGetValue(eventId, out value) || value == null)
			{
				return false;
			}
			if (value == handler)
			{
				return true;
			}
			Delegate[] invocationList = value.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				EventHandler<T> eventHandler = (EventHandler<T>)invocationList[i];
				if (eventHandler == handler)
				{
					return true;
				}
			}
			return false;
		}

		public void Subscribe(int eventId, EventHandler<T> handler)
		{
			if (handler != null)
			{
				EventHandler<T> value;
				if (!_handlers.TryGetValue(eventId, out value) || value == null)
				{
					_handlers[eventId] = handler;
					return;
				}
				if ((_mode & EventMode.AllowMultiHandler) == 0)
				{
					Log.Warning("Event '" + eventId + "' not allow multi-handler.");
					return;
				}
				if ((_mode & EventMode.AllowDuplicateHandler) == 0 && Contains(eventId, handler))
				{
					Log.Warning("Event '" + eventId + "' not allow duplicate handler.");
					return;
				}
				value = (EventHandler<T>)Delegate.Combine(value, handler);
				_handlers[eventId] = value;
			}
		}

		public void Unsubscribe(int eventId, EventHandler<T> handler)
		{
			EventHandler<T> value;
			if (handler != null && _handlers.TryGetValue(eventId, out value) && value != null)
			{
				if (value == handler)
				{
					_handlers.Remove(eventId);
					return;
				}
				value = (EventHandler<T>)Delegate.Remove(value, handler);
				_handlers[eventId] = value;
			}
		}

		public void Fire(object sender, T args)
		{
			_events.Add(new Event(sender, args));
		}

		public void FireNow(object sender, T args)
		{
			DispatchEvent(sender, args);
		}

		public void RemoveAllEvents()
		{
			_events.Clear();
		}

		public void Tick()
		{
			Profiler.BeginSample("EventPool.Tick");
			if (_events.Count > 0)
			{
				for (int i = 0; i < _events.Count; i++)
				{
					Event @event = _events[i];
					DispatchEvent(@event.Sender, @event.Args);
				}
				_events.Clear();
			}
			Profiler.EndSample();
		}

		public void Destroy()
		{
			RemoveAllEvents();
			_handlers.Clear();
			Handler = null;
		}

		private void DispatchEvent(object sender, T args)
		{
			Profiler.BeginSample("EventPool.DispatchEvent");
			try
			{
				EventHandler<T> value;
				if (_handlers.TryGetValue(args.Id, out value) && value != null)
				{
					value(sender, args);
					return;
				}
				if (Handler != null)
				{
					Handler(sender, args);
					return;
				}
				if ((_mode & EventMode.AllowNoHandler) == 0)
				{
					Log.Warning("Event '" + args.Id + "' not allow no handler.");
				}
			}
			catch (Exception exception)
			{
				Log.Exception(exception);
			}
			finally
			{
				Mod.Reference.Recycle(args);
			}
			Profiler.EndSample();
		}
	}
}
