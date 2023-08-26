using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SynChangePlayerInstrumentDataBuffer
{
	public List<PlayerServerInstrumentData> synDataList = new List<PlayerServerInstrumentData>();

	[NonSerialized]
	private bool isSyning;

	[NonSerialized]
	private static readonly object synLock = new object();

	public void ChangeInstrumentData(List<PlayerLocalInstrumentData> instrumentDataList)
	{
		lock (synLock)
		{
			PlayerServerInstrumentData playerServerInstrumentData = null;
			for (int i = 0; i < instrumentDataList.Count; i++)
			{
				playerServerInstrumentData = new PlayerServerInstrumentData();
				playerServerInstrumentData.InitFromLocalData(instrumentDataList[i]);
				synDataList.Add(playerServerInstrumentData);
			}
			SaveToLocal();
		}
	}

	public void SynInstrumentData(bool isForce)
	{
		if ((synDataList.Count >= GameCommon.SYSINSTRUMENTCHANGE_MIN_NUM || isForce) && !isSyning)
		{
			isSyning = true;
			List<PlayerServerInstrumentData> list = new List<PlayerServerInstrumentData>();
			for (int i = 0; i < synDataList.Count; i++)
			{
				list.Add(synDataList[i]);
			}
			SendSynChangeInstrumentData(list);
		}
	}

	private void SendSynChangeInstrumentData(List<PlayerServerInstrumentData> sendingList)
	{
		NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
		netMessageData.messageId = NetMessageId.POST_PLAYERINSTUMENTCHANGE;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[");
		string text = "";
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
		Debug.Log("SendSynChangeInstrumentDataContent:" + (netMessageData.content = stringBuilder.ToString()));
		netMessageData.succesHandler = delegate(NetMessageResultData content)
		{
			Debug.Log("SendSynChangeInstrumentData:" + content);
			isSyning = false;
			if (content.code == 0)
			{
				lock (synLock)
				{
					synDataList.RemoveRange(0, sendingList.Count);
					SaveToLocal();
				}
				SynInstrumentData(false);
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
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERSYNINSTRUMENTDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
	}

	public void SaveToLocal()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERSYNINSTRUMENTDATA, values);
	}

	public void Clear()
	{
		synDataList.Clear();
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERSYNINSTRUMENTDATA, "");
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
