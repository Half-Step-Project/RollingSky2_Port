using UnityEngine;

namespace Foundation
{
	[AddComponentMenu("Framework/Event")]
	[DisallowMultipleComponent]
	public sealed class EventMod : ModBase
	{
		private const EventMode Mode = EventMode.AllowNoHandler | EventMode.AllowMultiHandler;

		private readonly EventPool<EventArgs> _pool = new EventPool<EventArgs>(EventMode.AllowNoHandler | EventMode.AllowMultiHandler);

		public EventHandler<EventArgs> Handler
		{
			get
			{
				return _pool.Handler;
			}
			set
			{
				_pool.Handler = value;
			}
		}

		public int Count
		{
			get
			{
				return _pool.Count;
			}
		}

		public bool Contains(int id, EventHandler<EventArgs> handler)
		{
			return _pool.Contains(id, handler);
		}

		public void Subscribe(int id, EventHandler<EventArgs> handler)
		{
			_pool.Subscribe(id, handler);
		}

		public void Unsubscribe(int id, EventHandler<EventArgs> handler)
		{
			_pool.Unsubscribe(id, handler);
		}

		public void Fire(object sender, EventArgs args)
		{
			_pool.Fire(sender, args);
		}

		public void FireNow(object sender, EventArgs args)
		{
			_pool.FireNow(sender, args);
		}

		protected override void Awake()
		{
			Mod.Event = this;
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("EventMod.OnTick");
			_pool.Tick();
			Profiler.EndSample();
		}

		internal override void OnExit()
		{
			_pool.Destroy();
		}
	}
}
