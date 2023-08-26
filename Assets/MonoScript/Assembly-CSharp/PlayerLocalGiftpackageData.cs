using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

[Serializable]
public class PlayerLocalGiftpackageData
{
	public readonly int[] GiftIdList = new int[8] { 4100, 5001, 5002, 5003, 5004, 5005, 5006, 5007 };

	public int totalAdCount;

	public List<GiftPackBuyRecord> m_giftPackageBuyList = new List<GiftPackBuyRecord>();

	public int videoAwardTableId = -1;

	public float videoAwardRate;

	public bool isPayUser;

	public bool hasPointGiftIcon;

	public int m_TodayHadRecommand;

	[NonSerialized]
	private bool isSyning;

	public int VideoAwardTableId
	{
		get
		{
			return videoAwardTableId;
		}
		set
		{
			videoAwardTableId = value;
			SaveToLocal();
		}
	}

	public VideoAward_videoAwardTable VideoAwardTable
	{
		get
		{
			if (VideoAwardTableId == -1)
			{
				return null;
			}
			return Mod.DataTable.Get<VideoAward_videoAwardTable>().Get(VideoAwardTableId);
		}
	}

	public float VideoAwardRate
	{
		get
		{
			return videoAwardRate;
		}
		set
		{
			videoAwardRate = value;
			SaveToLocal();
		}
	}

	public bool HasPointGiftIcon
	{
		get
		{
			return hasPointGiftIcon;
		}
		set
		{
			hasPointGiftIcon = value;
			SaveToLocal();
		}
	}

	public bool TodayHadRecommand
	{
		get
		{
			return m_TodayHadRecommand == 1;
		}
		set
		{
			if (value)
			{
				m_TodayHadRecommand = 1;
			}
			else
			{
				m_TodayHadRecommand = 0;
			}
			SaveToLocal();
		}
	}

	public bool IsHadRecommand { get; set; }

	private void ChangeGiftBuyRecord(int id, int count)
	{
		if (count > 0)
		{
			GiftPackBuyRecord giftPackBuyRecord = m_giftPackageBuyList.Find((GiftPackBuyRecord x) => x.id == id);
			if (giftPackBuyRecord != null)
			{
				giftPackBuyRecord.count += count;
			}
			else
			{
				giftPackBuyRecord = new GiftPackBuyRecord();
				giftPackBuyRecord.id = id;
				giftPackBuyRecord.count = count;
				m_giftPackageBuyList.Add(giftPackBuyRecord);
			}
			SaveToLocal();
			SendSynChangeData();
		}
	}

	public int GetGiftBuyNum(int id)
	{
		GiftPackBuyRecord giftPackBuyRecord = m_giftPackageBuyList.Find((GiftPackBuyRecord x) => x.id == id);
		if (giftPackBuyRecord != null)
		{
			return giftPackBuyRecord.count;
		}
		return 0;
	}

	public void Init()
	{
		string config = EncodeConfig.getConfig(PlayerLocalDatakey.PLAYERGIFTPACKAGEDATA);
		if (config.Length > 0)
		{
			JsonUtility.FromJsonOverwrite(config, this);
		}
		AddEventHandler();
	}

	public void SaveToLocal()
	{
		string values = JsonUtility.ToJson(this);
		EncodeConfig.setConfig(PlayerLocalDatakey.PLAYERGIFTPACKAGEDATA, values);
	}

	private void AddEventHandler()
	{
		Mod.Event.Subscribe(EventArgs<BuySuccessEventArgs>.EventId, BuySuccessHandler);
		Mod.Event.Subscribe(EventArgs<AdPlayEventArgs>.EventId, PlayAdEventHandler);
	}

	private void PlayAdEventHandler(object sender, Foundation.EventArgs args)
	{
		AdPlayEventArgs adPlayEventArgs = args as AdPlayEventArgs;
		if (adPlayEventArgs != null && adPlayEventArgs.AdState == 1)
		{
			totalAdCount++;
			SaveToLocal();
		}
	}

	private void BuySuccessHandler(object sender, Foundation.EventArgs e)
	{
		BuySuccessEventArgs buySuccessEventArgs = e as BuySuccessEventArgs;
		if (buySuccessEventArgs != null)
		{
			int shopItemId = buySuccessEventArgs.ShopItemId;
			Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[shopItemId];
			ChangeGiftBuyRecord(shopItemId, shops_shopTable.Count);
		}
	}

	public int GetGiftIdIndex(int giftId)
	{
		for (int i = 0; i < GiftIdList.Length; i++)
		{
			if (GiftIdList[i] == giftId)
			{
				return i;
			}
		}
		return -1;
	}

	public int GetNextGiftId()
	{
		int[] giftIdList = GiftIdList;
		foreach (int num in giftIdList)
		{
			if (GetGiftBuyNum(num) <= 0)
			{
				return num;
			}
		}
		return -1;
	}

	private float GetTotalPayCount()
	{
		float num = 0f;
		foreach (GiftPackBuyRecord giftPackageBuy in m_giftPackageBuyList)
		{
			Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[giftPackageBuy.id];
			if (shops_shopTable != null && shops_shopTable.BuyType == 1)
			{
				float result = 0f;
				float.TryParse(shops_shopTable.Price.Substring(1), out result);
				num += result;
			}
		}
		return num;
	}

	private bool IsPayUser()
	{
		if (isPayUser)
		{
			return true;
		}
		if (GetTotalPayCount() >= GameCommon.payUserThreshold)
		{
			isPayUser = true;
			SaveToLocal();
		}
		return isPayUser;
	}

	public VideoAward_videoAwardTable GetVideoAwardTableByPlayerType()
	{
		int num = (IsPayUser() ? 1 : 2);
		VideoAward_videoAwardTable[] records = Mod.DataTable.Get<VideoAward_videoAwardTable>().Records;
		foreach (VideoAward_videoAwardTable videoAward_videoAwardTable in records)
		{
			if (videoAward_videoAwardTable.PlayerType == num)
			{
				return videoAward_videoAwardTable;
			}
		}
		return null;
	}

	public void NewVideoAwardId()
	{
		int num = (IsPayUser() ? 1 : 2);
		List<VideoAward_videoAwardTable> list = new List<VideoAward_videoAwardTable>();
		VideoAward_videoAwardTable[] records = Mod.DataTable.Get<VideoAward_videoAwardTable>().Records;
		foreach (VideoAward_videoAwardTable videoAward_videoAwardTable in records)
		{
			if (videoAward_videoAwardTable.PlayerType == num)
			{
				list.Add(videoAward_videoAwardTable);
			}
		}
		int num2 = 0;
		foreach (VideoAward_videoAwardTable item in list)
		{
			num2 += item.Weight;
		}
		int num3 = 0;
		int num4 = UnityEngine.Random.Range(0, num2);
		foreach (VideoAward_videoAwardTable item2 in list)
		{
			num3 += item2.Weight;
			if (num3 > num4)
			{
				VideoAwardTableId = item2.Id;
				VideoAwardRate = (float)item2.Weight / (float)num2;
				break;
			}
		}
	}

	public bool CanGetFree()
	{
		int nextGiftId = GetNextGiftId();
		Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>().Get(nextGiftId);
		if (shops_shopTable != null)
		{
			return totalAdCount >= shops_shopTable.AdFreeCount;
		}
		return false;
	}

	public int CompareWithServerData(PlayerAllServerData allServerData)
	{
		if (totalAdCount < allServerData.giftData.totalAdCount)
		{
			return -1;
		}
		if (totalAdCount > allServerData.giftData.totalAdCount)
		{
			return 1;
		}
		if (m_giftPackageBuyList.Count < allServerData.giftData.m_giftPackageBuyList.Count)
		{
			return -1;
		}
		if (m_giftPackageBuyList.Count > allServerData.giftData.m_giftPackageBuyList.Count)
		{
			return 1;
		}
		return 0;
	}

	public void ResetFromServerData(PlayerAllServerData allServerData)
	{
		totalAdCount = allServerData.giftData.totalAdCount;
		m_giftPackageBuyList.Clear();
		m_giftPackageBuyList.AddRange(allServerData.giftData.m_giftPackageBuyList);
	}

	public void SendSynChangeData()
	{
		if (!isSyning)
		{
			isSyning = true;
			NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
			netMessageData.messageId = NetMessageId.POST_GIFTDATA;
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
