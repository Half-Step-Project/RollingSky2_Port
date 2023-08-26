using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

[Serializable]
public class PlayerLocalLevelSeriesData
{
	[Serializable]
	public class LocalLevelSeriesData
	{
		public int levelSeriesId;

		public int isAllStarPlay;

		public int isPerfectAllStarPlay;

		public LocalLevelSeriesData(int levelSeriesId)
		{
			this.levelSeriesId = levelSeriesId;
			isAllStarPlay = 0;
			isPerfectAllStarPlay = 0;
		}
	}

	public List<LocalLevelSeriesData> levelSeriesDatas = new List<LocalLevelSeriesData>();

	public int motivateAwardTimes;

	[NonSerialized]
	public Dictionary<int, LocalLevelSeriesData> levelSeriesDataDic = new Dictionary<int, LocalLevelSeriesData>();

	[NonSerialized]
	private bool isSyning;

	public void Init()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERLOCALLEVELSERIESDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
		else
		{
			LevelSeries_table[] records = Mod.DataTable.Get<LevelSeries_table>().Records;
			for (int i = 0; i < records.Length; i++)
			{
				LocalLevelSeriesData item = new LocalLevelSeriesData(records[i].Id);
				levelSeriesDatas.Add(item);
			}
		}
		ResetDic();
	}

	public void Save()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERLOCALLEVELSERIESDATA, values);
	}

	public LocalLevelSeriesData GetOrAddLocalLevelSeriesData(int levelSeriesId)
	{
		if (!levelSeriesDataDic.ContainsKey(levelSeriesId))
		{
			LocalLevelSeriesData localLevelSeriesData = new LocalLevelSeriesData(levelSeriesId);
			levelSeriesDatas.Add(localLevelSeriesData);
			levelSeriesDataDic.Add(levelSeriesId, localLevelSeriesData);
			Save();
		}
		return levelSeriesDataDic[levelSeriesId];
	}

	public void AddMotivateAwardTimes()
	{
		if (!IsInMotivateAwardCD())
		{
			motivateAwardTimes++;
			Save();
		}
	}

	public void ResetMotivateAward()
	{
		motivateAwardTimes = 0;
		Save();
	}

	public bool IsInMotivateAwardCD()
	{
		return motivateAwardTimes >= GameCommon.maxMotivateAwardTimes;
	}

	public int GetLeftTimes()
	{
		return GameCommon.maxMotivateAwardTimes - motivateAwardTimes;
	}

	private MotivateReward_needEnergy GetNeedEnergyTableData()
	{
		int a = motivateAwardTimes + 1;
		int b = Mod.DataTable.Get<MotivateReward_needEnergy>().Records.Length;
		a = Mathf.Min(a, b);
		return Mod.DataTable.Get<MotivateReward_needEnergy>()[a];
	}

	public bool IsRare()
	{
		return GetNeedEnergyTableData().Rare == 1;
	}

	public void SetLocalLevelIsAllStarPlay(int levelSeriesId, int isPlay)
	{
		GetOrAddLocalLevelSeriesData(levelSeriesId).isAllStarPlay = isPlay;
		Save();
	}

	public void SetLocalLevelIsPerfectAllStarPlay(int levelSeriesId, int isPlay)
	{
		GetOrAddLocalLevelSeriesData(levelSeriesId).isPerfectAllStarPlay = isPlay;
		Save();
	}

	public int CompareWithServerData(PlayerAllServerData allServerData)
	{
		if (allServerData.levelSeriesData == null)
		{
			return 1;
		}
		if (allServerData.levelSeriesData.motivateAwardTimes < motivateAwardTimes)
		{
			return 1;
		}
		if (allServerData.levelSeriesData.motivateAwardTimes > motivateAwardTimes)
		{
			return -1;
		}
		return 0;
	}

	public void ResetFromServerData(PlayerAllServerData allServerData)
	{
		levelSeriesDatas.Clear();
		levelSeriesDatas.AddRange(allServerData.levelSeriesData.levelSeriesDatas);
		ResetDic();
		Save();
	}

	private void ResetDic()
	{
		levelSeriesDataDic.Clear();
		foreach (LocalLevelSeriesData levelSeriesData in levelSeriesDatas)
		{
			levelSeriesDataDic.Add(levelSeriesData.levelSeriesId, levelSeriesData);
		}
	}

	public void SendSynChangeData()
	{
		if (!isSyning)
		{
			isSyning = true;
			NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
			netMessageData.messageId = NetMessageId.POST_LEVELSERIESDATA;
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
