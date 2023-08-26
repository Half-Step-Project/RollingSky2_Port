using System;
using System.Collections.Generic;
using UnityEngine;

public class SynChangePlayerPropertyBuffer
{
	public List<PlayerServerPropertyData> synDataList = new List<PlayerServerPropertyData>();

	[NonSerialized]
	private bool isSyning;

	[NonSerialized]
	private static readonly object synLock = new object();

	public void ChangePropertyData(PlayerLocalPropertyData propertyData)
	{
		lock (synLock)
		{
			PlayerServerPropertyData playerServerPropertyData = new PlayerServerPropertyData();
			playerServerPropertyData.InitFromLocalData(propertyData);
			synDataList.Add(playerServerPropertyData);
			SaveToLocal();
		}
	}

	public void SynPropertylData(bool isForce)
	{
		if ((synDataList.Count >= GameCommon.SYSPROPERTYCHANGE_MIN_NUM || isForce) && !isSyning)
		{
			isSyning = true;
			lock (synLock)
			{
				SendSynChangePropertyData(synDataList[0]);
			}
		}
	}

	private void SendSynChangePropertyData(PlayerServerPropertyData propertyData)
	{
		NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
		netMessageData.messageId = NetMessageId.POST_PLAYERPROPERTYCHANGE;
		string text = (netMessageData.content = JsonUtility.ToJson(propertyData));
		Debug.Log("SendSynChangePropertyDataContent:" + netMessageData);
		netMessageData.succesHandler = delegate(NetMessageResultData content)
		{
			Debug.Log("SendSynChangePropertyData:" + content);
			isSyning = false;
			if (content.code == 0)
			{
				lock (synLock)
				{
					synDataList.Remove(propertyData);
					SaveToLocal();
				}
				SynPropertylData(false);
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
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERSYNPROPERTYDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
	}

	public void SaveToLocal()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERSYNPROPERTYDATA, values);
	}

	public void Clear()
	{
		synDataList.Clear();
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERSYNPROPERTYDATA, "");
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
