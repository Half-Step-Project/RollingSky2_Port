using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerLocalPropertyData
{
	[SerializeField]
	private int m_Level = 1;

	[SerializeField]
	private int m_starLevel;

	[SerializeField]
	private int m_HideLevel = 1;

	[SerializeField]
	private List<PlayerLocalGoodsStatisticalData> m_playerGoodsRecordData = new List<PlayerLocalGoodsStatisticalData>();

	[SerializeField]
	private long m_lastUpLevelTimeStamp = -1L;

	public long LastUpLevelTimeStamp
	{
		get
		{
			return m_lastUpLevelTimeStamp;
		}
		set
		{
			m_lastUpLevelTimeStamp = value;
		}
	}

	public List<PlayerLocalGoodsStatisticalData> PlayerGoodsRecordData
	{
		get
		{
			return m_playerGoodsRecordData;
		}
	}

	public int Level
	{
		get
		{
			return m_Level;
		}
		set
		{
			m_Level = value;
		}
	}

	public int StarLevel
	{
		get
		{
			return m_starLevel;
		}
		set
		{
			m_starLevel = value;
		}
	}

	public int HideLevel
	{
		get
		{
			return m_HideLevel;
		}
		set
		{
			m_HideLevel = value;
		}
	}

	public void InitFromLocal()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERLOCALPROPERTYDATA);
		if (!string.IsNullOrEmpty(config))
		{
			JsonUtility.FromJsonOverwrite(config, this);
			return;
		}
		m_Level = 1;
		m_starLevel = 0;
		m_HideLevel = 1;
		SaveToLocal();
	}

	public void SaveToLocal()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERLOCALPROPERTYDATA, values);
	}

	public void InitFromServerData(PlayerServerPropertyData serverData)
	{
		m_Level = serverData.level;
		m_starLevel = serverData.starLevel;
		m_HideLevel = serverData.hideLevel;
		for (int i = 0; i < serverData.playerGoodsRecordData.Count; i++)
		{
			new PlayerLocalGoodsStatisticalData().InitFromServer(serverData.playerGoodsRecordData[i]);
		}
		SaveToLocal();
	}

	public int CompareWithServerData(PlayerAllServerData server)
	{
		if (server == null || server.propertyData == null)
		{
			return 1;
		}
		if (StarLevel > server.propertyData.starLevel)
		{
			return 1;
		}
		if (Level > server.propertyData.level)
		{
			return 1;
		}
		if (StarLevel == server.propertyData.starLevel && Level == server.propertyData.level)
		{
			return 0;
		}
		return -1;
	}

	public void AddGoodsTotalGetNum(int id, double getNum = 1.0)
	{
		PlayerLocalGoodsStatisticalData playerLocalGoodsStatisticalData = m_playerGoodsRecordData.Find((PlayerLocalGoodsStatisticalData x) => x.goodsId == id);
		if (playerLocalGoodsStatisticalData == null)
		{
			playerLocalGoodsStatisticalData = new PlayerLocalGoodsStatisticalData();
			playerLocalGoodsStatisticalData.totalGetNum = getNum;
		}
		else
		{
			playerLocalGoodsStatisticalData.totalGetNum += getNum;
		}
		SaveToLocal();
	}

	public double GetGoodsTotalGetNum(int id)
	{
		PlayerLocalGoodsStatisticalData playerLocalGoodsStatisticalData = m_playerGoodsRecordData.Find((PlayerLocalGoodsStatisticalData x) => x.goodsId == id);
		if (playerLocalGoodsStatisticalData == null)
		{
			return 0.0;
		}
		return playerLocalGoodsStatisticalData.totalGetNum;
	}

	public void AddGoodsTotalUsedNum(int id, double usedNum = 1.0)
	{
		PlayerLocalGoodsStatisticalData playerLocalGoodsStatisticalData = m_playerGoodsRecordData.Find((PlayerLocalGoodsStatisticalData x) => x.goodsId == id);
		if (playerLocalGoodsStatisticalData == null)
		{
			playerLocalGoodsStatisticalData = new PlayerLocalGoodsStatisticalData();
			playerLocalGoodsStatisticalData.totalUsedNum = usedNum;
		}
		else
		{
			playerLocalGoodsStatisticalData.totalUsedNum += usedNum;
		}
		SaveToLocal();
	}

	public double GetGoodsTotalUsedNum(int id)
	{
		PlayerLocalGoodsStatisticalData playerLocalGoodsStatisticalData = m_playerGoodsRecordData.Find((PlayerLocalGoodsStatisticalData x) => x.goodsId == id);
		if (playerLocalGoodsStatisticalData == null)
		{
			return 0.0;
		}
		return playerLocalGoodsStatisticalData.totalUsedNum;
	}
}
