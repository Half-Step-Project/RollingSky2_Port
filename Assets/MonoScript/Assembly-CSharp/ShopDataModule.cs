using System;
using System.Collections.Generic;
using Foundation;
using RS2;

public class ShopDataModule : IDataModule
{
	private delegate void OnResetFunction(TimeSpan timeSpan);

	public ShopAdTimer m_shopAdTimer = new ShopAdTimer();

	public DataNames GetName()
	{
		return DataNames.ShopDataModule;
	}

	public ShopDataModule()
	{
		m_shopAdTimer.OnInit();
		CheckShopDataForTime();
	}

	public int GetBuyCountByID(int productID)
	{
		return int.Parse(EncodeConfig.getConfig(GetSaveBuyCountKeyByID(productID), "0"));
	}

	public bool IsBeyondLimits(int productID)
	{
		Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>().Get(productID);
		bool flag = true;
		if (shops_shopTable.Type == 10 || shops_shopTable.Type == 13)
		{
			flag = false;
			Mod.DataTable.Get<GoodsTeam_goodsTeamTable>().Get(shops_shopTable.GoodsTeamid);
			Dictionary<int, int> dictionary = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(shops_shopTable.GoodsTeamid, shops_shopTable.Count);
			foreach (KeyValuePair<int, int> item in dictionary)
			{
				if ((item.Key == GameCommon.ORIGIN_REBIRTH_FREE || item.Key == GameCommon.START_FREE_SHIELD || item.Key == GameCommon.EVERY_DAY_GIVE_POWER) && PlayerDataModule.Instance.GetPlayGoodsNum(item.Key) > 0.0)
				{
					flag = true;
					break;
				}
			}
			if (dictionary.Count == 1)
			{
				foreach (KeyValuePair<int, int> item2 in dictionary)
				{
					if (item2.Key == GameCommon.REMOVE_AD && PlayerDataModule.Instance.GetPlayGoodsNum(item2.Key) > 0.0)
					{
						return true;
					}
				}
				return flag;
			}
		}
		else
		{
			int buyCountByID = GetBuyCountByID(productID);
			flag = shops_shopTable.MaxBuyCount >= 0 && buyCountByID >= shops_shopTable.MaxBuyCount;
		}
		return flag;
	}

	public bool IsWithinTimeLimit(int productID)
	{
		Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>().Get(productID);
		bool result = false;
		if (!string.IsNullOrEmpty(shops_shopTable.OnTime) && !string.IsNullOrEmpty(shops_shopTable.OutTime))
		{
			if (shops_shopTable.OnTime == "-1" && shops_shopTable.OutTime == "-1")
			{
				result = true;
			}
			else
			{
				DateTime localTime = TimeTools.GetLocalTime();
				DateTime dateTime = TimeTools.StringToDatetime(shops_shopTable.OnTime);
				DateTime dateTime2 = TimeTools.StringToDatetime(shops_shopTable.OutTime);
				if (dateTime <= localTime && localTime <= dateTime2)
				{
					result = true;
				}
			}
		}
		return result;
	}

	public bool IsCanShowShopForm(int productID)
	{
		if (IsWithinTimeLimit(productID))
		{
			return !IsBeyondLimits(productID);
		}
		return false;
	}

	private void SetBuyCountByID(int productID, int count)
	{
		EncodeConfig.setConfig(GetSaveBuyCountKeyByID(productID), count.ToString());
	}

	public void AddBuyCountByID(int productID)
	{
		int buyCountByID = GetBuyCountByID(productID);
		buyCountByID++;
		SetBuyCountByID(productID, buyCountByID);
	}

	private void ResetBuyCountByID(int productID, int count = 0)
	{
		SetBuyCountByID(productID, count);
	}

	private void ResetBuyCountByID(params int[] productIDs)
	{
		for (int i = 0; i < productIDs.Length; i++)
		{
			ResetBuyCountByID(productIDs[i]);
		}
	}

	private void ResetBuyCountForNeedIDs()
	{
		ResetBuyCountByID(GetNeedResetIDs());
	}

	public int[] GetNeedResetIDs()
	{
		Shops_shopTable[] records = Mod.DataTable.Get<Shops_shopTable>().Records;
		List<int> list = new List<int>(records.Length);
		for (int i = 0; i < records.Length; i++)
		{
			if (records[i].IsResetForDay == 1)
			{
				list.Add(records[i].Id);
			}
		}
		return list.ToArray();
	}

	private string GetSaveBuyCountKeyByID(int productID)
	{
		return string.Format("{0}_{1}", PlayerLocalDatakey.SHOPBUYCOUNT, productID);
	}

	public void CheckShopDataForTime()
	{
		OnResetForTime(DateTime.Now, PlayerLocalDatakey.SHOPRESETTIME, GameCommon.shopResetTime, OnResetNeedIDs);
	}

	private void OnResetNeedIDs(TimeSpan timeSpan)
	{
		ResetBuyCountForNeedIDs();
	}

	private void OnResetForTime(DateTime currentTime, string configKey, int targetHour, OnResetFunction resetFunction)
	{
		TimeSpan minValue = TimeSpan.MinValue;
		DateTime dateTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, targetHour, 0, 0).AddDays(1.0);
		string text = EncodeConfig.getConfig(configKey);
		if (string.IsNullOrEmpty(text))
		{
			text = TimeTools.DateTimeToString(dateTime);
			EncodeConfig.setConfig(configKey, text);
		}
		TimeSpan timeSpan = TimeTools.StringToDatetime(text) - currentTime;
		if (timeSpan.TotalMilliseconds <= 0.0)
		{
			text = TimeTools.DateTimeToString(dateTime);
			EncodeConfig.setConfig(configKey, text);
			if (resetFunction != null)
			{
				resetFunction(minValue);
			}
			minValue = dateTime - currentTime;
		}
		else
		{
			minValue = timeSpan;
		}
	}
}
