using System;
using UnityEngine;

[Serializable]
public class PlayerLuckTurnTableLocalData
{
	[SerializeField]
	private int _freeCount;

	[SerializeField]
	private string _lastSaveTimestamp;

	public int MaxFreeCount
	{
		get
		{
			return 3;
		}
	}

	public int FreeCount
	{
		get
		{
			return _freeCount;
		}
		set
		{
			Refresh(value);
		}
	}

	public void Init()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.LUCKY_TURNTABLE_DATA);
		if (string.IsNullOrEmpty(config))
		{
			Refresh(MaxFreeCount);
			return;
		}
		JsonUtility.FromJsonOverwrite(config, this);
		if (!TimeTools.IsSameDay(TimeTools.StringToDatetime(_lastSaveTimestamp), DateTime.Now))
		{
			Refresh(MaxFreeCount);
		}
	}

	private void Refresh(int freeCount)
	{
		if (freeCount >= 0 && freeCount <= MaxFreeCount)
		{
			_freeCount = freeCount;
			_lastSaveTimestamp = TimeTools.DateTimeToString(DateTime.Now);
			string values = JsonUtility.ToJson(this);
			EncodeConfig.setConfig(PlayerLocalDatakey.LUCKY_TURNTABLE_DATA, values);
		}
	}
}
