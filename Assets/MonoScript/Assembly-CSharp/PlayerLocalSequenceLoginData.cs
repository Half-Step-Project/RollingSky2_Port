using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

[Serializable]
public class PlayerLocalSequenceLoginData
{
	public long m_sequenceLoginStartTimeStamp;

	public List<int> m_getAwardRecord;

	public string lastGetAwardTime;

	[NonSerialized]
	private bool isSyning;

	public bool IsGetToday { get; set; }

	public void Init()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERLOCALSEQUENCELOGINDATA);
		if (string.IsNullOrEmpty(config))
		{
			m_sequenceLoginStartTimeStamp = PlayerDataModule.Instance.ServerTime;
			int capacity = Mod.DataTable.Get<SequenceLoginAward_table>().Records.Length;
			m_getAwardRecord = new List<int>(capacity);
			for (int i = 0; i < m_getAwardRecord.Capacity; i++)
			{
				m_getAwardRecord.Add(0);
			}
			Save();
		}
		else
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
	}

	public void Save()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERLOCALSEQUENCELOGINDATA, values);
	}

	public bool IsEnd()
	{
		bool flag = true;
		for (int i = 0; i < m_getAwardRecord.Capacity; i++)
		{
			flag &= m_getAwardRecord[i] == 1;
		}
		return flag;
	}

	public int NowIndex()
	{
		if (m_getAwardRecord == null)
		{
			return -1;
		}
		int result = -1;
		for (int i = 0; i < m_getAwardRecord.Capacity; i++)
		{
			if (m_getAwardRecord[i] == 0)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public int GetAwardRecord(int index)
	{
		return m_getAwardRecord[index];
	}

	public void SetAwardRecord(int index)
	{
		m_getAwardRecord[index] = 1;
		lastGetAwardTime = TimeTools.DateTimeToString(DateTime.Now);
		SendSynChangeData();
		Save();
	}

	public bool HadGetToday()
	{
		if (string.IsNullOrEmpty(lastGetAwardTime))
		{
			return false;
		}
		return TimeTools.IsSameDay(TimeTools.StringToDatetime(lastGetAwardTime), DateTime.Now);
	}

	public bool IsCanShowGetForm()
	{
		if (IsGetToday)
		{
			return false;
		}
		if (IsEnd())
		{
			return false;
		}
		int num = NowIndex();
		if (num >= m_getAwardRecord.Count || num < 0)
		{
			return false;
		}
		if (GetAwardRecord(num) == 0)
		{
			return true;
		}
		return false;
	}

	public int CompareWithServerData(PlayerAllServerData serverData)
	{
		return NowIndex() - serverData.sequenceLoginData.NowIndex();
	}

	public void ResetFromServerData(PlayerAllServerData serverData)
	{
		int num = serverData.sequenceLoginData.NowIndex();
		for (int i = 0; i < m_getAwardRecord.Capacity; i++)
		{
			if (i < num)
			{
				m_getAwardRecord[i] = 1;
			}
		}
		lastGetAwardTime = serverData.sequenceLoginData.lastGetAwardTime;
		m_sequenceLoginStartTimeStamp = serverData.sequenceLoginData.m_sequenceLoginStartTimeStamp;
	}

	public void SendSynChangeData()
	{
		if (!isSyning)
		{
			isSyning = true;
			NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
			netMessageData.messageId = NetMessageId.POST_LOGINDATA;
			string text = (netMessageData.content = JsonUtility.ToJson(this));
			netMessageData.succesHandler = delegate
			{
				isSyning = false;
			};
			netMessageData.errorHandler = delegate
			{
				isSyning = false;
			};
			MonoSingleton<NetWorkManager>.Instacne.Send(netMessageData);
		}
	}
}
