using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerLocalRecordData
{
	public int homeTutorla;

	public int firstEnter_Level_1;

	public int firstEnter_Level_2;

	public int firstEnter_Level_3;

	public int firstEnter_Level_4;

	public int firstEnter_Level_5;

	public int firstEnter_Level_6;

	public int firstEnter_TimeOutForm;

	public int firstPlayAD_GetPower;

	public int firstWaitTime_GetPower;

	public int show_get_RebirthGoods;

	public int firstOpenLevelTarget;

	public int firstOpenUseAssets;

	public long m_noConsumePowerTimeStamp;

	public int m_timeOutFormOpenCount;

	public int m_watchPowerAdNum;

	public int m_IsbuffShowFinishTutorial;

	public int m_ScreenPluginsFormOpenTime;

	public int m_shopRemoveAdFormOpenCount;

	public int m_currentShowRemoveAdForScreenCount;

	public int m_SequenceLoginAwardFormCount;

	public int m_MenuFormOpenCount;

	public List<InfoCGameFillerData> m_infocGamefillerData = new List<InfoCGameFillerData>();

	public int isFinishedUsingAssetTutorial;

	public int MenuFormOpenCount
	{
		get
		{
			return m_MenuFormOpenCount;
		}
	}

	public int SequenceLoginAwardFormCount
	{
		get
		{
			return m_SequenceLoginAwardFormCount;
		}
	}

	public int IsbuffShowFinishTutorial
	{
		get
		{
			return m_IsbuffShowFinishTutorial;
		}
		set
		{
			m_IsbuffShowFinishTutorial = value;
			if (m_IsbuffShowFinishTutorial > 0)
			{
				SaveToLocal();
			}
		}
	}

	public int WatchPowerAdNum
	{
		get
		{
			return m_watchPowerAdNum;
		}
		set
		{
			if (m_watchPowerAdNum != value)
			{
				m_watchPowerAdNum = value;
				if (m_watchPowerAdNum < 0)
				{
					m_watchPowerAdNum = 0;
				}
				SaveToLocal();
			}
		}
	}

	public int TimeOutFormOpenTime
	{
		get
		{
			return m_timeOutFormOpenCount;
		}
		set
		{
			m_timeOutFormOpenCount = value;
			SaveToLocal();
		}
	}

	public int IsFinishedUsingAssetTutorial
	{
		get
		{
			return isFinishedUsingAssetTutorial;
		}
		set
		{
			isFinishedUsingAssetTutorial = value;
			if (isFinishedUsingAssetTutorial > 0)
			{
				SaveToLocal();
			}
		}
	}

	public int FirstOpenUseAssets
	{
		get
		{
			return firstOpenUseAssets;
		}
		set
		{
			firstOpenUseAssets = value;
			if (firstOpenUseAssets > 0)
			{
				SaveToLocal();
			}
		}
	}

	public int FirstOpenLevelTarget
	{
		get
		{
			return firstOpenLevelTarget;
		}
		set
		{
			firstOpenLevelTarget = value;
			if (firstOpenLevelTarget > 0)
			{
				SaveToLocal();
			}
		}
	}

	public void AddNoConsumePowerTime(int milSec)
	{
		if (m_noConsumePowerTimeStamp < PlayerDataModule.Instance.ServerTime)
		{
			m_noConsumePowerTimeStamp = PlayerDataModule.Instance.ServerTime;
		}
		m_noConsumePowerTimeStamp += milSec;
		SaveToLocal();
	}

	public bool IsInNoConsumePowerTime()
	{
		return m_noConsumePowerTimeStamp > PlayerDataModule.Instance.ServerTime;
	}

	public long LeftNoConsumePowerTime()
	{
		return m_noConsumePowerTimeStamp - PlayerDataModule.Instance.ServerTime;
	}

	public void Init()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERRECORDDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
	}

	public void SaveToLocal()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERRECORDDATA, values);
	}

	public int GetInfoCGameFillerState(int levelId)
	{
		int result = -1;
		InfoCGameFillerData infoCGameFillerData = m_infocGamefillerData.Find((InfoCGameFillerData x) => x.levelId == levelId);
		if (infoCGameFillerData != null)
		{
			result = infoCGameFillerData.state;
		}
		return result;
	}

	public void SetInfoCGameFillerState(int levelId, int state)
	{
		InfoCGameFillerData infoCGameFillerData = m_infocGamefillerData.Find((InfoCGameFillerData x) => x.levelId == levelId);
		if (infoCGameFillerData == null)
		{
			infoCGameFillerData = new InfoCGameFillerData();
			infoCGameFillerData.levelId = levelId;
		}
		infoCGameFillerData.state = state;
		SaveToLocal();
	}

	public void AddScreenPluginFormOpenTime()
	{
		m_ScreenPluginsFormOpenTime++;
		m_currentShowRemoveAdForScreenCount++;
		SaveToLocal();
	}

	public void AddShopRemoveAdFormOpenCount()
	{
		m_shopRemoveAdFormOpenCount++;
		SaveToLocal();
	}

	public void AddOpenSequenceLoginAwardFormCount()
	{
		m_SequenceLoginAwardFormCount++;
		SaveToLocal();
	}

	public void AddOpenMenuFormCount()
	{
		m_MenuFormOpenCount++;
		SaveToLocal();
	}
}
