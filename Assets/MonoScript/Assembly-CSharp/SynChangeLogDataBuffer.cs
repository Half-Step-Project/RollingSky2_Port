using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class SynChangeLogDataBuffer
{
	public List<SynChangeLogDataItem> synDataList = new List<SynChangeLogDataItem>();

	[NonSerialized]
	private bool isSyning;

	[NonSerialized]
	private static readonly object synLock = new object();

	public void ChangeGoods(int goodsId, long changeNum, AssertChangeType changeType)
	{
		lock (synLock)
		{
			SynChangeLogDataItem synChangeLogDataItem = new SynChangeLogDataItem();
			synChangeLogDataItem.goodsId = goodsId;
			synChangeLogDataItem.type = SynDataType.GOODS;
			synChangeLogDataItem.changeNum = changeNum;
			synChangeLogDataItem.changeType = changeType;
			synChangeLogDataItem.timeStamp = TimeTools.ChangeUnixTime(DateTime.Now);
			synDataList.Add(synChangeLogDataItem);
			SaveToLocal();
		}
	}

	public void SynChangeData(bool isForce)
	{
		if ((synDataList.Count >= GameCommon.SYSGOODSCHANGE_MIN_NUM || isForce) && !isSyning)
		{
			isSyning = true;
			List<SynChangeLogDataItem> list = new List<SynChangeLogDataItem>();
			for (int i = 0; i < synDataList.Count; i++)
			{
				list.Add(synDataList[i]);
			}
			SendSynChangeLogData(list);
		}
	}

	private void SendSynChangeLogData(List<SynChangeLogDataItem> sendingList)
	{
		NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
		netMessageData.messageId = NetMessageId.POST_PLAYERCHANGELOGDATA;
		SynChangeLogDataItem synChangeLogDataItem = synDataList[0];
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
		netMessageData.content = (netMessageData.content = stringBuilder.ToString());
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
				SynChangeData(false);
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
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERSYNLOGDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
	}

	public void SaveToLocal()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERSYNLOGDATA, values);
	}

	public void Clear()
	{
		synDataList.Clear();
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERSYNLOGDATA, "");
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
