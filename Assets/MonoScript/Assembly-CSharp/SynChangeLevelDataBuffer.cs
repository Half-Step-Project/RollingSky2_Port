using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class SynChangeLevelDataBuffer
{
	public List<SynChangeLevelDataItem> synDataList = new List<SynChangeLevelDataItem>();

	[NonSerialized]
	private bool isSyning;

	[NonSerialized]
	private static readonly object synLock = new object();

	public void ChangeLevelData(int levelId, PlayerLocalLevelData levelData)
	{
		if (MonoSingleton<NetWorkManager>.Instacne.IsSynNetwork)
		{
			lock (synLock)
			{
				SynChangeLevelDataItem synChangeLevelDataItem = new SynChangeLevelDataItem();
				synChangeLevelDataItem.levelId = levelId;
				synChangeLevelDataItem.type = SynDataType.LEVEL;
				PlayerServerLevelData playerServerLevelData = new PlayerServerLevelData();
				playerServerLevelData.InitFromLocalData(levelData);
				synChangeLevelDataItem.result = playerServerLevelData;
				synDataList.Add(synChangeLevelDataItem);
				SaveToLocal();
			}
		}
	}

	public void SynLevelData(bool isForce)
	{
		if (MonoSingleton<NetWorkManager>.Instacne.IsSynNetwork && (synDataList.Count >= GameCommon.SYSLEVELCHANGE_MIN_NUM || isForce) && !isSyning)
		{
			isSyning = true;
			List<SynChangeLevelDataItem> list = new List<SynChangeLevelDataItem>();
			for (int i = 0; i < synDataList.Count; i++)
			{
				list.Add(synDataList[i]);
			}
			SendSynChangeLevelData(list);
		}
	}

	private void SendSynChangeLevelData(List<SynChangeLevelDataItem> sendingList)
	{
		NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
		netMessageData.messageId = NetMessageId.POST_PLAYERCHANGEDATA;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("{\"synDataList\":[");
		string text = JsonUtility.ToJson(this);
		int i = 0;
		for (int count = sendingList.Count; i < count; i++)
		{
			text = JsonUtility.ToJson(sendingList[i]);
			stringBuilder.Append(text);
			if (i == count - 1)
			{
				stringBuilder.Append("]}");
			}
			else
			{
				stringBuilder.Append(",");
			}
		}
		text = (netMessageData.content = stringBuilder.ToString());
		netMessageData.succesHandler = delegate(NetMessageResultData content)
		{
			isSyning = false;
			if (content.code == 0)
			{
				lock (synLock)
				{
					synDataList.RemoveRange(0, sendingList.Count);
					SaveToLocal();
				}
				SynLevelData(false);
			}
		};
		netMessageData.errorHandler = delegate
		{
			isSyning = false;
		};
		MonoSingleton<NetWorkManager>.Instacne.Send(netMessageData);
	}

	public void InitFromLocal()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERSYNLEVELDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
	}

	public void SaveToLocal()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERSYNLEVELDATA, values);
	}

	public void Clear()
	{
		synDataList.Clear();
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERSYNLEVELDATA, "");
	}

	public override string ToString()
	{
		string text = "";
		for (int i = 0; i < synDataList.Count; i++)
		{
			text = text + synDataList[i].ToString() + "\n";
		}
		return text;
	}
}
