using System;
using UnityEngine;

[Serializable]
public class PlayerLocalVideoAwardData
{
	private const int MaxTipsTimes = 3;

	private readonly DateTime defaultDateTime = new DateTime(1970, 1, 1);

	public string lastGetAwardTime;

	public int getCounts;

	public int clickTimes;

	public bool hasDoFiveStar;

	public bool hasClickGuideInfo;

	public bool hasShowFiveStarToday;

	public bool HasDoFiveStar
	{
		get
		{
			return hasDoFiveStar;
		}
		set
		{
			hasDoFiveStar = value;
			Save();
		}
	}

	public bool HasClickGuideInfo
	{
		get
		{
			return hasClickGuideInfo;
		}
		set
		{
			hasClickGuideInfo = value;
			Save();
		}
	}

	public bool HasShowFiveStarToday
	{
		get
		{
			return hasShowFiveStarToday;
		}
		set
		{
			hasShowFiveStarToday = value;
			Save();
		}
	}

	public void GetAward()
	{
		getCounts++;
		lastGetAwardTime = TimeTools.DateTimeToString(DateTime.Now);
		Save();
	}

	public void Click()
	{
		clickTimes++;
		Save();
	}

	public bool IsClickMaxTimes()
	{
		return clickTimes >= 3;
	}

	public void Load()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERLOCALVIDEOAWARD);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
		if (string.IsNullOrEmpty(lastGetAwardTime))
		{
			lastGetAwardTime = TimeTools.DateTimeToString(defaultDateTime);
		}
		if (!hasDoFiveStar)
		{
			DateTime start = TimeTools.StringToDatetime(PlayerDataModule.Instance.GetAppLaunchTime());
			DateTime now = DateTime.Now;
			if (!TimeTools.IsSameDay(start, now))
			{
				HasShowFiveStarToday = false;
			}
		}
	}

	public void CheckNextDay()
	{
		if (getCounts != 0 && !TimeTools.IsSameDay(TimeTools.StringToDatetime(lastGetAwardTime), DateTime.Now))
		{
			getCounts = 0;
			lastGetAwardTime = TimeTools.DateTimeToString(defaultDateTime);
			Save();
		}
	}

	private void Save()
	{
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERLOCALVIDEOAWARD, JsonUtility.ToJson(this));
	}

	public bool NeedShowFiveStar()
	{
		if (Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId == 10000)
		{
			return false;
		}
		if (HasDoFiveStar)
		{
			return false;
		}
		if (HasShowFiveStarToday)
		{
			return false;
		}
		if (Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule).Progress >= 100)
		{
			return true;
		}
		return false;
	}
}
