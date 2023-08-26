using System;

public class ShopAdTimer
{
	protected DateTime m_currentTime;

	protected DateTime m_todayResetTime;

	protected DateTime m_tomorrowResetTime;

	protected TimeSpan m_nextResetTimeSpan;

	public int ResetTime
	{
		get
		{
			return GameCommon.shopFreeAdResetTime;
		}
	}

	public string SaveTimeKey
	{
		get
		{
			return PlayerLocalDatakey.SHOPFREEADRESETTIME;
		}
	}

	public string ShowCountKey
	{
		get
		{
			return PlayerLocalDatakey.SHOPFREEADTODAYSHOWCOUNT;
		}
	}

	public int ShowCount
	{
		get
		{
			return int.Parse(EncodeConfig.getConfig(ShowCountKey, "0"));
		}
		set
		{
			EncodeConfig.setConfig(ShowCountKey, value.ToString());
		}
	}

	public int ShowMaxCount
	{
		get
		{
			return GameCommon.shopFreeAdTodayShowMaxCount;
		}
	}

	public void OnInit()
	{
		m_currentTime = DateTime.Now;
		m_todayResetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, ResetTime, 0, 0);
		m_tomorrowResetTime = m_todayResetTime.AddDays(1.0);
		string text = EncodeConfig.getConfig(SaveTimeKey);
		if (string.IsNullOrEmpty(text))
		{
			text = TimeTools.DateTimeToString(m_tomorrowResetTime);
			EncodeConfig.setConfig(SaveTimeKey, text);
		}
		TimeSpan nextResetTimeSpan = TimeTools.StringToDatetime(text) - m_currentTime;
		if (nextResetTimeSpan.TotalMilliseconds <= 0.0)
		{
			text = TimeTools.DateTimeToString(m_tomorrowResetTime);
			EncodeConfig.setConfig(SaveTimeKey, text);
			OnRefreshData();
			m_nextResetTimeSpan = m_tomorrowResetTime - m_currentTime;
		}
		else
		{
			m_nextResetTimeSpan = nextResetTimeSpan;
		}
	}

	protected void OnRefreshData()
	{
		ShowCount = 0;
	}

	public bool IsCanShowForCount()
	{
		return ShowCount < ShowMaxCount;
	}
}
