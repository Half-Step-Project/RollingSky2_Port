using System;
using System.Collections;
using UnityEngine;

namespace Foundation
{
	public class MonoTimer
	{
		public float Interval { get; private set; }

		public bool Repeat { get; private set; }

		public bool FireElapsedOnStop { get; set; }

		public bool Enabled { get; private set; }

		public event Action Elapsed;

		public MonoTimer(float interval, bool repeat = false)
		{
			if (interval <= 0f)
			{
				throw new ArgumentNullException("interval");
			}
			Interval = interval;
			Repeat = repeat;
			FireElapsedOnStop = true;
		}

		public void Start()
		{
			if (!Enabled)
			{
				Enabled = true;
				MonoTimerManager.Instance.StartTimer(this);
			}
		}

		public void Stop()
		{
			if (Enabled)
			{
				MonoTimerManager.Instance.StopTimer(this);
				StopInternal();
			}
		}

		internal IEnumerator StartInternal()
		{
			do
			{
				yield return new WaitForSecondsRealtime(Interval);
				if (this.Elapsed != null)
				{
					this.Elapsed();
				}
			}
			while (Repeat);
			MonoTimerManager.Instance.StopTimer(this);
			Enabled = false;
		}

		internal void StopInternal()
		{
			if (FireElapsedOnStop && this.Elapsed != null)
			{
				this.Elapsed();
			}
			Enabled = false;
		}
	}
}
