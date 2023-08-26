using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class SynChangeDataBuffer
{
	public List<SynChangeGoodsDataItem> synDataList = new List<SynChangeGoodsDataItem>();

	[NonSerialized]
	private bool isSyning;

	[NonSerialized]
	private static readonly object synLock = new object();

	[NonSerialized]
	private float lastSynTime;

	public void ChangeGoods(int goodsId, double resultNum)
	{
		lock (synLock)
		{
			SynChangeGoodsDataItem synChangeGoodsDataItem = new SynChangeGoodsDataItem();
			synChangeGoodsDataItem.goodsId = goodsId;
			synChangeGoodsDataItem.type = SynDataType.GOODS;
			synChangeGoodsDataItem.resultNum = resultNum;
			synDataList.Add(synChangeGoodsDataItem);
			SaveToLocal();
		}
	}

	public void SynChangeData(bool isForce = false)
	{
		if (((synDataList.Count > 0 && Time.realtimeSinceStartup - lastSynTime > (float)GameCommon.SYSGOODSCHANGE_MIN_NUM) || isForce) && !isSyning)
		{
			isSyning = true;
			List<SynChangeGoodsDataItem> list = new List<SynChangeGoodsDataItem>();
			for (int i = 0; i < synDataList.Count; i++)
			{
				list.Add(synDataList[i]);
			}
			SendSynChangeGoodsData(list);
			lastSynTime = Time.realtimeSinceStartup;
		}
	}

	private void SendSynChangeGoodsData(List<SynChangeGoodsDataItem> sendingList)
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
				SynChangeData();
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
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERSYNGOODSDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
	}

	public void SaveToLocal()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERSYNGOODSDATA, values);
	}

	public void Clear()
	{
		synDataList.Clear();
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERSYNGOODSDATA, "");
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
