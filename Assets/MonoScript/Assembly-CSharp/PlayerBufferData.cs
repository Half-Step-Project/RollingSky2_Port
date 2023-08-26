public class PlayerBufferData
{
	private int m_bufferId;

	private long m_bufferTimeStamp;

	public int BufferId
	{
		get
		{
			return m_bufferId;
		}
	}

	public PlayerBufferData(int bufferId)
	{
		m_bufferId = bufferId;
		InitFromLocal();
	}

	public bool IsEnable()
	{
		if (!IsEnableForever())
		{
			return IsTimeEnable();
		}
		return true;
	}

	private string GetLocalDataKey()
	{
		return string.Format("BufferTimeData_{0}", m_bufferId);
	}

	public void AddBufferTime(int milSec)
	{
		if (m_bufferTimeStamp < PlayerDataModule.Instance.ServerTime)
		{
			m_bufferTimeStamp = PlayerDataModule.Instance.ServerTime;
		}
		m_bufferTimeStamp += milSec;
		SaveToLocal();
	}

	public bool IsTimeEnable()
	{
		return m_bufferTimeStamp > PlayerDataModule.Instance.ServerTime;
	}

	public bool IsEnableForever()
	{
		if (m_bufferId == GameCommon.START_FREE_SHIELD || m_bufferId == GameCommon.EVERY_DAY_GIVE_POWER || m_bufferId == GameCommon.ORIGIN_REBIRTH_FREE)
		{
			return PlayerDataModule.Instance.GetPlayGoodsNum(m_bufferId) > 0.0;
		}
		return false;
	}

	public long LeftBufferTime()
	{
		return m_bufferTimeStamp - PlayerDataModule.Instance.ServerTime;
	}

	public void InitFromLocal()
	{
		string config = EncodeConfig.getConfig(GetLocalDataKey());
		if (config.Length > 0)
		{
			long.TryParse(config, out m_bufferTimeStamp);
		}
	}

	private void SaveToLocal()
	{
		string values = m_bufferTimeStamp.ToString();
		EncodeConfig.setConfig(GetLocalDataKey(), values);
	}
}
