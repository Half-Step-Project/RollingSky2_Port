using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerLocalAllMessionData
{
	public List<PlayerLocalMessionData> missionLocalData = new List<PlayerLocalMessionData>();

	public void InitFromLocal()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERLOCALMESSIONYDATA);
		if (!string.IsNullOrEmpty(config))
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
	}

	public void SaveToLocal()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERLOCALMESSIONYDATA, values);
	}

	public bool IsFinishMession(int messionId)
	{
		if (missionLocalData.Find((PlayerLocalMessionData x) => x.messsionId == messionId) != null)
		{
			return true;
		}
		return false;
	}

	public bool FinishMession(int messionId)
	{
		PlayerLocalMessionData playerLocalMessionData = missionLocalData.Find((PlayerLocalMessionData x) => x.messsionId == messionId);
		if (playerLocalMessionData != null)
		{
			return false;
		}
		playerLocalMessionData = new PlayerLocalMessionData();
		playerLocalMessionData.messsionId = messionId;
		missionLocalData.Add(playerLocalMessionData);
		SaveToLocal();
		return true;
	}

	public PlayerLocalMessionData GetMessionDataById(int messionId)
	{
		return missionLocalData.Find((PlayerLocalMessionData x) => x.messsionId == messionId);
	}

	public int CompareWithServerData(PlayerAllServerData server)
	{
		if (server == null || server.messionDataList == null)
		{
			return 1;
		}
		int num = 0;
		if (missionLocalData.Count > server.messionDataList.Count)
		{
			return 1;
		}
		if (missionLocalData.Count < server.messionDataList.Count)
		{
			return -1;
		}
		return 0;
	}

	public void ResetFromServerData(PlayerAllServerData serverData)
	{
		if (serverData != null && serverData.messionDataList != null)
		{
			missionLocalData.Clear();
			PlayerLocalMessionData playerLocalMessionData = null;
			for (int i = 0; i < serverData.messionDataList.Count; i++)
			{
				playerLocalMessionData = new PlayerLocalMessionData();
				playerLocalMessionData.InitFromServerData(serverData.messionDataList[i]);
				missionLocalData.Add(playerLocalMessionData);
			}
			SaveToLocal();
		}
	}
}
