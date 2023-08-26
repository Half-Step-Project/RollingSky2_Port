using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SynChangePlayerMessionBuffer
{
	public List<PlayerServerMessionData> synDataList = new List<PlayerServerMessionData>();

	[NonSerialized]
	private bool isSyning;

	[NonSerialized]
	private static readonly object synLock = new object();

	public void ChangeMessionData(List<PlayerLocalMessionData> messionList)
	{
		lock (synLock)
		{
			PlayerServerMessionData playerServerMessionData = null;
			for (int i = 0; i < messionList.Count; i++)
			{
				playerServerMessionData = new PlayerServerMessionData();
				playerServerMessionData.InitFromLocalData(messionList[i]);
				synDataList.Add(playerServerMessionData);
			}
			SaveToLocal();
		}
	}

	public void SynMessionlData(bool isForce)
	{
		if ((synDataList.Count >= GameCommon.SYSMESSIONCHANGE_MIN_NUM || isForce) && !isSyning)
		{
			isSyning = true;
			List<PlayerServerMessionData> list = new List<PlayerServerMessionData>();
			for (int i = 0; i < synDataList.Count; i++)
			{
				list.Add(synDataList[i]);
			}
			SendSynChangeMessionData(synDataList);
		}
	}

	private void SendSynChangeMessionData(List<PlayerServerMessionData> sendingList)
	{
		NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
		netMessageData.messageId = NetMessageId.POST_PLAYERMESSIONCHANGE;
		string text = "";
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[");
		int i = 0;
		for (int count = sendingList.Count; i < count; i++)
		{
			text = JsonUtility.ToJson(sendingList[i]);
			stringBuilder.Append(text);
			if (i == count - 1)
			{
				stringBuilder.Append("]");
			}
			else
			{
				stringBuilder.Append(",");
			}
		}
		Debug.Log("SendSynChangeMessionContent:" + (netMessageData.content = stringBuilder.ToString()));
		netMessageData.succesHandler = delegate(NetMessageResultData content)
		{
			Debug.Log("SendSynChangeMessionData:" + content);
			isSyning = false;
			if (content.code == 0)
			{
				lock (synLock)
				{
					synDataList.RemoveRange(0, sendingList.Count);
					SaveToLocal();
				}
				SynMessionlData(false);
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
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERSYNMESSIONDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
	}

	public void SaveToLocal()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERSYNMESSIONDATA, values);
	}

	public void Clear()
	{
		synDataList.Clear();
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERSYNMESSIONDATA, "");
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
