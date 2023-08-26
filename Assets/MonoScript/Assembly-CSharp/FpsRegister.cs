using System.Collections.Generic;
using Foundation;
using UnityEngine;

public class FpsRegister
{
	private sealed class Progress
	{
		public float m_fps;

		public float m_progress;

		public override string ToString()
		{
			return string.Format("fps = {0}, m_progress= {1}", m_fps, m_progress);
		}
	}

	private sealed class ProgressComparer : Comparer<Progress>
	{
		public override int Compare(Progress x, Progress y)
		{
			return x.m_fps.CompareTo(y.m_fps);
		}
	}

	private float m_UpdateInterval;

	private float m_CurrentFps;

	private int m_Frames;

	private float m_Accumulator;

	private float m_TimeLeft;

	private List<Progress> m_progress = new List<Progress>();

	private bool m_enableDebug;

	private bool m_isSample;

	private bool m_enable;

	private bool m_pause;

	public bool Enable
	{
		get
		{
			return m_enable;
		}
		set
		{
			m_enable = value;
		}
	}

	public InsideGameDataModule GetInsideGameDataModule
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		}
	}

	public float UpdateInterval
	{
		get
		{
			return m_UpdateInterval;
		}
		set
		{
			if (value <= 0f)
			{
				Log.Error("Update interval is invalid.");
				return;
			}
			m_UpdateInterval = value;
			Reset();
		}
	}

	public float CurrentFps
	{
		get
		{
			return m_CurrentFps;
		}
	}

	public bool IsSample
	{
		get
		{
			return m_isSample;
		}
		set
		{
			m_isSample = value;
		}
	}

	public bool Pause
	{
		get
		{
			return m_pause;
		}
		set
		{
			m_pause = value;
		}
	}

	public FpsRegister(float updateInterval = 1f)
	{
		if (updateInterval <= 0f)
		{
			Log.Error("Update interval is invalid.");
			return;
		}
		m_UpdateInterval = updateInterval;
		Reset();
	}

	public void BeginSample()
	{
		if (m_enable)
		{
			if (m_enableDebug)
			{
				Debug.Log("BeginSample ");
			}
			Reset();
			IsSample = true;
			Pause = false;
		}
	}

	public void EndSample()
	{
		if (m_enable)
		{
			if (m_enableDebug)
			{
				Debug.Log("EndSample ");
			}
			IsSample = false;
			if (m_enableDebug)
			{
				Debug.Log("排序前 ");
			}
			for (int i = 0; i < m_progress.Count; i++)
			{
				Debug.Log(i + "= " + m_progress[i]);
			}
			ProgressComparer comparer = new ProgressComparer();
			m_progress.Sort(comparer);
			if (m_enableDebug)
			{
				Debug.Log("排序后 ");
			}
			for (int j = 0; j < m_progress.Count; j++)
			{
				Debug.Log(j + "= " + m_progress[j]);
			}
			if (m_enableDebug)
			{
				Debug.Log("上报 ");
			}
		}
	}

	public void Update(float elapseSeconds, float realElapseSeconds)
	{
		if (!m_enable || !IsSample)
		{
			return;
		}
		m_Frames++;
		m_Accumulator += realElapseSeconds;
		m_TimeLeft -= realElapseSeconds;
		if (!(m_TimeLeft <= 0f))
		{
			return;
		}
		m_CurrentFps = ((m_Accumulator > 0f) ? ((float)m_Frames / m_Accumulator) : 0f);
		if (!m_pause)
		{
			Progress progress = new Progress();
			progress.m_fps = m_CurrentFps;
			progress.m_progress = GetInsideGameDataModule.ProgressPercentage;
			m_progress.Add(progress);
			if (m_enableDebug)
			{
				Debug.Log(m_progress.Count + " = " + progress.ToString());
			}
			m_Frames = 0;
			m_Accumulator = 0f;
			m_TimeLeft += m_UpdateInterval;
		}
	}

	private void Reset()
	{
		m_CurrentFps = 0f;
		m_Frames = 0;
		m_Accumulator = 0f;
		m_TimeLeft = 0f;
		m_progress.Clear();
	}
}
