using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

[Serializable]
public class PlayerLocalCoolPlayPackageData
{
	[SerializeField]
	private string m_lastRecommandTime = "";

	[SerializeField]
	private long m_packageEndTimeStamp;

	[SerializeField]
	private int m_totalDeadCount;

	[SerializeField]
	private List<long> m_deadTimeRecord = new List<long>();

	public bool IsCanReommand(int deadCount = 1)
	{
		if (IsEnable())
		{
			return false;
		}
		if (!TutorialManager.Instance.IsForceTutorialEnd())
		{
			return false;
		}
		m_totalDeadCount++;
		m_deadTimeRecord.Add(PlayerDataModule.Instance.ServerTime);
		Save();
		bool flag = false;
		if (!string.IsNullOrEmpty(m_lastRecommandTime))
		{
			flag = TimeTools.IsSameDay(TimeTools.StringToDatetime(m_lastRecommandTime), DateTime.Now);
		}
		if (m_totalDeadCount >= GameCommon.coolPlayPackageContidtion_Z && !flag)
		{
			return true;
		}
		if (m_deadTimeRecord.Count >= GameCommon.coolPlayPackageContidtion_Y)
		{
			int index = m_deadTimeRecord.Count - GameCommon.coolPlayPackageContidtion_Y;
			if (PlayerDataModule.Instance.ServerTime - m_deadTimeRecord[index] <= GameCommon.coolPlayPackageContidtion_X * 60 * 1000 && !flag)
			{
				return true;
			}
		}
		return false;
	}

	public void Init()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERLOCALCOOLPLAYPACKAGE);
		if (!string.IsNullOrEmpty(config))
		{
			JsonUtility.FromJsonOverwrite(config, this);
			if (!TimeTools.IsSameDay(TimeTools.StringToDatetime(m_lastRecommandTime), DateTime.Now))
			{
				ClearRecord();
				Save();
			}
		}
	}

	public void Save()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERLOCALCOOLPLAYPACKAGE, values);
	}

	private void ClearRecord()
	{
		m_totalDeadCount = 0;
		m_deadTimeRecord.Clear();
	}

	public void AddPacakge()
	{
		int num = 7200000;
		Goods_goodsTable goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>()[GameCommon.REMOVE_AD_TIME];
		if (goods_goodsTable != null)
		{
			num = goods_goodsTable.FunctionNum * 60 * 1000;
		}
		if (m_packageEndTimeStamp < PlayerDataModule.Instance.ServerTime)
		{
			m_packageEndTimeStamp = PlayerDataModule.Instance.ServerTime;
		}
		m_packageEndTimeStamp += num;
		ClearRecord();
		Save();
	}

	public void Record()
	{
		m_lastRecommandTime = TimeTools.DateTimeToString(DateTime.Now);
		ClearRecord();
		Save();
	}

	public bool IsEnable()
	{
		return m_packageEndTimeStamp > PlayerDataModule.Instance.ServerTime;
	}

	public int LeftTime()
	{
		int num = (int)(m_packageEndTimeStamp - PlayerDataModule.Instance.ServerTime);
		if (num <= 0)
		{
			return 0;
		}
		return num;
	}
}
