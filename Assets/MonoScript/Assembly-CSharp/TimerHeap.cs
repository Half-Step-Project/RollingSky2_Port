using System;
using System.Diagnostics;

public class TimerHeap
{
	private static uint nextTimerId;

	private static uint tick;

	private static KeyedPriorityQueue<uint, AbsTimerData, ulong> m_queue;

	private static Stopwatch m_stopwatch;

	private static readonly object timerLock;

	private TimerHeap()
	{
	}

	static TimerHeap()
	{
		timerLock = new object();
		m_queue = new KeyedPriorityQueue<uint, AbsTimerData, ulong>();
		m_stopwatch = new Stopwatch();
	}

	public static uint AddTimer(uint start, uint interval, Action handler)
	{
		TimerData timerData = GetTimerData(new TimerData(), start, interval);
		timerData.Action = handler;
		return AddTimer(timerData);
	}

	public static uint AddTimer<T>(uint start, uint interval, Action<T> handler, T args1)
	{
		TimerData<T> timerData = GetTimerData(new TimerData<T>(), start, interval);
		timerData.Action = handler;
		timerData.Args = args1;
		return AddTimer(timerData);
	}

	public static uint AddTimer<T, U>(uint start, uint interval, Action<T, U> handler, T args1, U args2)
	{
		TimerData<T, U> timerData = GetTimerData(new TimerData<T, U>(), start, interval);
		timerData.Action = handler;
		timerData.Args1 = args1;
		timerData.Args2 = args2;
		return AddTimer(timerData);
	}

	private static uint AddTimer(AbsTimerData p)
	{
		lock (timerLock)
		{
			m_queue.Enqueue(p.Id, p, p.NextTick);
		}
		return p.Id;
	}

	public static void DelTimer(uint timerId)
	{
		lock (timerLock)
		{
			m_queue.Remove(timerId);
		}
	}

	public static uint GetLeftTimeById(uint timerId)
	{
		AbsTimerData absTimerData = null;
		lock (timerLock)
		{
			absTimerData = m_queue.Get(timerId);
		}
		if (absTimerData != null)
		{
			return absTimerData.NextTick - tick;
		}
		return 0u;
	}

	public static uint GetLeftTime(AbsTimerData timer)
	{
		if (timer == null)
		{
			return 0u;
		}
		return timer.NextTick - tick;
	}

	public static AbsTimerData GetTimer(uint timerId)
	{
		AbsTimerData absTimerData = null;
		lock (timerLock)
		{
			return m_queue.Get(timerId);
		}
	}

	public static void Tick()
	{
		tick += (uint)(int)m_stopwatch.ElapsedMilliseconds;
		m_stopwatch.Reset();
		m_stopwatch.Start();
		while (m_queue.Count != 0)
		{
			AbsTimerData absTimerData;
			lock (timerLock)
			{
				absTimerData = m_queue.Peek();
			}
			if (tick < absTimerData.NextTick)
			{
				break;
			}
			lock (timerLock)
			{
				m_queue.Dequeue();
			}
			if (absTimerData.Interval != 0)
			{
				absTimerData.NextTick += absTimerData.Interval;
				lock (timerLock)
				{
					m_queue.Enqueue(absTimerData.Id, absTimerData, absTimerData.NextTick);
				}
				absTimerData.DoAction();
			}
			else
			{
				absTimerData.DoAction();
			}
		}
	}

	private static T GetTimerData<T>(T p, uint start, uint interval) where T : AbsTimerData
	{
		p.Id = ++nextTimerId;
		p.Interval = interval;
		p.NextTick = start + 1 + tick;
		return p;
	}
}
